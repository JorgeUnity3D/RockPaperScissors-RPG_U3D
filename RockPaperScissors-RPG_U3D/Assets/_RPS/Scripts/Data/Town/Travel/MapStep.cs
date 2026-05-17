using System;
using System.Collections.Generic;
using UnityEngine;

namespace Kapibara.RPS
{
	[Serializable]
	public class MapStep
	{
		[SerializeField] private MapStepType  _type;
		[SerializeField] private EnemyScrObj  _enemy;             // Combat / Boss
		[SerializeField] private int          _goldAmount;        // Treasure
		[SerializeField] private Sprite       _treasureSprite;    // Treasure
		[SerializeField] private TownMenu     _targetBuilding;    // NpcRescue
		[SerializeField] private Sprite       _npcSprite;         // NpcRescue
		[SerializeField] private List<string> _npcDialogueLines;  // NpcRescue

		public MapStepType  Type             => _type;
		public EnemyScrObj  Enemy            => _enemy;
		public int          GoldAmount       => _goldAmount;
		public Sprite       TreasureSprite   => _treasureSprite;
		public TownMenu     TargetBuilding   => _targetBuilding;
		public Sprite       NPCSprite        => _npcSprite;
		public List<string> NPCDialogueLines => _npcDialogueLines;
	}
}
