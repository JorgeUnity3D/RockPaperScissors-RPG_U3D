using System.Collections.Generic;
using Kapibara.UI;
using UnityEngine;

namespace Kapibara.RPS
{
	/// <summary>
	/// Galería del Theater. Muestra la lista de historias disponibles como botones.
	/// Las historias bloqueadas aparecen con overlay de candado e interacción deshabilitada.
	/// Al pulsar un botón desbloqueado dispara AppEvents.OnStorySelected(index).
	/// </summary>
	public class TheaterUIController : UIController
	{
		[Header("UI")]
		[SerializeField] private RectTransform _storiesHolder;
		[SerializeField] private GameObject _storyButtonPrefab;

		private List<StoryButton> _storyButtons = new List<StoryButton>();

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
		/// Popula la galería con la lista de historias y los IDs de las desbloqueadas.
		/// Llamado desde TheaterManager.Initialize().
		/// </summary>
		public void SetData(List<ComicStoryScrObj> stories, List<int> unlockedStoryIds)
		{
			foreach (StoryButton btn in _storyButtons)
			{
				Destroy(btn.gameObject);
			}
			_storyButtons.Clear();

			for (int i = stories.Count - 1; i >= 0; i--)
			{
				int index = i;
				bool isUnlocked = unlockedStoryIds.Contains(stories[i].StoryId);
				StoryButton storyButton = Instantiate(_storyButtonPrefab, _storiesHolder).GetComponent<StoryButton>();
				storyButton.SetUp(stories[i], isUnlocked, () => AppEvents.OnStorySelected?.Invoke(index));
				_storyButtons.Add(storyButton);
			}
		}

		#endregion
	}
}
