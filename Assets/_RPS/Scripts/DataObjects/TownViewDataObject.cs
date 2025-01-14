using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS
{
	[CreateAssetMenu(fileName = "TownViews", menuName = "RPSRPG/TownViews")]
	public class TownViewDataObject : SerializedScriptableObject
	{
		[ListDrawerSettings(ListElementLabelName = "townMenu")]
		[SerializeField] private List<TownData> _data;
		public List<TownData> Data => _data;

	}
}