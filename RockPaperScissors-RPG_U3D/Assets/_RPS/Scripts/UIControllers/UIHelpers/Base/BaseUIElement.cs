using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Kapibara.UI
{
	/// <summary>
	/// Clase base para todos los elementos de UI. Gestiona mostrar/ocultar mediante DOTween fade sobre el CanvasGroup.
	/// </summary>
	public abstract class BaseUIElement : MonoBehaviour
	{
        #region FIELDS

		private CanvasGroup _canvasGroup;
		private Canvas _canvas;

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

		/// <summary>Inicializa referencias y configura listeners. Llamado en Awake por las subclases.</summary>
		public abstract void SetUp();

		/// <summary>Muestra el canvas con fade de 0.5 s.</summary>
		public void ShowCanvas()
		{
			ShowCanvas(CanvasGroup);
		}

		/// <summary>Oculta el canvas con fade de 0.5 s.</summary>
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

		/// <summary>Habilita interacción y raycasts del CanvasGroup.</summary>
		public void EnableInteraction()
		{
			CanvasGroup.interactable = true;
			CanvasGroup.blocksRaycasts = true;
		}

		/// <summary>Deshabilita interacción y raycasts del CanvasGroup.</summary>
		public void DisableInteraction()
		{
			CanvasGroup.interactable = false;
			CanvasGroup.blocksRaycasts = false;
		}

		private void RefreshLayoutGroupsImmediateAndRecursive()
		{
			foreach (RectTransform rt in GetComponentsInChildren<RectTransform>())
			{
				LayoutRebuilder.ForceRebuildLayoutImmediate(rt);
			}
		}

        #endregion
	}
}