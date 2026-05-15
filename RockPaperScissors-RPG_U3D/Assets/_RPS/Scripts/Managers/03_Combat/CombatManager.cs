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
		[SerializeField, ReadOnly] private CombatUIController _combatUI;

		private Player        _player;
		private CombatContext _context;
		private Enemy         _enemy;

		private int _currentRound;
		private int _playerHP;
		private int _playerEnergy;

		#region SETUP

		public override void SetUp()
		{
			_combatUI = ServiceLocator.Instance.GetService<UIService>().GetController<CombatUIController>();
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

			_currentRound = 0;
			_playerHP     = _player.MaxHealth.TotalValue;
			_playerEnergy = _player.InitialEnergy;

			EnemyData data = _context.CurrentStep.Enemy.Data;
			_enemy = new Enemy(data);
			_enemy.LanguageRoll();

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
			_combatUI.SetActionsInteractable(true);
		}

		private void OnPlayerActionSelected(Actions playerAction)
		{
			_combatUI.SetActionsInteractable(false);
			ResolveRound(playerAction, _enemy.CurrentAction);
		}

		private void ResolveRound(Actions playerAction, Actions enemyAction)
		{
			int playerDamageDealt = 0;
			int enemyDamageDealt  = 0;

			// ── Super check BEFORE energy changes ────────────────────────────────
			bool playerIsSuper = _playerEnergy >= GameConsts.COMBAT_MAX_ENERGY && playerAction != Actions.ENERGY;
			bool enemyIsSuper  = _enemy.CurrentEnergy >= _enemy.MaxEnergy && enemyAction != Actions.ENERGY;

			// ── Player energy ─────────────────────────────────────────────────────
			if (playerAction == Actions.ENERGY)
			{
				_playerEnergy = Mathf.Min(GameConsts.COMBAT_MAX_ENERGY, _playerEnergy + _player.EnergyRecovery.TotalValue);
			}
			else
			{
				_playerEnergy = Mathf.Max(0, _playerEnergy - PlayerActionCost(playerAction));
			}

			// ── Enemy energy ──────────────────────────────────────────────────────
			if (enemyAction == Actions.ENERGY)
				_enemy.RecoverEnergy();
			else
				_enemy.PayActionEnergyCost();

			// ── Player attacks enemy ──────────────────────────────────────────────
			if (playerAction != Actions.ENERGY && playerAction != Actions.DEFENSE)
			{
				float multiplier = CombatResolver.GetMultiplier(playerAction, enemyAction, playerIsSuper, enemyIsSuper);
				int   baseDmg    = CombatResolver.DamageRoll(
					playerAction,
					_player.Rock.TotalValue,
					_player.Paper.TotalValue,
					_player.Scissor.TotalValue,
					_player.Defense.TotalValue,
					_player.Level);
				int critBonus = CombatResolver.CritBonus(
					playerAction,
					_player.Rock.TotalValue,
					_player.Paper.TotalValue,
					_player.Scissor.TotalValue,
					_player.Defense.TotalValue,
					_player.EnergyRecovery.TotalValue,
					_player.Crit.TotalValue);

				playerDamageDealt = Mathf.FloorToInt(baseDmg * multiplier) + critBonus;
				_enemy.ReceiveDamage(playerDamageDealt);
			}

			// ── Enemy attacks player ──────────────────────────────────────────────
			if (enemyAction != Actions.ENERGY && enemyAction != Actions.DEFENSE)
			{
				float multiplier = CombatResolver.GetMultiplier(enemyAction, playerAction, enemyIsSuper, playerIsSuper);
				int   baseDmg    = CombatResolver.DamageRoll(
					enemyAction,
					_enemy.Rock,
					_enemy.Paper,
					_enemy.Scissor,
					_enemy.Defense,
					_enemy.Level);
				int critBonus = CombatResolver.CritBonus(
					enemyAction,
					_enemy.Rock,
					_enemy.Paper,
					_enemy.Scissor,
					_enemy.Defense,
					_enemy.EnergyRecovery,
					_enemy.Crit);

				enemyDamageDealt = Mathf.FloorToInt(baseDmg * multiplier) + critBonus;

				if (playerAction == Actions.DEFENSE && _player.Thorns.TotalValue > 0)
				{
					int thorns = CombatResolver.ThornsRoll(_player.Thorns.TotalValue, multiplier, critBonus, _player.Level);
					_enemy.ReceiveDamage(thorns);
				}

				_playerHP = Mathf.Max(0, _playerHP - enemyDamageDealt);
			}

			_enemy.ResetCombatState();

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
				bool playerWins = !playerDead && (enemyDead || _playerHP > _enemy.CurrentHealth);
				EndCombat(playerWins);
				return;
			}

			BeginRound();
		}

		private void EndCombat(bool playerWins)
		{
			if (playerWins)
			{
				int gold = _enemy.RewardRoll();
				_player.Gold += gold;
			}

			AppEvents.OnCombatFinished?.Invoke(playerWins);
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

		#endregion
	}
}
