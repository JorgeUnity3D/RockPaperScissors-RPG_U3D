using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS
{
	/// <summary>
	/// Manager del Theater. Gestiona la galería de historias y el lanzamiento del lector de comics.
	/// Flujo: Initialize() → TheaterUIController.SetData() con la lista de historias y el conteo desbloqueado.
	/// Al pulsar un botón de historia → AppEvents.OnStorySelected → PlayStory() → ComicPlayerUIController.SetData().
	/// Al cerrar el comic → AppEvents.OnComicClosed (el Theater ya está visible, no requiere acción extra).
	/// </summary>
	public class TheaterManager : BaseManager, ITownBuilding
	{
		private TheaterScrObj _theaterScrObj;

		[Header("DEBUG")]
		[SerializeField, ReadOnly] private TheaterUIController _theaterUIController;
		[SerializeField, ReadOnly] private ComicPlayerUIController _comicPlayerUIController;

		#region SETUP

		public override void SetUp()
		{
			Debug.Log("[TheaterManager] SetUp()");
			_theaterScrObj = ServiceLocator.Instance.GetService<StaticDataService>().TheaterData;
			UIService uiService = ServiceLocator.Instance.GetService<UIService>();
			_theaterUIController    = uiService.GetController<TheaterUIController>();
			_comicPlayerUIController = uiService.GetController<ComicPlayerUIController>();
		}

		protected override void Subscribe()
		{
			AppEvents.OnStorySelected += PlayStory;
			AppEvents.OnComicClosed   += OnComicClosed;
		}

		protected override void UnSubscribe()
		{
			AppEvents.OnStorySelected -= PlayStory;
			AppEvents.OnComicClosed   -= OnComicClosed;
		}

		#endregion

		#region CONTROL

		public void OnMenuOpen()
		{
			Debug.Log("[TheaterManager] OnMenuOpen()");
			List<ComicStoryScrObj> stories = _theaterScrObj.Data;
			List<int> unlockedStoryIds = AppContext.Player.UnlockedStoryIds;
			_theaterUIController.SetData(stories, unlockedStoryIds);
		}

		private void PlayStory(int index)
		{
			Debug.Log($"[TheaterManager] PlayStory() -> index: {index}");

			List<ComicStoryScrObj> stories = _theaterScrObj.Data;

			if (index < 0 || index >= stories.Count)
			{
				Debug.LogWarning($"[TheaterManager] PlayStory() -> índice {index} fuera de rango (total: {stories.Count}).");
				return;
			}

			if (!AppContext.Player.IsStoryUnlocked(index))
			{
				Debug.LogWarning($"[TheaterManager] PlayStory() -> historia {index} bloqueada.");
				return;
			}

			_comicPlayerUIController.SetData(stories[index]);
		}

		private void OnComicClosed()
		{
			Debug.Log("[TheaterManager] OnComicClosed() -> comic cerrado, Theater permanece visible.");
		}

		#endregion
	}
}
