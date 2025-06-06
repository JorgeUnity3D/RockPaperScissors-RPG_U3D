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
	}
}