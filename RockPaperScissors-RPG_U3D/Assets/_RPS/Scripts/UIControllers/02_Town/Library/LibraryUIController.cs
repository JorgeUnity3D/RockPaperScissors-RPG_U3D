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
		/// Instancia una tarjeta por cada quest visible.
		/// quests proviene de GameContext.LibraryQuests — ya materializado.
		/// unlockedPageCount determina qué páginas se muestran.
		/// </summary>
		public void SetData(List<LibraryQuestProgress> quests, int unlockedPageCount)
		{
			foreach (Transform child in _cardContainer)
				Destroy(child.gameObject);

			foreach (LibraryQuestProgress quest in quests)
			{
				if (quest.PageIndex >= unlockedPageCount) continue;
				LibraryQuestCard card = Instantiate(_questCardPrefab, _cardContainer);
				card.SetData(quest);
			}
		}

		#endregion
	}
}
