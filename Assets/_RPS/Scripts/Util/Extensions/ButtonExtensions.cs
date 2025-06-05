using UnityEngine.Events;
using UnityEngine.UI;

namespace Kapibara.Util.Extensions
{
	public static class ButtonExtensions
	{
		public static void AddListener(this Button button, UnityAction listener, bool removePreviousListener = false)
		{
			if (!button)
			{
				return;
			}
			if (removePreviousListener)
			{
				button.onClick.RemoveAllListeners();
			}
			button.onClick.AddListener(listener);
		}

		//public static void AddListener(this IButton uiButton, UnityAction listener, bool removePreviousListener = false, float cooldown = 0f)
		//{
		//	if (!uiButton)
		//	{
		//		return;
		//	}
		//	if (removePreviousListener)
		//	{
		//		uiButton.onClickBehaviour.Event.RemoveAllListeners();
		//	}
		//	uiButton.Cooldown = cooldown;
		//	uiButton.onClickBehaviour.Event.AddListener(listener);
		//}		
	}
}