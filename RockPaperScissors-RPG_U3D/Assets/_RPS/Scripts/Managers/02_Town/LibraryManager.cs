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

#if UNITY_EDITOR
		[Button("Simulate NPC Rescued")]
		private void Debug_SimulateNpcRescued()
		{
			TownData libraryTownData = AppContext.TownData?.Find(td => td.TownMenu == TownMenu.LIBRARY);
			if (libraryTownData == null) { Debug.Log("[LibraryManager] No TownData for LIBRARY found."); return; }
			libraryTownData.NpcUnlocked = true;
			Debug.Log("[LibraryManager] NpcUnlocked set to true for LIBRARY.");
		}
#endif

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
