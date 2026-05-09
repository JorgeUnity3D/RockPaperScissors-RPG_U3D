using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS
{
	[CreateAssetMenu(fileName = "Item", menuName = "RPSRPG/Player/Item")]
	public class ItemScrObj : SerializedScriptableObject
	{
		[HideLabel, SerializeField] private Item _data;

		public Item Data => _data;
	}
}
