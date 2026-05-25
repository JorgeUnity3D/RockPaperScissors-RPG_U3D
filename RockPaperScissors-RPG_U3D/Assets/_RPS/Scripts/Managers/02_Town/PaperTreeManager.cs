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
		private PaperTreeScrObj  _paperTreeScrObj;
		private IconsDictionary  _icons;

		[Header("DEBUG")]
		[SerializeField, ReadOnly] private Player _player;
		[SerializeField, ReadOnly] private TownData _paperTreeData;
		[SerializeField, ReadOnly] private PaperTreeUIController _paperTreeUIController;
		
		#region SETUP
		
		public override void SetUp()
		{
			Debug.Log($"[PaperTreeManager] SetUp() -> ");
			StaticDataService staticData = ServiceLocator.Instance.GetService<StaticDataService>();
			_paperTreeScrObj = staticData.PaperTreeSkillTrees;
			_icons           = staticData.StatIcons.Data;
			_paperTreeUIController = ServiceLocator.Instance.GetService<UIService>().GetController<PaperTreeUIController>();
			_player = AppContext.Player;
			_paperTreeData = AppContext.TownData.Find(t => t.TownMenu == TownMenu.PAPER_TREE);
			if (_paperTreeData == null)
				Debug.Log("[PaperTreeManager] SetUp() -> TownData entry for PAPER_TREE not found.");
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
			_paperTreeUIController.SetData(_player.Attributes, _paperTreeScrObj, _icons, AppContext.Player.Gold);
		}

		private static readonly Stats[] PaperTreeStats =
		{
			Stats.ROCK, Stats.PAPER, Stats.SCISSOR, Stats.DEFENSE, Stats.ENERGY_RECOVERY
		};

		private static Stats GetTreeStat(SkillNode nodeID)
		{
			return PaperTreeStats[(int)nodeID / 100 - 1];
		}

		private void OnPaperTreeNodeSelected(PaperTreeNode node)
		{
			StatAttribute attribute = _player.Attributes.Find(a => a.Stat == GetTreeStat(node.NodeID));
			if (attribute == null) return;
			PaperTreeModifier modifier = attribute.GetModifier<PaperTreeModifier>();
			if (modifier == null) return;

			if (modifier.UnlockedNodes.Contains(node.NodeID)) return;
			if (!node.CanUnlock(modifier.UnlockedNodes)) return;
			if (_player.Gold < node.Cost) return;

			_player.Gold = Mathf.Max(0, _player.Gold - node.Cost);
			modifier.UnlockedNodes.Add(node.NodeID);
			modifier.Modifier += node.Modifier;

			AddPaperTreeExp(1);
			_paperTreeUIController.SetData(_player.Attributes, _paperTreeScrObj, _icons, _player.Gold);
			AppEvents.OnGameContextUpdated?.Invoke();
		}

		private void AddPaperTreeExp(int amount)
		{
			if (_paperTreeData == null) return;
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

		#endregion

		#region DEBUG

		[FoldoutGroup("DEBUG")]
		[ValueDropdown("GetDebugNodeOptions")]
		[SerializeField] private int _debugNodeIndex;

		private IEnumerable<ValueDropdownItem> GetDebugNodeOptions()
		{
			if (_paperTreeScrObj == null) { yield return new ValueDropdownItem("(no asset)", 0); yield break; }
			int globalIndex = 0;
			foreach (Stats stat in PaperTreeStats)
			{
				List<PaperTreeNode> tree = _paperTreeScrObj[stat];
				if (tree == null) continue;
				PaperTreeModifier mod = _player?.Attributes.Find(a => a.Stat == stat)?.GetModifier<PaperTreeModifier>();
				foreach (PaperTreeNode node in tree)
				{
					bool unlocked = mod != null && mod.UnlockedNodes.Contains(node.NodeID);
					string label = $"[{stat}] {node.NodeID}  +{node.Modifier}{(unlocked ? "  ✓" : "")}";
					yield return new ValueDropdownItem(label, globalIndex);
					globalIndex++;
				}
			}
		}

		private bool GetNodeAtGlobalIndex(int globalIndex, out PaperTreeNode node, out PaperTreeModifier modifier)
		{
			node     = null;
			modifier = null;
			int i = 0;
			foreach (Stats stat in PaperTreeStats)
			{
				List<PaperTreeNode> tree = _paperTreeScrObj[stat];
				if (tree == null) continue;
				foreach (PaperTreeNode n in tree)
				{
					if (i == globalIndex)
					{
						node     = n;
						modifier = _player.Attributes.Find(a => a.Stat == GetTreeStat(n.NodeID))?.GetModifier<PaperTreeModifier>();
						return true;
					}
					i++;
				}
			}
			return false;
		}

		[FoldoutGroup("DEBUG"), Button("Unlock Node")]
		private void Debug_UnlockNode()
		{
			if (!GetNodeAtGlobalIndex(_debugNodeIndex, out PaperTreeNode node, out PaperTreeModifier modifier)) return;
			if (modifier == null) return;
			if (modifier.UnlockedNodes.Contains(node.NodeID)) { Debug.Log("[PaperTreeManager] Node already unlocked."); return; }
			modifier.UnlockedNodes.Add(node.NodeID);
			modifier.Modifier += node.Modifier;
			_paperTreeUIController.SetData(_player.Attributes, _paperTreeScrObj, _icons, _player.Gold);
			AppEvents.OnGameContextUpdated?.Invoke();
			Debug.Log($"[PaperTreeManager] Unlocked node {node.NodeID}.");
		}

		[FoldoutGroup("DEBUG"), Button("Reset All Nodes")]
		private void Debug_ResetAllNodes()
		{
			foreach (Stats stat in PaperTreeStats)
			{
				StatAttribute attribute = _player.Attributes.Find(a => a.Stat == stat);
				if (attribute == null) continue;
				PaperTreeModifier modifier = attribute.GetModifier<PaperTreeModifier>();
				if (modifier == null) continue;
				modifier.UnlockedNodes.Clear();
				modifier.Modifier = 0;
			}
			_paperTreeUIController.SetData(_player.Attributes, _paperTreeScrObj, _icons, _player.Gold);
			AppEvents.OnGameContextUpdated?.Invoke();
			Debug.Log("[PaperTreeManager] All nodes reset.");
		}

		#endregion
	}
}