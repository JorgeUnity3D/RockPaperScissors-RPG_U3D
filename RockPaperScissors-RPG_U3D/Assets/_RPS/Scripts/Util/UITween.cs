using DG.Tweening;
using UnityEngine;

namespace Kapibara.Util
{
	/// <summary>
	/// Dirección de entrada para animaciones de deslizamiento.
	/// </summary>
	public enum SlideDirection { LEFT, RIGHT, UP, DOWN }

	/// <summary>
	/// Utilidad estática de animaciones DOTween reutilizables para UI.
	/// Todos los métodos devuelven el Tween creado para permitir encadenado con OnComplete, etc.
	/// </summary>
	public static class UITween
	{
		/// <summary>
		/// Hace fade-in del CanvasGroup de 0 a 1.
		/// Fuerza alpha a 0 al inicio del tween (no al inicio del delay).
		/// </summary>
		public static Tween FadeIn(CanvasGroup cg, float duration, float delay = 0f)
		{
			cg.alpha = 0f;
			return cg.DOFade(1f, duration).SetDelay(delay);
		}

		/// <summary>
		/// Hace fade-out del CanvasGroup de su alpha actual a 0.
		/// </summary>
		public static Tween FadeOut(CanvasGroup cg, float duration, float delay = 0f)
		{
			return cg.DOFade(0f, duration).SetDelay(delay);
		}

		/// <summary>
		/// Desplaza el RectTransform desde un offset en la dirección indicada hasta su posición original.
		/// Salta inmediatamente a la posición de inicio y luego anima al origen.
		/// </summary>
		public static Tween SlideFrom(RectTransform rt, SlideDirection direction, float distance, float duration, float delay = 0f)
		{
			Vector2 origin = rt.anchoredPosition;
			Vector2 offset = direction switch
			{
				SlideDirection.LEFT   => new Vector2(-distance, 0f),
				SlideDirection.RIGHT  => new Vector2( distance, 0f),
				SlideDirection.UP     => new Vector2(0f,  distance),
				SlideDirection.DOWN   => new Vector2(0f, -distance),
				_                     => Vector2.zero
			};
			rt.anchoredPosition = origin + offset;
			return rt.DOAnchorPos(origin, duration).SetDelay(delay);
		}

		/// <summary>
		/// Anima el RectTransform desde fromScale hasta escala 1 con un efecto de rebote (OutBack).
		/// </summary>
		public static Tween ZoomIn(RectTransform rt, float fromScale, float duration, float delay = 0f)
		{
			rt.localScale = Vector3.one * fromScale;
			return rt.DOScale(Vector3.one, duration).SetDelay(delay).SetEase(Ease.OutBack);
		}

		/// <summary>
		/// Desplaza el RectTransform desde su posición actual hasta un offset en la dirección indicada.
		/// Inverso de SlideFrom.
		/// </summary>
		public static Tween SlideTo(RectTransform rt, SlideDirection direction, float distance, float duration, float delay = 0f)
		{
			Vector2 origin = rt.anchoredPosition;
			Vector2 offset = direction switch
			{
				SlideDirection.LEFT   => new Vector2(-distance, 0f),
				SlideDirection.RIGHT  => new Vector2( distance, 0f),
				SlideDirection.UP     => new Vector2(0f,  distance),
				SlideDirection.DOWN   => new Vector2(0f, -distance),
				_                     => Vector2.zero
			};
			return rt.DOAnchorPos(origin + offset, duration).SetDelay(delay);
		}

		/// <summary>
		/// Anima el RectTransform desde escala 1 hasta toScale con aceleración (InCubic).
		/// </summary>
		public static Tween ZoomOut(RectTransform rt, float toScale, float duration, float delay = 0f)
		{
			rt.localScale = Vector3.one;
			return rt.DOScale(Vector3.one * toScale, duration).SetDelay(delay).SetEase(Ease.InCubic);
		}
	}
}
