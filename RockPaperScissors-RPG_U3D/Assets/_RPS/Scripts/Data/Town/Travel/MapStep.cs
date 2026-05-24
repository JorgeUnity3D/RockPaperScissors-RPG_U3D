using System.Collections.Generic;
using UnityEngine;

namespace Kapibara.RPS
{
	[System.Serializable]
	public class MapStep
	{
		[SerializeField] private MapStepType  _type;
		[SerializeField] private EnemyScrObj  _enemy;             // Combat / Boss
		[SerializeField] private int          _goldAmount;        // Treasure
		[SerializeField] private Sprite       _treasureSprite;    // Treasure
		[SerializeField] private TownMenu     _targetBuilding;    // NpcRescue
		[SerializeField] private List<string> _npcDialogueLines;  // NpcRescue

		public MapStepType  Type             => _type;
		public EnemyScrObj  Enemy            => _enemy;
		public int          GoldAmount       => _goldAmount;
		public Sprite       TreasureSprite   => _treasureSprite;
		public TownMenu     TargetBuilding   => _targetBuilding;
		public List<string> NPCDialogueLines => _npcDialogueLines;

		public MapStep(MapStepType type, EnemyScrObj enemy)
		{
			_type  = type;
			_enemy = enemy;
		}

		public MapStep(int goldAmount, Sprite treasureSprite)
		{
			_type           = MapStepType.TREASURE;
			_goldAmount     = goldAmount;
			_treasureSprite = treasureSprite;
		}

		public MapStep(TownMenu targetBuilding, List<string> npcDialogueLines)
		{
			_type             = MapStepType.NPC_RESCUE;
			_targetBuilding   = targetBuilding;
			_npcDialogueLines = npcDialogueLines;
		}

		public MapStep()
		{
			_type = MapStepType.SURPRISE_BOX;
		}
	}
}
