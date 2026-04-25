using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS
{
	[CreateAssetMenu(fileName = "Language", menuName = "RPSRPG/Language")]
	public class LanguageScrObj : SerializedScriptableObject
	{
		[HideLabel, SerializeField] private Language _data;

		public Language Data => _data;
	}
}
