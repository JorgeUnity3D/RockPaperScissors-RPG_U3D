using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS
{
	[CreateAssetMenu(fileName = "NpcRescueStep", menuName = "RPSRPG/Map/NpcRescueStep")]
	public class NpcRescueScrObj : SerializedScriptableObject
	{
		[SerializeField] private TownMenu _targetBuilding;

		public TownMenu TargetBuilding => _targetBuilding;
	}
}
