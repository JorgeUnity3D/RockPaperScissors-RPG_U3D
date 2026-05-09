using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS
{
	[CreateAssetMenu(fileName = "TreasureStep", menuName = "RPSRPG/Map/TreasureStep")]
	public class TreasureStepScrObj : SerializedScriptableObject
	{
		[SerializeField] private int _goldAmount;

		public int GoldAmount => _goldAmount;
	}
}
