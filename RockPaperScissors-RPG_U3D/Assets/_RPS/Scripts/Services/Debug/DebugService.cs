using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS
{
	/// <summary>
	/// Servicio de debug centralizado. Agrupa todas las operaciones de estado que antes vivían como
	/// [Button] dispersos en managers. Diseñado para ser conducido por DebugUIController en runtime
	/// (útil en builds de desarrollo donde el Inspector de Odin no está disponible).
	/// Persiste entre escenas al ser parte de GameCore.
	/// </summary>
	public class DebugService : ServiceSubscriber<DebugService>
	{
		[SerializeField, ReadOnly] private DebugUIController _debugUIController;

		private PaperTreeScrObj   _paperTreeScrObj;
		private StoneSmithyScrObj _stoneSmithyScrObj;

		private PaperTreeScrObj   PaperTreeScrObj   => _paperTreeScrObj   ??= ServiceLocator.Instance.GetService<StaticDataService>()?.PaperTreeSkillTrees;
		private StoneSmithyScrObj StoneSmithyScrObj => _stoneSmithyScrObj ??= ServiceLocator.Instance.GetService<StaticDataService>()?.StoneSmithyItems;

		private static readonly Stats[] PaperTreeStats =
		{
			Stats.ROCK, Stats.PAPER, Stats.SCISSOR, Stats.DEFENSE, Stats.ENERGY_RECOVERY
		};

		// ── UI control ─────────────────────────────────────────────────────────

		public void ShowDebugUI()   => _debugUIController?.Show();
		public void HideDebugUI()   => _debugUIController?.Hide();
		public void ToggleDebugUI() => _debugUIController?.Toggle();

		// ── Guard ──────────────────────────────────────────────────────────────

		private bool IsReady(string caller)
		{
			if (AppContext.GameContext != null) return true;
			Debug.LogWarning($"[DebugService] {caller} — no game context loaded.");
			return false;
		}

		private static void Save() => AppEvents.OnGameContextUpdated?.Invoke();

		// ── Economy ────────────────────────────────────────────────────────────

		[FoldoutGroup("Economy"), Button("Add Gold")]
		public void AddGold(int amount = 100)
		{
			if (!IsReady(nameof(AddGold))) return;
			AppContext.Player.Gold += amount;
			Save();
			Debug.Log($"[DebugService] AddGold({amount}) → {AppContext.Player.Gold}");
		}

		[FoldoutGroup("Economy"), Button("Set Gold")]
		public void SetGold(int amount = 0)
		{
			if (!IsReady(nameof(SetGold))) return;
			AppContext.Player.Gold = Mathf.Max(0, amount);
			Save();
			Debug.Log($"[DebugService] SetGold({amount}) → {AppContext.Player.Gold}");
		}

		// ── Player level / ScissorBonfire ──────────────────────────────────────

		[FoldoutGroup("Player Level"), Button("Force Level Up")]
		public void ForceLevelUp()
		{
			if (!IsReady(nameof(ForceLevelUp))) return;
			int levelIndex = AppContext.Player.Level;
			if (levelIndex >= GameConsts.SCISSOR_MODS.Count)
			{
				Debug.Log("[DebugService] ForceLevelUp — already at max level.");
				return;
			}
			AppContext.Player.Level++;
			List<StatAttribute> attrs = AppContext.Player.Attributes.FindAll(a => a.GetModifier<ScissorBonfireModifier>() != null);
			attrs.ForEach(a =>
			{
				ScissorBonfireModifier mod = a.GetModifier<ScissorBonfireModifier>();
				mod.Level    = AppContext.Player.Level;
				mod.Modifier += GameConsts.SCISSOR_MODS[levelIndex][a.Stat];
			});
			Save();
			Debug.Log($"[DebugService] ForceLevelUp → level {AppContext.Player.Level}");
		}

		[FoldoutGroup("Player Level"), Button("Reset Player Level")]
		public void ResetPlayerLevel()
		{
			if (!IsReady(nameof(ResetPlayerLevel))) return;
			AppContext.Player.Level = 1;
			List<StatAttribute> attrs = AppContext.Player.Attributes.FindAll(a => a.GetModifier<ScissorBonfireModifier>() != null);
			attrs.ForEach(a =>
			{
				ScissorBonfireModifier mod = a.GetModifier<ScissorBonfireModifier>();
				mod.Level    = 1;
				mod.Modifier = 0;
			});
			Save();
			Debug.Log("[DebugService] ResetPlayerLevel → level 1, all ScissorBonfireModifiers cleared.");
		}

		// ── Items / StoneSmithy ────────────────────────────────────────────────

		[FoldoutGroup("Items"), Button("Max All Items")]
		public void MaxAllItems()
		{
			if (!IsReady(nameof(MaxAllItems))) return;
			if (StoneSmithyScrObj == null) { Debug.LogWarning("[DebugService] MaxAllItems — StoneSmithyScrObj not available."); return; }
			AppContext.Player.AttackItemLevel = StoneSmithyScrObj.Data.attackItem.amountsPerLevel.Count;
			AppContext.Player.HealItemLevel   = StoneSmithyScrObj.Data.healItem.amountsPerLevel.Count;
			AppContext.Player.EnergyItemLevel = StoneSmithyScrObj.Data.energyItem.amountsPerLevel.Count;
			Save();
			Debug.Log("[DebugService] MaxAllItems — Attack/Heal/Energy maxed.");
		}

		[FoldoutGroup("Items"), Button("Reset All Items")]
		public void ResetAllItems()
		{
			if (!IsReady(nameof(ResetAllItems))) return;
			AppContext.Player.AttackItemLevel = 0;
			AppContext.Player.HealItemLevel   = 0;
			AppContext.Player.EnergyItemLevel = 0;
			Save();
			Debug.Log("[DebugService] ResetAllItems — all item levels set to 0.");
		}

		// ── Paper Tree ─────────────────────────────────────────────────────────

		[FoldoutGroup("Paper Tree"), Button("Unlock All Nodes")]
		public void UnlockAllPaperTreeNodes()
		{
			if (!IsReady(nameof(UnlockAllPaperTreeNodes))) return;
			if (PaperTreeScrObj == null) { Debug.LogWarning("[DebugService] UnlockAllPaperTreeNodes — PaperTreeScrObj not available."); return; }
			foreach (Stats stat in PaperTreeStats)
			{
				StatAttribute attribute = AppContext.Player.Attributes.Find(a => a.Stat == stat);
				if (attribute == null) continue;
				PaperTreeModifier modifier = attribute.GetModifier<PaperTreeModifier>();
				if (modifier == null) continue;
				List<PaperTreeNode> tree = PaperTreeScrObj[stat];
				if (tree == null) continue;
				foreach (PaperTreeNode node in tree)
				{
					if (modifier.UnlockedNodes.Contains(node.NodeID)) continue;
					modifier.UnlockedNodes.Add(node.NodeID);
					modifier.Modifier += node.Modifier;
				}
			}
			Save();
			Debug.Log("[DebugService] UnlockAllPaperTreeNodes — all nodes unlocked.");
		}

		[FoldoutGroup("Paper Tree"), Button("Reset All Nodes")]
		public void ResetAllPaperTreeNodes()
		{
			if (!IsReady(nameof(ResetAllPaperTreeNodes))) return;
			foreach (Stats stat in PaperTreeStats)
			{
				StatAttribute attribute = AppContext.Player.Attributes.Find(a => a.Stat == stat);
				if (attribute == null) continue;
				PaperTreeModifier modifier = attribute.GetModifier<PaperTreeModifier>();
				if (modifier == null) continue;
				modifier.UnlockedNodes.Clear();
				modifier.Modifier = 0;
			}
			Save();
			Debug.Log("[DebugService] ResetAllPaperTreeNodes — all trees cleared.");
		}

		// ── Stories / Theater ──────────────────────────────────────────────────

		[FoldoutGroup("Stories"), Button("Unlock All Stories")]
		public void UnlockAllStories()
		{
			if (!IsReady(nameof(UnlockAllStories))) return;
			for (int i = 0; i < 10; i++)
				AppContext.Player.UnlockStory(i);
			Save();
			Debug.Log("[DebugService] UnlockAllStories — stories 0-9 unlocked.");
		}

		[FoldoutGroup("Stories"), Button("Lock All Stories")]
		public void LockAllStories()
		{
			if (!IsReady(nameof(LockAllStories))) return;
			AppContext.Player.UnlockedStoryIds.Clear();
			AppContext.Player.UnlockedStoryIds.Add(0);
			Save();
			Debug.Log("[DebugService] LockAllStories — reset to story 0 only.");
		}

		// ── Library ────────────────────────────────────────────────────────────

		[FoldoutGroup("Library"), Button("Complete All Quests")]
		public void CompleteAllLibraryQuests()
		{
			if (!IsReady(nameof(CompleteAllLibraryQuests))) return;
			List<LibraryQuestProgress> quests = AppContext.GameContext.LibraryQuests;
			if (quests == null || quests.Count == 0)
			{
				Debug.Log("[DebugService] CompleteAllLibraryQuests — no quests materialized yet.");
				return;
			}
			foreach (LibraryQuestProgress quest in quests)
			{
				if (quest.IsCompleted) continue;
				quest.CurrentKills = quest.TargetKills;
				quest.IsCompleted  = true;
				StatAttribute    attribute   = AppContext.Player.Attributes.Find(a => a.Stat == quest.RewardStat);
				LibraryModifier  libraryMod  = attribute?.GetModifier<LibraryModifier>();
				if (libraryMod != null) libraryMod.Modifier += quest.RewardAmount;
			}
			Save();
			Debug.Log("[DebugService] CompleteAllLibraryQuests — all quests completed.");
		}

		[FoldoutGroup("Library"), Button("Reset Quests")]
		public void ResetLibraryQuests()
		{
			if (!IsReady(nameof(ResetLibraryQuests))) return;
			AppContext.GameContext.LibraryQuests?.Clear();
			Save();
			Debug.Log("[DebugService] ResetLibraryQuests — quest list cleared.");
		}

		// ── Town buildings ─────────────────────────────────────────────────────

		[FoldoutGroup("Town"), Button("Unlock All Buildings")]
		public void UnlockAllBuildings()
		{
			if (!IsReady(nameof(UnlockAllBuildings))) return;
			foreach (TownData data in AppContext.TownData)
				data.IsUnlocked = true;
			Save();
			Debug.Log("[DebugService] UnlockAllBuildings — all buildings unlocked.");
		}

		[FoldoutGroup("Town"), Button("Rescue All NPCs")]
		public void RescueAllNPCs()
		{
			if (!IsReady(nameof(RescueAllNPCs))) return;
			foreach (TownData data in AppContext.TownData)
			{
				data.IsUnlocked  = true;
				data.NpcUnlocked = true;
			}
			Save();
			Debug.Log("[DebugService] RescueAllNPCs — all buildings unlocked + NPCs rescued.");
		}

		[FoldoutGroup("Town"), Button("Reset All Buildings")]
		public void ResetAllBuildings()
		{
			if (!IsReady(nameof(ResetAllBuildings))) return;
			foreach (TownData data in AppContext.TownData)
			{
				data.IsUnlocked  = false;
				data.NpcUnlocked = false;
				data.Level       = 0;
				data.Experience  = 0;
			}
			Save();
			Debug.Log("[DebugService] ResetAllBuildings — all buildings locked and reset.");
		}
	}
}
