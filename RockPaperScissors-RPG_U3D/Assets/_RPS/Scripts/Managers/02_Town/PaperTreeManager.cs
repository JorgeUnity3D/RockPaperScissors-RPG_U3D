using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS
{
	/// <summary>
	/// Manager del árbol de habilidades Paper Tree. Alimenta la vista con los datos del jugador y el ScriptableObject del árbol.
	/// </summary>
	public class PaperTreeManager : BaseManager, ITownBuilding
	{
		[Header("UI")]
		[SerializeField] private PaperTreeScrObj _paperTreeScrObj;
		[Header("DEBUG")]
		[SerializeField, ReadOnly] private Player _player;
		[SerializeField, ReadOnly] private TownData _paperTreeData;
		[SerializeField, ReadOnly] private PaperTreeUIController _paperTreeUIController;
		
		#region SETUP
		
		public override void SetUp()
		{
			Debug.Log($"[PaperTreeManager] SetUp() -> ");
			_paperTreeUIController = ServiceLocator.Instance.GetService<UIService>().GetController<PaperTreeUIController>();
			_player = AppContext.Player;
			_paperTreeData = AppContext.TownData.Find(t => t.TownMenu == TownMenu.PAPER_TREE);
		}

		protected override void Subscribe()
		{
			AppEvents.OnPaperTreeNodeSelected += OnPaperTreeNodeSelected;
		}

		protected override void UnSubscribe()
		{
			AppEvents.OnPaperTreeNodeSelected -= OnPaperTreeNodeSelected;
		}

        #endregion

        #region CONTROL

		public void OnMenuOpen()
		{
			Debug.Log($"[PaperTreeManager] OnMenuOpen() -> ");
			RestoreNodeState();
			_paperTreeUIController.SetData(_player.Attributes, _paperTreeScrObj, AppContext.Player.Gold);
		}

		private static readonly Stats[] PaperTreeStats =
		{
			Stats.ROCK, Stats.PAPER, Stats.SCISSOR, Stats.DEFENSE, Stats.ENERGY_RECOVERY
		};

		private void OnPaperTreeNodeSelected(PaperTreeNode node)
		{
			if (node.IsUnlocked || !node.CanUnlock || _player.Gold < node.Cost) return;

			StatAttribute attribute = _player.Attributes.Find(a => a.Stat == node.Stats);
			if (attribute == null) return;
			PaperTreeModifier modifier = attribute.GetModifier<PaperTreeModifier>();
			if (modifier == null) return;

			_player.Gold = Mathf.Max(0, _player.Gold - node.Cost);
			modifier.UnlockedNodes.Add(node.NodeID);
			modifier.Modifier += node.Modifier;

			AddPaperTreeExp(1);
			RestoreNodeState();
			_paperTreeUIController.SetData(_player.Attributes, _paperTreeScrObj, _player.Gold);
			AppEvents.OnGameContextUpdated?.Invoke();
		}

		private void AddPaperTreeExp(int amount)
		{
			_paperTreeData.Experience += amount;
			int maxLevel = GameConsts.TRAINING_EXP_PER_LEVEL.Count - 1;
			while (_paperTreeData.Level < maxLevel &&
			       _paperTreeData.Experience >= GameConsts.TRAINING_EXP_PER_LEVEL[_paperTreeData.Level])
			{
				_paperTreeData.Level++;
			}
			if (_paperTreeData.Level >= maxLevel)
				_paperTreeData.Experience = Mathf.Min(_paperTreeData.Experience, GameConsts.TRAINING_EXP_PER_LEVEL[maxLevel]);
			AppEvents.OnBuildingExpUpdated?.Invoke(TownMenu.PAPER_TREE);
		}

		private void RestoreNodeState()
		{
			foreach (Stats stat in PaperTreeStats)
			{
				List<PaperTreeNode> nodes = _paperTreeScrObj[stat];
				if (nodes == null) continue;
				StatAttribute attribute = _player.Attributes.Find(a => a.Stat == stat);
				if (attribute == null) continue;
				PaperTreeModifier modifier = attribute.GetModifier<PaperTreeModifier>();
				if (modifier == null) continue;
				foreach (PaperTreeNode node in nodes)
					node.IsUnlocked = modifier.UnlockedNodes.Contains(node.NodeID);
			}
		}
		
		#endregion
	}
}