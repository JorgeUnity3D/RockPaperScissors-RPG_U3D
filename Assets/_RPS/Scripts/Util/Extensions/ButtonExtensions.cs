using Doozy.Runtime.UIManager.Components;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Kapibara.RPS.Extensions
{
	public static class ButtonExtensions
	{
		public static void AddListener(this Button button, UnityAction listener, bool removePreviousListener = false)
		{
			if (!button)
			{
				return;
			}
			if (button.GetComponent<UIButton>() != null)
			{
				button.GetComponent<UIButton>().AddListener(listener, removePreviousListener);
				return;
			}
			if (removePreviousListener)
			{
				button.onClick.RemoveAllListeners();
			}
			button.onClick.AddListener(listener);
		}
		public static void AddListener(this UIButton uiButton, UnityAction listener, bool removePreviousListener = false, float cooldown = 0f)
		{
			if (!uiButton)
			{
				return;
			}
			if (removePreviousListener)
			{
				uiButton.onClickBehaviour.Event.RemoveAllListeners();
			}
			uiButton.Cooldown = cooldown;
			uiButton.onClickBehaviour.Event.AddListener(listener);
		}

		public static void Interactable(this UIButton uiButton, bool isInteractable)
		{
			uiButton.interactable = isInteractable;
		}
	}
}