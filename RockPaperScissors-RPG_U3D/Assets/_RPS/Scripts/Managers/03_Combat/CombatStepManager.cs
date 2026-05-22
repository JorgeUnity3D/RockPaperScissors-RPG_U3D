using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS
{
	/// <summary>
	/// Orquesta el loop de combate. Recibe la acción del jugador vía AppEvents,
	/// resuelve el daño con CombatResolver y actualiza PlayerHUDUIController y EnemyHUDUIController.
	/// </summary>
	public class CombatStepManager : BaseManager
	{
		[SerializeField, ReadOnly] private PlayerHUDUIController _playerHUD;
		[SerializeField, ReadOnly] private EnemyHUDUIController _enemyHUD;
		[SerializeField]           private LanguageScrObj       _commonLanguage;
		[SerializeField]           private StoneSmithyScrObj         _stoneSmithyScrObj;

		private Player        _player;
		private CombatContext _context;
		private Enemy         _enemy;

		private int _currentRound;
		private int _playerHP;
		private int _playerEnergy;

		private bool _attackItemUsed;
		private bool _healItemUsed;
		private bool _energyItemUsed;
		private bool _playerSuperKill;

		private int                   _pendingTrainingExp;
		private TrainingHouseModifier _activeTrainingModifier;

		#region SETUP

		public override void SetUp()
		{
			UIService uiService = ServiceLocator.Instance.GetService<UIService>();
			_playerHUD = uiService.GetController<PlayerHUDUIController>();
			_enemyHUD  = uiService.GetController<EnemyHUDUIController>();
			_player    = AppContext.Player;
			_context   = AppContext.CombatContext;

			_playerHUD.SetCommonLanguage(_commonLanguage != null ? _commonLanguage.Data : null);
		}

		protected override void Subscribe()
		{
			AppEvents.OnCombatActionSelected += OnPlayerActionSelected;
			AppEvents.OnBackpackItemUsed     += OnBackpackItemUsed;
		}

		protected override void UnSubscribe()
		{
			AppEvents.OnCombatActionSelected -= OnPlayerActionSelected;
			AppEvents.OnBackpackItemUsed     -= OnBackpackItemUsed;
		}

		#endregion

		#region CONTROL

		public override void Initialize()
		{
			if (_context == null)
			{
				Debug.LogError("[CombatStepManager] Initialize() -> CombatContext is null. Was TravelManager bypassed?");
				return;
			}

			_currentRound           = 0;
			_playerHP               = _player.CurrentHealth;
			_playerEnergy           = _player.CurrentEnergy;
			_attackItemUsed         = false;
			_healItemUsed           = false;
			_energyItemUsed         = false;
			_playerSuperKill        = false;
			_pendingTrainingExp     = 0;
			_activeTrainingModifier = FindActiveTrainingModifier();

			_playerHUD.SetBackpackData(_player.AttackItemLevel, _player.HealItemLevel, _player.EnergyItemLevel);

			EnemyScrObj enemyScrObj = _context.CurrentStep.Enemy;
			if (enemyScrObj == null)
			{
				Debug.LogError("[CombatStepManager] CurrentStep.Enemy is null — cannot start combat. Check MapLevels SO.");
				AppEvents.OnStepFinished?.Invoke(false);
				return;
			}
			EnemyData data = enemyScrObj.Data;
			_enemy = new Enemy(data);
			_enemy.LanguageRoll();

			Debug.Log($"[CombatStepManager] ═══════════════ COMBAT START ═══════════════");
			Debug.Log($"[CombatStepManager] Player → HP:{_playerHP}  Energy:{_playerEnergy}  Lv:{_player.Level}  Mentality:{_player.Mentality.TotalValue}");
			Debug.Log($"[CombatStepManager] Enemy  → {_enemy.Name}  HP:{_enemy.MaxHealth}  Energy:{GameConsts.COMBAT_MAX_ENERGY}  Lv:{_enemy.Level}  Mentality:{_enemy.StoredMentality}");
			Debug.Log($"[CombatStepManager] Language → {(_enemy.CurrentLanguage != null ? _enemy.CurrentLanguage.GetType().Name : "none")}");
			Debug.Log($"[CombatStepManager] Training → {(_activeTrainingModifier != null ? _activeTrainingModifier.Stat.ToString() : "none")}");

			_enemyHUD.SetData(_enemy.Portrait, _enemy.MaxHealth, GameConsts.COMBAT_MAX_ENERGY);
			_enemyHUD.ShowCanvas();

			BeginRound();
		}

		#endregion

		#region ROUND LOOP

		private void BeginRound()
		{
			_currentRound++;
			_enemy.ActionRoll();
			_playerHUD.HidePlayerBubbles();
			_enemyHUD.HideBubbles();

			Debug.Log($"[CombatStepManager] ─── Round {_currentRound} BEGIN ─── PlayerHP:{_playerHP}/{_player.MaxHealth.TotalValue}  Energy:{_playerEnergy}/{GameConsts.COMBAT_MAX_ENERGY}  │  EnemyHP:{_enemy.CurrentHealth}/{_enemy.MaxHealth}  Energy:{_enemy.CurrentEnergy}/{GameConsts.COMBAT_MAX_ENERGY}");
			Debug.Log($"[CombatStepManager] Enemy rolled → CurrentAction:{ColorAction(_enemy.CurrentAction)}  ThinkingAction:{ColorAction(_enemy.ThinkingAction)}  StoredMentality:{_enemy.StoredMentality}");

			int mentalityResult = _enemy.MentalityRollAgainst(_player.Mentality.TotalValue);
			if (mentalityResult <= 0)
			{
				Debug.Log($"[CombatStepManager] Mentality roll {mentalityResult} ≤ 0 → player reads enemy mind → {ColorAction(_enemy.ThinkingAction)}");
				_enemyHUD.ShowEnemyThoughtBubble(GetActionIconEnemy(_enemy.ThinkingAction));
				_enemy.ResetMentality();
			}
			else
			{
				Debug.Log($"[CombatStepManager] Mentality roll {mentalityResult} > 0 → enemy conceals thought");
				_enemyHUD.ShowEnemyThoughtBubble(GetActionIconCommon(Actions.NONE));
				_enemy.IncreaseMentality();
			}

			_playerHUD.SetCombatActionsInteractable(true);
		}

		private void OnPlayerActionSelected(Actions playerAction)
		{
			Debug.Log($"[CombatStepManager] Player selected: {ColorAction(playerAction)}");
			_playerHUD.SetCombatActionsInteractable(false);
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
			bool enemyIsSuper  = _enemy.CurrentEnergy >= GameConsts.COMBAT_MAX_ENERGY && enemyAction != Actions.ENERGY;

			Debug.Log($"[CombatStepManager] RESOLVE  Player:{ColorAction(playerAction)}{(playerIsSuper ? "<color=#FFFFFF>(SUPER)</color>" : "")}  vs  Enemy:{ColorAction(enemyAction)}{(enemyIsSuper ? "<color=#FFFFFF>(SUPER)</color>" : "")}");

			// ── Energy management ─────────────────────────────────────────────────
			if (playerAction == Actions.ENERGY)
				_playerEnergy = Mathf.Min(GameConsts.COMBAT_MAX_ENERGY, _playerEnergy + _player.EnergyRecovery.TotalValue);
			else
				_playerEnergy = Mathf.Max(0, _playerEnergy - PlayerActionCost(playerAction));

			if (enemyAction == Actions.ENERGY)
				_enemy.RecoverEnergy();
			else
				_enemy.PayActionEnergyCost();

			Debug.Log($"[CombatStepManager] Energy after → Player:{_playerEnergy}/{GameConsts.COMBAT_MAX_ENERGY}  Enemy:{_enemy.CurrentEnergy}/{GameConsts.COMBAT_MAX_ENERGY}");

			// ── Effective power ───────────────────────────────────────────────────
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

			Debug.Log($"[CombatStepManager] Effective → Player:{playerEffective} (×{playerMult} crit:{playerCritBonus})  Enemy:{enemyEffective} (×{enemyMult} crit:{enemyCritBonus})");

			// ── Damage cases (difference-based clash) ─────────────────────────────
			if (!playerIsAttack && !enemyIsAttack)
			{
				Debug.Log($"[CombatStepManager] Case 2 (both passive) → no damage");
			}
			else if (playerIsAttack && enemyIsDefense)
			{
				if (playerEffective > enemyEffective)
				{
					playerDamageDealt = playerEffective - enemyEffective;
					_enemy.ReceiveDamage(playerDamageDealt);
					if (playerIsSuper && _enemy.CurrentHealth <= 0) _playerSuperKill = true;
					Debug.Log($"[CombatStepManager] Case 3: player breaks shield → enemy takes {playerDamageDealt}  EnemyHP:{_enemy.CurrentHealth}/{_enemy.MaxHealth}");
				}
				else
				{
					Debug.Log($"[CombatStepManager] Case 3: enemy shield holds ({enemyEffective} ≥ {playerEffective}) → no damage");
				}
			}
			else if (enemyIsAttack && playerIsDefense)
			{
				if (_player.Thorns.TotalValue > 0)
				{
					int thorns = CombatResolver.ThornsRoll(_player.Thorns.TotalValue, enemyMult, enemyCritBonus, _player.Level);
					_enemy.ReceiveDamage(thorns);
					Debug.Log($"[CombatStepManager] Thorns → {thorns} reflected  EnemyHP:{_enemy.CurrentHealth}/{_enemy.MaxHealth}");
				}

				if (enemyEffective > playerEffective)
				{
					enemyDamageDealt = enemyEffective - playerEffective;
					_playerHP = Mathf.Max(0, _playerHP - enemyDamageDealt);
					Debug.Log($"[CombatStepManager] Case 3: enemy breaks shield → player takes {enemyDamageDealt}  PlayerHP:{_playerHP}/{_player.MaxHealth.TotalValue}");
				}
				else
				{
					Debug.Log($"[CombatStepManager] Case 3: player shield holds ({playerEffective} ≥ {enemyEffective}) → no damage");
				}
			}
			else
			{
				int diff = playerEffective - enemyEffective;
				if (diff > 0)
				{
					playerDamageDealt = diff;
					_enemy.ReceiveDamage(playerDamageDealt);
					if (playerIsSuper && _enemy.CurrentHealth <= 0) _playerSuperKill = true;
					Debug.Log($"[CombatStepManager] Case 1: player wins ({playerEffective} vs {enemyEffective}) → enemy takes {playerDamageDealt}  EnemyHP:{_enemy.CurrentHealth}/{_enemy.MaxHealth}");
				}
				else if (diff < 0)
				{
					enemyDamageDealt = -diff;
					_playerHP = Mathf.Max(0, _playerHP - enemyDamageDealt);
					Debug.Log($"[CombatStepManager] Case 1: enemy wins ({enemyEffective} vs {playerEffective}) → player takes {enemyDamageDealt}  PlayerHP:{_playerHP}/{_player.MaxHealth.TotalValue}");
				}
				else
				{
					Debug.Log($"[CombatStepManager] Case 1: tie ({playerEffective} = {enemyEffective}) → no damage");
				}
			}

			_enemy.ResetCombatState();
			AccumulateTrainingExp(playerAction, playerMult, playerCritBonus, enemyDamageDealt);

			// ── Update UI ─────────────────────────────────────────────────────────
			_playerHUD.RefreshBars(_playerHP, _playerEnergy);
			_enemyHUD.RefreshBars(_enemy.CurrentHealth, _enemy.CurrentEnergy);
			_playerHUD.ShowPlayerActionBubble(GetActionIconCommon(playerAction), playerDamageDealt);
			_enemyHUD.ShowEnemyActionBubble(GetActionIconCommon(enemyAction), enemyDamageDealt);

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
				Debug.Log($"[CombatStepManager] Combat END → {reason}  playerWins:{playerWins}  PlayerHP:{_playerHP}  EnemyHP:{_enemy.CurrentHealth}");
				EndCombat(playerWins);
				return;
			}

			BeginRound();
		}

		private void EndCombat(bool playerWins)
		{
			_player.CurrentHealth = _playerHP;
			_player.CurrentEnergy = _playerEnergy;

			if (playerWins)
			{
				int goldEarned = _enemy.RewardRoll();
				if (_playerSuperKill) goldEarned *= 2;
				_player.Gold += goldEarned;
				AppContext.CombatContext?.AddGold(goldEarned);
				Debug.Log($"[CombatStepManager] Victory → gold:{goldEarned}{(_playerSuperKill ? " (x2 super kill)" : "")}  total:{_player.Gold}");
			}
			else
			{
				Debug.Log($"[CombatStepManager] Defeat → no gold earned");
			}

			ApplyPendingTrainingExp();
			AppContext.CombatContext?.AddTrainingExp(_pendingTrainingExp);
			AppEvents.OnGameContextUpdated?.Invoke();

			_playerHUD.SetCombatActionsInteractable(false);
			_playerHUD.HidePlayerBubbles();
			_playerHUD.HideCanvas();
			_enemyHUD.HideBubbles();
			_enemyHUD.HideCanvas();

			AppEvents.OnStepFinished?.Invoke(playerWins);
		}

		#endregion

		#region BACKPACK

		private void OnBackpackItemUsed(ItemType type)
		{
			if (_stoneSmithyScrObj == null)
			{
				Debug.LogWarning("[CombatStepManager] StoneSmithyScrObj not assigned — item effect skipped.");
				return;
			}

			StoneSmithyData data = _stoneSmithyScrObj.Data;

			switch (type)
			{
				case ItemType.SHURIKEN:
					if (_attackItemUsed) return;
					_attackItemUsed = true;
					int dmg = data.attackItem.amountsPerLevel[_player.AttackItemLevel - 1];
					_enemy.ReceiveDamage(dmg);
					Debug.Log($"[CombatStepManager] Shuriken → {dmg} damage  EnemyHP:{_enemy.CurrentHealth}/{_enemy.MaxHealth}");
					break;

				case ItemType.HEALTH_POTION:
					if (_healItemUsed) return;
					_healItemUsed = true;
					int heal = data.healItem.amountsPerLevel[_player.HealItemLevel - 1];
					_playerHP = Mathf.Min(_player.MaxHealth.TotalValue, _playerHP + heal);
					Debug.Log($"[CombatStepManager] Potion → +{heal} HP  PlayerHP:{_playerHP}/{_player.MaxHealth.TotalValue}");
					break;

				case ItemType.TORCH:
					if (_energyItemUsed) return;
					_energyItemUsed = true;
					int energy = data.energyItem.amountsPerLevel[_player.EnergyItemLevel - 1];
					_playerEnergy = Mathf.Min(GameConsts.COMBAT_MAX_ENERGY, _playerEnergy + energy);
					Debug.Log($"[CombatStepManager] Torch → +{energy} energy  PlayerEnergy:{_playerEnergy}/{GameConsts.COMBAT_MAX_ENERGY}");
					break;

				default:
					return;
			}

			_playerHUD.SetItemUsed(type);
			_playerHUD.RefreshBars(_playerHP, _playerEnergy);
			_enemyHUD.RefreshBars(_enemy.CurrentHealth, _enemy.CurrentEnergy);
		}

		#endregion

		#region HELPERS

		private Sprite GetActionIconCommon(Actions action)
		{
			if (_commonLanguage == null)
			{
				Debug.LogWarning("[CombatStepManager] _commonLanguage is not assigned in Inspector.");
				return null;
			}
			return _commonLanguage.Data.GetActionIcon(action);
		}

		private Sprite GetActionIconEnemy(Actions action)
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
				Debug.Log($"[CombatStepManager] Training EXP → skipped (took {enemyDamageDealt} damage this round)");
				return;
			}

			int exp = 0;
			if      (multiplier >= 2f)   exp = 2;
			else if (multiplier >= 1.2f) exp = 1;

			if (critBonus > 0) exp++;

			_pendingTrainingExp += exp;
			Debug.Log($"[CombatStepManager] Training EXP → +{exp} this round  (mult:{multiplier} crit:{critBonus})  total pending:{_pendingTrainingExp}");
		}

		private static string ColorAction(Actions action)
		{
			switch (action)
			{
				case Actions.ROCK:    return "<color=#FF6666>ROCK</color>";
				case Actions.PAPER:   return "<color=#66AAFF>PAPER</color>";
				case Actions.SCISSOR: return "<color=#66FF88>SCISSOR</color>";
				case Actions.DEFENSE: return "<color=#FFDD44>DEFENSE</color>";
				case Actions.ENERGY:  return "<color=#44DDFF>ENERGY</color>";
				default:              return $"<color=#AAAAAA>{action}</color>";
			}
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
				Debug.Log($"[CombatStepManager] Training LEVEL UP! {_activeTrainingModifier.Stat} {levelBefore} → {_activeTrainingModifier.Level}");
				AppEvents.OnTrainingLevelUpdated?.Invoke();
			}
			Debug.Log($"[CombatStepManager] Training EXP applied → stat:{_activeTrainingModifier.Stat}  +{_pendingTrainingExp}EXP  level:{_activeTrainingModifier.Level}");
		}

		#endregion
	}
}
