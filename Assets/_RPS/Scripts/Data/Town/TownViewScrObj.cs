using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS
{
	[CreateAssetMenu(fileName = "TownViews", menuName = "RPSRPG/TownViews")]
	public class TownViewScrObj : SerializedScriptableObject
	{
		[HideLabel, ListDrawerSettings(ListElementLabelName = "townMenu"), SerializeField] private List<TownView> _data;
		public List<TownView> Data => _data;
	}
}