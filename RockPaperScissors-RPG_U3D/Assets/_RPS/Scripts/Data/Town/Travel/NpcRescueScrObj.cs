using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS
{
	[CreateAssetMenu(fileName = "NpcRescueStep", menuName = "RPSRPG/Levels/Steps/NpcRescue")]
	public class NpcRescueScrObj : SerializedScriptableObject
	{
		[SerializeField] private TownMenu _targetBuilding;

		public TownMenu TargetBuilding => _targetBuilding;
	}
}
