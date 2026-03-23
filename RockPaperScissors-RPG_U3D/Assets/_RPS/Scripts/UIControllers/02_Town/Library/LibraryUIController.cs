using System.Collections.Generic;
using Kapibara.UI;
using UnityEngine;

namespace Kapibara.RPS
{
	public class LibraryUIController : UIController
	{
		[Header("UI")]
		[SerializeField] private Transform _cardContainer;
		[SerializeField] private LibraryQuestCard _questCardPrefab;

		#region UNITY_LIFECYCLE

		private void Awake()
		{
			SetUp();
		}

		#endregion

		#region SETUP

		public override void SetUp()
		{
			HideCanvas(0);
		}

		#endregion

		#region CONTROL

		/// <summary>
		/// Instancia una tarjeta por cada quest de todas las páginas.
		/// killCounts está indexado por posición plana de quest (igual que Player._libraryKillCounts).
		/// </summary>
		public void SetData(LibraryData libraryData, List<int> killCounts)
		{
			int questIndex = 0;
			foreach (LibraryPageData page in libraryData.Pages)
			{
				foreach (LibraryQuestData quest in page.Quests)
				{
					int kills = questIndex < killCounts.Count ? killCounts[questIndex] : 0;
					LibraryQuestCard card = Instantiate(_questCardPrefab, _cardContainer);
					card.SetData(quest, kills);
					questIndex++;
				}
			}
		}

		#endregion
	}
}
