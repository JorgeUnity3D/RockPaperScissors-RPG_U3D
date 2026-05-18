using System.Collections.Generic;
using Kapibara.UI;
using Kapibara.Util;
using UnityEngine;
using UnityEngine.UI;

namespace Kapibara.RPS
{
	/// <summary>
	/// Lector de comics. Overlay a pantalla completa que muestra una historia página a página.
	/// Cada página instancia un prefab de layout (según ComicPageLayout) con slots nombrados Slot_0, Slot_1...
	/// Las viñetas se revelan de una en una con Next; al agotar la historia, se cierra y dispara OnComicClosed.
	///
	/// Setup requerido en Inspector:
	///   _pageContainer    : RectTransform del área de página (rellena la pantalla).
	///   _layouts          : lista de ComicLayoutEntry (Layout + Prefab). El orden no importa.
	///                       Cada prefab tiene hijos nombrados Slot_0, Slot_1... con Image + CanvasGroup.
	///   _nextButton       : botón para revelar siguiente viñeta / avanzar página.
	///   _closeButton      : botón para cerrar el comic en cualquier momento.
	/// El Canvas de este controller debe tener sortingOrder alto para aparecer sobre todo.
	/// </summary>
	public class ComicPlayerUIController : UIController
	{
		[Header("Layout")]
		[SerializeField] private RectTransform _pageContainer;
		[SerializeField] private List<ComicLayoutEntry> _layouts;

		[Header("Controls")]
		[SerializeField] private Button _nextButton;
		[SerializeField] private Button _closeButton;

		private ComicStoryScrObj _currentStory;
		private int _currentPageIndex;
		private int _currentVignetteIndex;

		private GameObject _currentPageInstance;
		private List<CanvasGroup> _slotGroups;
		private List<RectTransform> _slotRects;
		private List<VignetteAnimation> _slotAnimations;


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
			_nextButton.onClick.AddListener(OnNext);
			_closeButton.onClick.AddListener(Close);
		}

		#endregion

		#region CONTROL

		/// <summary>
		/// Carga una historia y muestra el lector desde la primera página.
		/// Llamado por TheaterManager.PlayStory().
		/// </summary>
		public void SetData(ComicStoryScrObj story)
		{
			_currentStory = story;
			_currentPageIndex = 0;
			ShowPage(0);
			ShowCanvas();
		}

		private void ShowPage(int pageIndex)
		{
			if (_currentPageInstance != null)
			{
				Destroy(_currentPageInstance);
			}

			ComicPageData page = _currentStory.Data.Pages[pageIndex].Data;

			GameObject layoutPrefab = null;
			for (int i = 0; i < _layouts.Count; i++)
			{
				if (_layouts[i].Layout == page.Layout)
				{
					layoutPrefab = _layouts[i].Prefab;
					break;
				}
			}

			if (layoutPrefab == null)
			{
				Debug.LogError($"[ComicPlayerUIController] ShowPage() -> layout prefab faltante para {page.Layout}.");
				return;
			}

			_currentPageInstance = Instantiate(layoutPrefab, _pageContainer);

			_slotGroups    = new List<CanvasGroup>();
			_slotRects     = new List<RectTransform>();
			_slotAnimations = new List<VignetteAnimation>();

			for (int i = 0; i < page.Vignettes.Count; i++)
			{
				Transform slot = _currentPageInstance.transform.Find($"Slot_{i}");
				if (slot == null)
				{
					Debug.LogWarning($"[ComicPlayerUIController] ShowPage() -> Slot_{i} no encontrado en prefab {page.Layout}.");
					continue;
				}

				Image slotImage = slot.GetComponent<Image>();
				if (slotImage != null)
				{
					slotImage.sprite = page.Vignettes[i].Sprite;
				}

				CanvasGroup cg = slot.GetComponent<CanvasGroup>();
				if (cg == null)
				{
					cg = slot.gameObject.AddComponent<CanvasGroup>();
				}
				cg.alpha = 0f;

				_slotGroups.Add(cg);
				_slotRects.Add(slot.GetComponent<RectTransform>());
				_slotAnimations.Add(page.Vignettes[i].Animation);
			}

			_currentVignetteIndex = -1;
		}

		private void OnNext()
		{
			_currentVignetteIndex++;

			if (_currentVignetteIndex < _slotGroups.Count)
			{
				AnimateSlot(_currentVignetteIndex);
				return;
			}

			_currentPageIndex++;
			if (_currentPageIndex < _currentStory.Data.Pages.Count)
			{
				ShowPage(_currentPageIndex);
				return;
			}

			Close();
		}

		private void AnimateSlot(int index)
		{
			CanvasGroup cg   = _slotGroups[index];
			RectTransform rt = _slotRects[index];
			VignetteAnimation anim = _slotAnimations[index];

			switch (anim)
			{
				case VignetteAnimation.FADE_IN:
					UITween.FadeIn(cg, GameConsts.COMIC_VIGNETTE_DURATION);
					break;
				case VignetteAnimation.SLIDE_FROM_LEFT:
					UITween.FadeIn(cg, GameConsts.COMIC_VIGNETTE_DURATION * 0.5f);
					UITween.SlideFrom(rt, SlideDirection.Left, GameConsts.COMIC_SLIDE_DISTANCE, GameConsts.COMIC_VIGNETTE_DURATION);
					break;
				case VignetteAnimation.SLIDE_FROM_RIGHT:
					UITween.FadeIn(cg, GameConsts.COMIC_VIGNETTE_DURATION * 0.5f);
					UITween.SlideFrom(rt, SlideDirection.Right, GameConsts.COMIC_SLIDE_DISTANCE, GameConsts.COMIC_VIGNETTE_DURATION);
					break;
				case VignetteAnimation.SLIDE_FROM_BOTTOM:
					UITween.FadeIn(cg, GameConsts.COMIC_VIGNETTE_DURATION * 0.5f);
					UITween.SlideFrom(rt, SlideDirection.Down, GameConsts.COMIC_SLIDE_DISTANCE, GameConsts.COMIC_VIGNETTE_DURATION);
					break;
				case VignetteAnimation.ZOOM_IN:
					UITween.FadeIn(cg, GameConsts.COMIC_VIGNETTE_DURATION * 0.5f);
					UITween.ZoomIn(rt, 0.7f, GameConsts.COMIC_VIGNETTE_DURATION);
					break;
			}
		}

		private void Close()
		{
			if (_currentPageInstance != null)
			{
				Destroy(_currentPageInstance);
				_currentPageInstance = null;
			}
			HideCanvas();
			AppEvents.OnComicClosed?.Invoke();
		}

		#endregion
	}
}
