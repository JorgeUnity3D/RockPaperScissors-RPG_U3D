using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Kapibara.UI
{
	public abstract class BaseUIElement : MonoBehaviour
	{
        #region FIELDS

		private CanvasGroup _canvasGroup;
		private Canvas _canvas;
		private List<RectTransform> _rectTransforms;

		private Action _onShowAction;
		private Action _onHideAction;

        #endregion

        #region PROPERTIES

		private CanvasGroup CanvasGroup
		{
			get
			{
				if (_canvasGroup == null)
				{
					_canvasGroup = GetComponent<CanvasGroup>();
				}
				return _canvasGroup;
			}
		}

		private Canvas Canvas
		{
			get
			{
				if (_canvas == null)
				{
					_canvas = GetComponent<Canvas>();
				}
				return _canvas;
			}
		}

        #endregion

        #region CONTROL

		public abstract void SetUp();
		
		public void ShowCanvas()
		{
			ShowCanvas(CanvasGroup);
		}

		public void HideCanvas()
		{
			HideCanvas(CanvasGroup);
		}

		public void ShowCanvas(Action OnShowAction = null, Action OnHideAction = null)
		{
			ShowCanvas(CanvasGroup, 0.5f, OnShowAction, OnHideAction);
		}

		public void HideCanvas(Action OnHideAction = null)
		{
			HideCanvas(CanvasGroup, 0.5f, OnHideAction);
		}

		public void ShowCanvas(float duration, Action OnShowAction = null, Action OnHideAction = null)
		{
			ShowCanvas(CanvasGroup, duration, OnShowAction, OnHideAction);
		}

		public void HideCanvas(float duration, Action OnHideAction = null)
		{
			HideCanvas(CanvasGroup, duration, OnHideAction);
		}

		public void ShowCanvas(CanvasGroup canvasGroup, float duration = 0.5f, Action onShowAction = null, Action OnHideAction = null)
		{

			if (Canvas != null)
			{
				Canvas.enabled = true;
			}

			_onShowAction = onShowAction;
			_onHideAction = OnHideAction;

			EnableInteraction();
			RefreshUi();
			canvasGroup.DOKill();
			canvasGroup.DOFade(1, duration).OnComplete(() =>
			{
				_onShowAction?.Invoke();
			});
		}

		public void HideCanvas(CanvasGroup canvasGroup, float duration = 0.5f, Action OnHideAction = null)
		{
			//IsVisible = false;
			DisableInteraction();
			canvasGroup.DOKill();
			canvasGroup.DOFade(0, duration).OnComplete(() =>
			{
				OnHideAction?.Invoke();
				_onHideAction?.Invoke();
				if (Canvas != null)
				{
					Canvas.enabled = false;
				}
			});
		}

		public void RefreshUi()
		{
			RefreshLayoutGroupsImmediateAndRecursive();
		}

		public void EnableInteraction()
		{
			CanvasGroup.interactable = true;
			CanvasGroup.blocksRaycasts = true;
		}

		public void DisableInteraction()
		{
			CanvasGroup.interactable = false;
			CanvasGroup.blocksRaycasts = false;
		}

		private void RefreshLayoutGroupsImmediateAndRecursive()
		{
			if (_rectTransforms == null) return;

			foreach (RectTransform rt in _rectTransforms)
			{
				LayoutRebuilder.ForceRebuildLayoutImmediate(rt);
			}
		}

        #endregion
	}
}