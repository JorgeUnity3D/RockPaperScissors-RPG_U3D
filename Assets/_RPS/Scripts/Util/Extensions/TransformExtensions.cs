using UnityEngine;

namespace Kapibara.RPS.Extensions
{
	public static class TransformExtensions
	{
		public static Transform FindDeepChild(this Transform parent, string name)
		{
			Transform result = parent.Find(name);
			if (result != null)
			{
				return result;
			}

			foreach (Transform child in parent)
			{
				result = child.FindDeepChild(name);
				if (result != null)
				{
					return result;
				}
			}
			return null;
		}

		public static void DestroyChildren(this Transform parent)
		{
			foreach (Transform child in parent)
			{
				Object.Destroy(child.gameObject);
			}
		}
	}
}