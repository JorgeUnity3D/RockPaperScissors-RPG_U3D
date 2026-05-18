using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS
{
	/// <summary>
	/// Orquesta el loop de combate. Recibe la acción del jugador vía AppEvents,
	/// resuelve el daño con CombatResolver y actualiza CombatUIController.
	/// </summary>
	public class CombatManager : BaseManager
	{
		[SerializeField, ReadOnly] private CombatUIController       _combatUI;
		[SerializeField, ReadOnly] private CombatResultUIController _resultUI;

		private Player        _player;
		private CombatContext _context;
		private Enemy         _enemy;

		private int _currentRound;
		private int _playerHP;
		private int _playerEnergy;

		private int                   _pendingTrainingExp;
		private TrainingHouseModifier _activeTrainingModifier;

		#region SETUP

		public override void SetUp()
		{
			UIService uiService = ServiceLocator.Instance.GetService<UIService>();
			_combatUI = uiService.GetController<CombatUIController>();
			_resultUI = uiService.GetController<CombatResultUIController>();
			_player   = AppContext.Player;
			_context  = AppContext.CombatContext;
		}

		protected override void Subscribe()
		{
			AppEvents.OnCombatActionSelected += OnPlayerActionSelected;
		}

		protected override void UnSubscribe()
		{
			AppEvents.OnCombatActionSelected -= OnPlayerActionSelected;
		}

		#endregion

		#region CONTROL

		public override void Initialize()
		{
			if (_context == null)
			{
				Debug.LogError("[CombatManager] Initialize() -> CombatContext is null. Was TravelManager bypassed?");
				return;
			}

			_currentRound            = 0;
			_playerHP                = _player.MaxHealth.TotalValue;
			_playerEnergy            = _player.InitialEnergy;
			_pendingTrainingExp      = 0;
			_activeTrainingModifier  = FindActiveTrainingModifier();

			EnemyData data = _context.CurrentStep.Enemy.Data;
			_enemy = new Enemy(data);
			_enemy.LanguageRoll();

			Debug.Log($"[CombatManager] ═══════════════ COMBAT START ═══════════════");
			Debug.Log($"[CombatManager] Player → HP:{_playerHP}  Energy:{_playerEnergy}  Lv:{_player.Level}  Mentality:{_player.Mentality.TotalValue}");
			Debug.Log($"[CombatManager] Enemy  → {_enemy.Name}  HP:{_enemy.MaxHealth}  Energy:{_enemy.MaxEnergy}  Lv:{_enemy.Level}  Mentality:{_enemy.StoredMentality}");
			Debug.Log($"[CombatManager] Language → {(_enemy.CurrentLanguage != null ? _enemy.CurrentLanguage.GetType().Name : "none")}");
			Debug.Log($"[CombatManager] Training → {(_activeTrainingModifier != null ? _activeTrainingModifier.Stat.ToString() : "none")}");

			_combatUI.SetData(
				_enemy.Portrait,
				_player.MaxHealth.TotalValue,
				GameConsts.COMBAT_MAX_ENERGY,
				_enemy.MaxHealth,
				_enemy.MaxEnergy
			);
			_combatUI.ShowCanvas();

			BeginRound();
		}

		#endregion

		#region ROUND LOOP

		private void BeginRound()
		{
			_currentRound++;
			_enemy.ActionRoll();
			_combatUI.HideBubbles();

			Debug.Log($"[CombatManager] ─── Round {_currentRound} BEGIN ─── PlayerHP:{_playerHP}/{_player.MaxHealth.TotalValue}  Energy:{_playerEnergy}/{GameConsts.COMBAT_MAX_ENERGY}  │  EnemyHP:{_enemy.CurrentHealth}/{_enemy.MaxHealth}  Energy:{_enemy.CurrentEnergy}/{_enemy.MaxEnergy}");
			Debug.Log($"[CombatManager] Enemy rolled → CurrentAction:{_enemy.CurrentAction}  ThinkingAction:{_enemy.ThinkingAction}  StoredMentality:{_enemy.StoredMentality}");

			int mentalityResult = _enemy.MentalityRollAgainst(_player.Mentality.TotalValue);
			if (mentalityResult <= 0)
			{
				Debug.Log($"[CombatManager] Mentality roll {mentalityResult} ≤ 0 → player reads enemy mind → {_enemy.ThinkingAction}");
				_combatUI.ShowPlayerThoughtBubble(GetActionIcon(_enemy.ThinkingAction));
				_enemy.ResetMentality();
			}
			else
			{
				Debug.Log($"[CombatManager] Mentality roll {mentalityResult} > 0 → enemy conceals thought");
				_combatUI.ShowEnemyThoughtBubble(null);
				_enemy.IncreaseMentality();
			}

			_combatUI.SetActionsInteractable(true);
		}

		private void OnPlayerActionSelected(Actions playerAction)
		{
			Debug.Log($"[CombatManager] Player selected: {playerAction}");
			_combatUI.SetActionsInteractable(false);
			ResolveRound(playerAction, _enemy.CurrentAction);
		}

		private void ResolveRound(Actions playerAction, Actions enemyAction)
		{
			int   playerDamageDealt = 0;
			int   enemyDamageDealt  = 0;
			int   playerCritBonus   = 0;
			int   enemyCritBonus    = 0;
			float playerMult        = 0f;
			float enemyMult         = 0f;

			// ── Super check BEFORE energy changes ────────────────────────────────
			bool playerIsSuper = _playerEnergy >= GameConsts.COMBAT_MAX_ENERGY && playerAction != Actions.ENERGY;
			bool enemyIsSuper  = _enemy.CurrentEnergy >= _enemy.MaxEnergy && enemyAction != Actions.ENERGY;

			Debug.Log($"[CombatManager] RESOLVE  Player:{playerAction}{(playerIsSuper ? "(SUPER)" : "")}  vs  Enemy:{enemyAction}{(enemyIsSuper ? "(SUPER)" : "")}");

			// ── Energy management ─────────────────────────────────────────────────
			if (playerAction == Actions.ENERGY)
				_playerEnergy = Mathf.Min(GameConsts.COMBAT_MAX_ENERGY, _playerEnergy + _player.EnergyRecovery.TotalValue);
			else
				_playerEnergy = Mathf.Max(0, _playerEnergy - PlayerActionCost(playerAction));

			if (enemyAction == Actions.ENERGY)
				_enemy.RecoverEnergy();
			else
				_enemy.PayActionEnergyCost();

			Debug.Log($"[CombatManager] Energy after → Player:{_playerEnergy}/{GameConsts.COMBAT_MAX_ENERGY}  Enemy:{_enemy.CurrentEnergy}/{_enemy.MaxEnergy}");

			// ── Effective power ───────────────────────────────────────────────────
			// ENERGY always has 0 effective; DEFENSE computes with defense stat × its multiplier
			bool playerIsAttack  = playerAction != Actions.ENERGY && playerAction != Actions.DEFENSE;
			bool playerIsDefense = playerAction == Actions.DEFENSE;
			bool enemyIsAttack   = enemyAction  != Actions.ENERGY && enemyAction  != Actions.DEFENSE;
			bool enemyIsDefense  = enemyAction  == Actions.DEFENSE;

			int playerEffective = 0;
			int enemyEffective  = 0;

			if (playerAction != Actions.ENERGY)
			{
				playerMult      = CombatResolver.GetMultiplier(playerAction, enemyAction, playerIsSuper, enemyIsSuper);
				int baseDmg     = CombatResolver.DamageRoll(playerAction, _player.Rock.TotalValue, _player.Paper.TotalValue, _player.Scissor.TotalValue, _player.Defense.TotalValue, _player.Level);
				playerCritBonus = CombatResolver.CritBonus(playerAction, _player.Rock.TotalValue, _player.Paper.TotalValue, _player.Scissor.TotalValue, _player.Defense.TotalValue, _player.EnergyRecovery.TotalValue, _player.Crit.TotalValue);
				playerEffective = Mathf.FloorToInt(baseDmg * playerMult) + playerCritBonus;
			}

			if (enemyAction != Actions.ENERGY)
			{
				enemyMult      = CombatResolver.GetMultiplier(enemyAction, playerAction, enemyIsSuper, playerIsSuper);
				int baseDmg    = CombatResolver.DamageRoll(enemyAction, _enemy.Rock, _enemy.Paper, _enemy.Scissor, _enemy.Defense, _enemy.Level);
				enemyCritBonus = CombatResolver.CritBonus(enemyAction, _enemy.Rock, _enemy.Paper, _enemy.Scissor, _enemy.Defense, _enemy.EnergyRecovery, _enemy.Crit);
				enemyEffective = Mathf.FloorToInt(baseDmg * enemyMult) + enemyCritBonus;
			}

			Debug.Log($"[CombatManager] Effective → Player:{playerEffective} (×{playerMult} crit:{playerCritBonus})  Enemy:{enemyEffective} (×{enemyMult} crit:{enemyCritBonus})");

			// ── Damage cases (difference-based clash) ─────────────────────────────
			if (!playerIsAttack && !enemyIsAttack)
			{
				// Case 2: both passive (Defense+Defense, Energy+Energy, Defense+Energy) → no damage
				Debug.Log($"[CombatManager] Case 2 (both passive) → no damage");
			}
			else if (playerIsAttack && enemyIsDefense)
			{
				// Case 3a: player attacks, enemy defends → attacker wins only if effective > shield
				if (playerEffective > enemyEffective)
				{
					playerDamageDealt = playerEffective - enemyEffective;
					_enemy.ReceiveDamage(playerDamageDealt);
					Debug.Log($"[CombatManager] Case 3: player breaks shield → enemy takes {playerDamageDealt}  EnemyHP:{_enemy.CurrentHealth}/{_enemy.MaxHealth}");
				}
				else
				{
					Debug.Log($"[CombatManager] Case 3: enemy shield holds ({enemyEffective} ≥ {playerEffective}) → no damage");
				}
			}
			else if (enemyIsAttack && playerIsDefense)
			{
				// Case 3b: enemy attacks, player defends → thorns always fire; attacker wins only if effective > shield
				if (_player.Thorns.TotalValue > 0)
				{
					int thorns = CombatResolver.ThornsRoll(_player.Thorns.TotalValue, enemyMult, enemyCritBonus, _player.Level);
					_enemy.ReceiveDamage(thorns);
					Debug.Log($"[CombatManager] Thorns → {thorns} reflected  EnemyHP:{_enemy.CurrentHealth}/{_enemy.MaxHealth}");
				}

				if (enemyEffective > playerEffective)
				{
					enemyDamageDealt = enemyEffective - playerEffective;
					_playerHP = Mathf.Max(0, _playerHP - enemyDamageDealt);
					Debug.Log($"[CombatManager] Case 3: enemy breaks shield → player takes {enemyDamageDealt}  PlayerHP:{_playerHP}/{_player.MaxHealth.TotalValue}");
				}
				else
				{
					Debug.Log($"[CombatManager] Case 3: player shield holds ({playerEffective} ≥ {enemyEffective}) → no damage");
				}
			}
			else
			{
				// Case 1: attack vs attack, attack vs energy, energy vs attack → symmetric difference
				int diff = playerEffective - enemyEffective;
				if (diff > 0)
				{
					playerDamageDealt = diff;
					_enemy.ReceiveDamage(playerDamageDealt);
					Debug.Log($"[CombatManager] Case 1: player wins ({playerEffective} vs {enemyEffective}) → enemy takes {playerDamageDealt}  EnemyHP:{_enemy.CurrentHealth}/{_enemy.MaxHealth}");
				}
				else if (diff < 0)
				{
					enemyDamageDealt = -diff;
					_playerHP = Mathf.Max(0, _playerHP - enemyDamageDealt);
					Debug.Log($"[CombatManager] Case 1: enemy wins ({enemyEffective} vs {playerEffective}) → player takes {enemyDamageDealt}  PlayerHP:{_playerHP}/{_player.MaxHealth.TotalValue}");
				}
				else
				{
					Debug.Log($"[CombatManager] Case 1: tie ({playerEffective} = {enemyEffective}) → no damage");
				}
			}

			_enemy.ResetCombatState();
			AccumulateTrainingExp(playerAction, playerMult, playerCritBonus, enemyDamageDealt);

			// ── Update UI ─────────────────────────────────────────────────────────
			_combatUI.RefreshPlayerBars(_playerHP, _playerEnergy);
			_combatUI.RefreshEnemyBars(_enemy.CurrentHealth, _enemy.CurrentEnergy);
			_combatUI.ShowPlayerActionBubble(GetActionIcon(playerAction), playerDamageDealt);
			_combatUI.ShowEnemyActionBubble(GetActionIcon(enemyAction), enemyDamageDealt);

			StartCoroutine(WaitThenContinue());
		}

		private IEnumerator WaitThenContinue()
		{
			yield return new WaitForSeconds(GameConsts.COMBAT_ROUND_DELAY);
			CheckCombatEnd();
		}

		private void CheckCombatEnd()
		{
			bool playerDead = _playerHP <= 0;
			bool enemyDead  = _enemy.CurrentHealth <= 0;

			if (playerDead || enemyDead || _currentRound >= GameConsts.COMBAT_MAX_ROUNDS)
			{
				bool   playerWins = !playerDead && (enemyDead || _playerHP > _enemy.CurrentHealth);
				string reason     = playerDead ? "player dead" : enemyDead ? "enemy dead" : "max rounds";
				Debug.Log($"[CombatManager] Combat END → {reason}  playerWins:{playerWins}  PlayerHP:{_playerHP}  EnemyHP:{_enemy.CurrentHealth}");
				EndCombat(playerWins);
				return;
			}

			BeginRound();
		}

		private void EndCombat(bool playerWins)
		{
			_player.CurrentHealth = _playerHP;

			int goldEarned = 0;
			if (playerWins)
			{
				goldEarned = _enemy.RewardRoll();
				_player.Gold += goldEarned;
				Debug.Log($"[CombatManager] Victory → gold earned:{goldEarned}  totalGold:{_player.Gold}");
			}
			else
			{
				Debug.Log($"[CombatManager] Defeat → no gold earned");
			}

			ApplyPendingTrainingExp();
			AppEvents.OnGameContextUpdated?.Invoke();

			_combatUI.SetActionsInteractable(false);
			_combatUI.HideCanvas();

			if (_resultUI == null)
			{
				Debug.LogError("[CombatManager] EndCombat() -> _resultUI is null. Is CombatResult_UIController a child of Combat_UIService?");
				AppEvents.OnCombatFinished?.Invoke(playerWins);
				return;
			}

			_resultUI.SetData(playerWins, goldEarned, _pendingTrainingExp);
			_resultUI.ShowCanvas();
			// OnCombatFinished lo dispara el botón Continuar de _resultUI
		}

		#endregion

		#region HELPERS

		private Sprite GetActionIcon(Actions action)
		{
			if (_enemy.CurrentLanguage == null) return null;
			return _enemy.CurrentLanguage.GetActionIcon(action);
		}

		private int PlayerActionCost(Actions action)
		{
			switch (action)
			{
				case Actions.ROCK:    return _player.RockCost;
				case Actions.PAPER:   return _player.PaperCost;
				case Actions.SCISSOR: return _player.ScissorCost;
				case Actions.DEFENSE: return _player.DefenseCost;
				default:              return 0;
			}
		}

		private TrainingHouseModifier FindActiveTrainingModifier()
		{
			foreach (StatAttribute attr in _player.Attributes)
			{
				TrainingHouseModifier mod = attr.GetModifier<TrainingHouseModifier>();
				if (mod != null && mod.IsTraining && mod.IsUnlocked)
					return mod;
			}
			return null;
		}

		private void AccumulateTrainingExp(Actions playerAction, float multiplier, int critBonus, int enemyDamageDealt)
		{
			if (_activeTrainingModifier == null) return;
			if (!ActionMatchesTrainingStat(playerAction, _activeTrainingModifier.Stat)) return;

			bool tookNoDamage = enemyDamageDealt == 0;
			if (!tookNoDamage)
			{
				Debug.Log($"[CombatManager] Training EXP → skipped (took {enemyDamageDealt} damage this round)");
				return;
			}

			int exp = 0;
			if      (multiplier >= 2f)   exp = 2;
			else if (multiplier >= 1.2f) exp = 1;

			if (critBonus > 0) exp++;

			_pendingTrainingExp += exp;
			Debug.Log($"[CombatManager] Training EXP → +{exp} this round  (mult:{multiplier} crit:{critBonus})  total pending:{_pendingTrainingExp}");
		}

		private static bool ActionMatchesTrainingStat(Actions action, Stats stat)
		{
			switch (action)
			{
				case Actions.ROCK:    return stat == Stats.ROCK;
				case Actions.PAPER:   return stat == Stats.PAPER;
				case Actions.SCISSOR: return stat == Stats.SCISSOR;
				default:              return false;
			}
		}

		private void ApplyPendingTrainingExp()
		{
			if (_activeTrainingModifier == null || _pendingTrainingExp <= 0) return;

			int levelBefore = _activeTrainingModifier.Level;
			_activeTrainingModifier.Experience += _pendingTrainingExp;
			AppEvents.OnTrainingExpUpdated?.Invoke();
			if (_activeTrainingModifier.Level > levelBefore)
			{
				Debug.Log($"[CombatManager] Training LEVEL UP! {_activeTrainingModifier.Stat} {levelBefore} → {_activeTrainingModifier.Level}");
				AppEvents.OnTrainingLevelUpdated?.Invoke();
			}
			Debug.Log($"[CombatManager] Training EXP applied → stat:{_activeTrainingModifier.Stat}  +{_pendingTrainingExp}EXP  level:{_activeTrainingModifier.Level}");
		}

		#endregion
	}
}
