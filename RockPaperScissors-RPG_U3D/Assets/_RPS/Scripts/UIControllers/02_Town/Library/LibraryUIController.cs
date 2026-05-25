using System.Collections.Generic;
using Kapibara.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Kapibara.RPS
{
	public class LibraryUIController : UIController
	{
		[Header("UI")]
		[SerializeField] private Transform        _questPageContent;
		[SerializeField] private Transform        _hiddenPageContent;
		[SerializeField] private LibraryQuestCard _questCardPrefab;
		[SerializeField] private Button           _pageButtonPrefab;
		[SerializeField] private Transform        _pageButtonContainer;

		private List<List<LibraryQuestCard>> _cardsByPage = new List<List<LibraryQuestCard>>();
		private int                          _currentPage = -1;

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

		public void SetData(List<LibraryQuestProgress> quests)
		{
			foreach (List<LibraryQuestCard> page in _cardsByPage)
				foreach (LibraryQuestCard card in page)
					Destroy(card.gameObject);
			_cardsByPage.Clear();

			foreach (Transform child in _pageButtonContainer)
				Destroy(child.gameObject);

			int totalPages = 0;
			foreach (LibraryQuestProgress quest in quests)
				if (quest.PageIndex + 1 > totalPages) totalPages = quest.PageIndex + 1;

			for (int i = 0; i < totalPages; i++)
				_cardsByPage.Add(new List<LibraryQuestCard>());

			foreach (LibraryQuestProgress quest in quests)
			{
				LibraryQuestCard card = Instantiate(_questCardPrefab, _hiddenPageContent);
				card.SetData(quest);
				_cardsByPage[quest.PageIndex].Add(card);
			}

			for (int i = totalPages - 1; i >= 0; i--)
			{
				int pageIndex = i;
				Button btn = Instantiate(_pageButtonPrefab, _pageButtonContainer);
				btn.onClick.AddListener(() => ShowPage(pageIndex));
			}

			_currentPage = -1;
			ShowPage(0);
		}

		private void ShowPage(int pageIndex)
		{
			if (pageIndex == _currentPage) return;
			if (pageIndex >= _cardsByPage.Count) return;

			if (_currentPage >= 0 && _currentPage < _cardsByPage.Count)
				foreach (LibraryQuestCard card in _cardsByPage[_currentPage])
					card.transform.SetParent(_hiddenPageContent, false);

			foreach (LibraryQuestCard card in _cardsByPage[pageIndex])
				card.transform.SetParent(_questPageContent, false);

			_currentPage = pageIndex;
		}

		#endregion
	}
}
