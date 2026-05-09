using System;
using UnityEngine;

namespace Kapibara.RPS
{
	[Serializable]
	public class MapStep
	{
		[SerializeField] private MapStepType _type;
		[SerializeField] private EnemyScrObj _enemy;           // Combat / Boss
		[SerializeField] private int         _goldAmount;      // Treasure
		[SerializeField] private TownMenu    _targetBuilding;  // NpcRescue

		public MapStepType Type            => _type;
		public EnemyScrObj Enemy           => _enemy;
		public int         GoldAmount      => _goldAmount;
		public TownMenu    TargetBuilding  => _targetBuilding;
	}
}
