using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS
{
	/// <summary>
	/// Manager de la Biblioteca. Materializa las quests en GameContext la primera vez que se abre
	/// (solo si el NPC está rescatado). El tracking de kills y las recompensas viven en GameManager.
	/// </summary>
	public class LibraryManager : BaseManager, ITownBuilding
	{
		private LibraryScrObj _libraryScrObj;
		[Header("DEBUG")]
		[SerializeField, ReadOnly] private LibraryUIController _libraryUIController;

		#region SETUP

		public override void SetUp()
		{
			Debug.Log($"[LibraryManager] SetUp() -> ");
			_libraryScrObj = ServiceLocator.Instance.GetService<StaticDataService>().LibraryQuests;
			_libraryUIController = ServiceLocator.Instance.GetService<UIService>().GetController<LibraryUIController>();
		}

		protected override void Subscribe() { }

		protected override void UnSubscribe() { }

		#endregion

		#region DEBUG

		[FoldoutGroup("DEBUG")]
		[ValueDropdown("GetDebugQuestOptions")]
		[SerializeField] private int _debugQuestIndex;

		private IEnumerable<ValueDropdownItem> GetDebugQuestOptions()
		{
			List<LibraryQuestProgress> quests = AppContext.GameContext?.LibraryQuests;
			if (quests == null || quests.Count == 0)
			{
				yield return new ValueDropdownItem("(no quests)", 0);
				yield break;
			}
			for (int i = 0; i < quests.Count; i++)
			{
				LibraryQuestProgress q = quests[i];
				string label = $"[{i}] {q.EnemyDisplayName}  {q.CurrentKills}/{q.TargetKills}";
				if (q.IsCompleted) label += "  ✓";
				yield return new ValueDropdownItem(label, i);
			}
		}

		[FoldoutGroup("DEBUG"), Button("Add Progress")]
		private void Debug_AddProgress()
		{
			List<LibraryQuestProgress> quests = AppContext.GameContext.LibraryQuests;
			if (quests == null || quests.Count == 0) { Debug.Log("[LibraryManager] No quests materialized."); return; }
			if (_debugQuestIndex >= quests.Count) { Debug.Log("[LibraryManager] Quest index out of range."); return; }
			LibraryQuestProgress quest = quests[_debugQuestIndex];
			if (quest.IsCompleted) { Debug.Log($"[LibraryManager] Quest [{_debugQuestIndex}] already completed."); return; }
			quest.CurrentKills = Mathf.Min(quest.CurrentKills + 1, quest.TargetKills);
			_libraryUIController.SetData(quests);
			AppEvents.OnGameContextUpdated?.Invoke();
			Debug.Log($"[LibraryManager] [{_debugQuestIndex}] {quest.EnemyDisplayName} — {quest.CurrentKills}/{quest.TargetKills}");
		}

		[FoldoutGroup("DEBUG"), Button("Complete Quest")]
		private void Debug_CompleteQuest()
		{
			List<LibraryQuestProgress> quests = AppContext.GameContext.LibraryQuests;
			if (quests == null || quests.Count == 0) { Debug.Log("[LibraryManager] No quests materialized."); return; }
			if (_debugQuestIndex >= quests.Count) { Debug.Log("[LibraryManager] Quest index out of range."); return; }
			LibraryQuestProgress quest = quests[_debugQuestIndex];
			if (quest.IsCompleted) { Debug.Log($"[LibraryManager] Quest [{_debugQuestIndex}] already completed."); return; }
			quest.CurrentKills = quest.TargetKills;
			quest.IsCompleted  = true;
			Player player = AppContext.Player;
			StatAttribute attribute = player.Attributes.Find(a => a.Stat == quest.RewardStat);
			if (attribute != null)
			{
				LibraryModifier modifier = attribute.GetModifier<LibraryModifier>();
				if (modifier != null)
				{
					modifier.Modifier += quest.RewardAmount;
					Debug.Log($"[LibraryManager] Quest complete — +{quest.RewardAmount} {quest.RewardStat}");
				}
			}
			_libraryUIController.SetData(quests);
			AppEvents.OnGameContextUpdated?.Invoke();
		}

		[FoldoutGroup("DEBUG"), Button("Reset Quests")]
		private void Debug_ResetQuests()
		{
			AppContext.GameContext.LibraryQuests.Clear();
			Debug.Log("[LibraryManager] All quests cleared.");
			AppEvents.OnGameContextUpdated?.Invoke();
		}

		#endregion

		#region CONTROL

		public void OnMenuOpen()
		{
			Debug.Log($"[LibraryManager] OnMenuOpen() -> ");
			MaterializeIfNeeded();
			_libraryUIController.SetData(AppContext.GameContext.LibraryQuests);
		}

		private void MaterializeIfNeeded()
		{
			List<LibraryQuestProgress> quests = AppContext.GameContext.LibraryQuests;
			if (quests.Count > 0) return;

			TownData libraryTownData = AppContext.TownData.Find(td => td.TownMenu == TownMenu.LIBRARY);
			if (libraryTownData == null || !libraryTownData.NpcUnlocked) return;

			List<LibraryPageData> pages = _libraryScrObj.Data.Pages;
			for (int pageIndex = 0; pageIndex < pages.Count; pageIndex++)
			{
				foreach (LibraryQuestData quest in pages[pageIndex].Quests)
				{
					if (quest.TargetEnemy == null) continue;
					quests.Add(new LibraryQuestProgress(
						quest.TargetEnemy.Data.Id,
						quest.TargetEnemy.Data.Name,
						quest.TargetKills,
						quest.RewardStat,
						quest.RewardAmount,
						pageIndex
					));
				}
			}

			Debug.Log($"[LibraryManager] Materialized {quests.Count} quests.");
			AppEvents.OnGameContextUpdated?.Invoke();
		}

#endregion
	}
}
