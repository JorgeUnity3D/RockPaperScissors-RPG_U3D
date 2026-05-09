using System;
using UnityEngine;

namespace Kapibara.RPS
{
	/// <summary>
	/// Un paso dentro de un MapLevel. El tipo determina qué campo de datos usar.
	/// Combat y Boss usan _combatData; Treasure usa _treasureData; NpcRescue usa _npcRescueData.
	/// </summary>
	[Serializable]
	public class MapStep
	{
		[SerializeField] private MapStepType       _type;
		[SerializeField] private CombatStepScrObj  _combatData;
		[SerializeField] private TreasureStepScrObj _treasureData;
		[SerializeField] private NpcRescueScrObj   _npcRescueData;

		public MapStepType        Type           => _type;
		public CombatStepScrObj   CombatData     => _combatData;
		public TreasureStepScrObj TreasureData   => _treasureData;
		public NpcRescueScrObj    NpcRescueData  => _npcRescueData;
	}
}
