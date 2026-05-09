using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS
{
	[CreateAssetMenu(fileName = "CombatStep", menuName = "RPSRPG/Map/CombatStep")]
	public class CombatStepScrObj : SerializedScriptableObject
	{
		[SerializeField] private EnemyScrObj _enemy;

		public EnemyScrObj Enemy => _enemy;
	}
}
