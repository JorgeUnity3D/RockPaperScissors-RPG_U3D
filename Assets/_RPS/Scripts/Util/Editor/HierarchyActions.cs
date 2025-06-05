using UnityEditor;
using UnityEngine;

namespace Kapibara.Util.Editor
{
	public class HierarchyActions
	{
		[MenuItem("GameObject/Separator", priority = 0)]
		public static void CreateSeparator()
		{
			GameObject emptyGameObject = new GameObject();

			emptyGameObject.name = "[--- SEPARATOR ---]";

			emptyGameObject.transform.SetParent(null);

			emptyGameObject.transform.position = Vector3.zero;
			emptyGameObject.transform.rotation = Quaternion.identity;
			emptyGameObject.transform.localScale = Vector3.one;
		}
	}
}
