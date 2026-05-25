using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS
{
	/// <summary>
	/// Manager de la Herrería de Piedra. Lee la config de items del ScriptableObject e inyecta
	/// el nivel del jugador para saber el estado actual de cada consumible.
	/// Coste de mejora: (nivelActual + 1) × GameConsts.STONE_SMITHY_COST_PER_LEVEL.
	/// Los consumibles se usan en combate usando el mismo patrón: SO como referencia + nivel del contexto (Phase 5).
	/// </summary>
	public class StoneSmithyManager : BaseManager, ITownBuilding
	{
		private StoneSmithyScrObj _stoneSmithyScrObj;

		[Header("DEBUG")]
		[SerializeField, ReadOnly] private StoneSmithyUIController _stoneSmithyUIController;
		[SerializeField, ReadOnly] private Player _player;

		private Item _attackItem;
		private Item _healItem;
		private Item _energyItem;

		#region SETUP

		public override void SetUp()
		{
			Debug.Log($"[StoneSmithyManager] SetUp() -> ");
			_stoneSmithyScrObj = ServiceLocator.Instance.GetService<StaticDataService>().StoneSmithyItems;
			_stoneSmithyUIController = ServiceLocator.Instance.GetService<UIService>().GetController<StoneSmithyUIController>();
			_player = AppContext.Player;
		}

		protected override void Subscribe()
		{
			Debug.Log($"[StoneSmithyManager] Subscribe() -> ");
			AppEvents.OnUpgradeAttack += UpgradeAttack;
			AppEvents.OnUpgradeHeal   += UpgradeHeal;
			AppEvents.OnUpgradeEnergy += UpgradeEnergy;
		}

		protected override void UnSubscribe()
		{
			Debug.Log($"[StoneSmithyManager] UnSubscribe() -> ");
			AppEvents.OnUpgradeAttack -= UpgradeAttack;
			AppEvents.OnUpgradeHeal   -= UpgradeHeal;
			AppEvents.OnUpgradeEnergy -= UpgradeEnergy;
		}

		#endregion

		#region CONTROL

		public void OnMenuOpen()
		{
			Debug.Log($"[StoneSmithyManager] OnMenuOpen() -> ");

			_attackItem = _stoneSmithyScrObj.Data.attackItem.Clone();
			_healItem   = _stoneSmithyScrObj.Data.healItem.Clone();
			_energyItem = _stoneSmithyScrObj.Data.energyItem.Clone();

			_attackItem.level = _player.AttackItemLevel;
			_healItem.level   = _player.HealItemLevel;
			_energyItem.level = _player.EnergyItemLevel;

			_stoneSmithyUIController.SetData(_attackItem, _healItem, _energyItem);
		}

		private void UpgradeAttack()
		{
			Debug.Log($"[StoneSmithyManager] UpgradeAttack() -> ");
			int maxLevel = _stoneSmithyScrObj.Data.attackItem.amountsPerLevel.Count;
			if (_player.AttackItemLevel >= maxLevel) { Debug.Log("[StoneSmithyManager] Attack item already at max level."); return; }
			if (!TrySpendGold(GetUpgradeCost(_player.AttackItemLevel))) return;
			_player.AttackItemLevel++;
			_attackItem.level = _player.AttackItemLevel;
			_stoneSmithyUIController.RefreshAttackLevel(_attackItem.level);
			AppEvents.OnGameContextUpdated?.Invoke();
		}

		private void UpgradeHeal()
		{
			Debug.Log($"[StoneSmithyManager] UpgradeHeal() -> ");
			int maxLevel = _stoneSmithyScrObj.Data.healItem.amountsPerLevel.Count;
			if (_player.HealItemLevel >= maxLevel) { Debug.Log("[StoneSmithyManager] Heal item already at max level."); return; }
			if (!TrySpendGold(GetUpgradeCost(_player.HealItemLevel))) return;
			_player.HealItemLevel++;
			_healItem.level = _player.HealItemLevel;
			_stoneSmithyUIController.RefreshHealLevel(_healItem.level);
			AppEvents.OnGameContextUpdated?.Invoke();
		}

		private void UpgradeEnergy()
		{
			Debug.Log($"[StoneSmithyManager] UpgradeEnergy() -> ");
			int maxLevel = _stoneSmithyScrObj.Data.energyItem.amountsPerLevel.Count;
			if (_player.EnergyItemLevel >= maxLevel) { Debug.Log("[StoneSmithyManager] Energy item already at max level."); return; }
			if (!TrySpendGold(GetUpgradeCost(_player.EnergyItemLevel))) return;
			_player.EnergyItemLevel++;
			_energyItem.level = _player.EnergyItemLevel;
			_stoneSmithyUIController.RefreshEnergyLevel(_energyItem.level);
			AppEvents.OnGameContextUpdated?.Invoke();
		}

		private bool TrySpendGold(int cost)
		{
			if (_player.Gold < cost)
			{
				Debug.Log($"[StoneSmithyManager] TrySpendGold() -> Not enough gold. Required: {cost}, Have: {_player.Gold}");
				return false;
			}
			_player.Gold -= cost;
			return true;
		}

		private int GetUpgradeCost(int currentLevel)
		{
			return (currentLevel + 1) * GameConsts.STONE_SMITHY_COST_PER_LEVEL;
		}

		#endregion

		#region DEBUG

		[FoldoutGroup("DEBUG"), Button("Force Upgrade Attack")]
		private void Debug_UpgradeAttack()
		{
			int maxLevel = _stoneSmithyScrObj.Data.attackItem.amountsPerLevel.Count;
			if (_player.AttackItemLevel >= maxLevel) { Debug.Log("[StoneSmithyManager] Attack already at max."); return; }
			_player.AttackItemLevel++;
			_attackItem.level = _player.AttackItemLevel;
			_stoneSmithyUIController.RefreshAttackLevel(_attackItem.level);
			AppEvents.OnGameContextUpdated?.Invoke();
		}

		[FoldoutGroup("DEBUG"), Button("Force Upgrade Heal")]
		private void Debug_UpgradeHeal()
		{
			int maxLevel = _stoneSmithyScrObj.Data.healItem.amountsPerLevel.Count;
			if (_player.HealItemLevel >= maxLevel) { Debug.Log("[StoneSmithyManager] Heal already at max."); return; }
			_player.HealItemLevel++;
			_healItem.level = _player.HealItemLevel;
			_stoneSmithyUIController.RefreshHealLevel(_healItem.level);
			AppEvents.OnGameContextUpdated?.Invoke();
		}

		[FoldoutGroup("DEBUG"), Button("Force Upgrade Energy")]
		private void Debug_UpgradeEnergy()
		{
			int maxLevel = _stoneSmithyScrObj.Data.energyItem.amountsPerLevel.Count;
			if (_player.EnergyItemLevel >= maxLevel) { Debug.Log("[StoneSmithyManager] Energy already at max."); return; }
			_player.EnergyItemLevel++;
			_energyItem.level = _player.EnergyItemLevel;
			_stoneSmithyUIController.RefreshEnergyLevel(_energyItem.level);
			AppEvents.OnGameContextUpdated?.Invoke();
		}

		[FoldoutGroup("DEBUG"), Button("Reset All Items")]
		private void Debug_ResetAllItems()
		{
			_player.AttackItemLevel = 0;
			_player.HealItemLevel   = 0;
			_player.EnergyItemLevel = 0;
			_attackItem.level = 0;
			_healItem.level   = 0;
			_energyItem.level = 0;
			_stoneSmithyUIController.RefreshAttackLevel(0);
			_stoneSmithyUIController.RefreshHealLevel(0);
			_stoneSmithyUIController.RefreshEnergyLevel(0);
			AppEvents.OnGameContextUpdated?.Invoke();
			Debug.Log("[StoneSmithyManager] All item levels reset to 0.");
		}

		#endregion
	}
}
