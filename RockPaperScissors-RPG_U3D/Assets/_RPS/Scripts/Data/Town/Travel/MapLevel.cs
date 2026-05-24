using System;
using System.Collections.Generic;
using UnityEngine;

namespace Kapibara.RPS
{
	[Serializable]
	public class MapLevel
	{
		[SerializeField] private int              _level;
		[SerializeField] private string           _levelName;
		[SerializeField] private Sprite           _levelIcon;
		[SerializeField] private Sprite           _levelPortrait;
		[SerializeField] private bool             _isAvailable;

		[Header("Enemies")]
		[SerializeField] private List<EnemyScrObj> _possibleEnemies;
		[SerializeField] private EnemyScrObj       _boss;

		[Header("Treasure Step")]
		[SerializeField] private int    _treasureGoldAmount;
		[SerializeField] private Sprite _treasureSprite;

		[Header("Boss Historia")]
		[SerializeField] private ComicStoryScrObj _bossStory;

		[Header("NPC Step")]
		[SerializeField] private TownMenu     _targetBuilding;
		[SerializeField] private List<string> _npcDialogueLines;

		public int               Level             => _level;
		public string            LevelName         => _levelName;
		public Sprite            LevelIcon         => _levelIcon;
		public Sprite            LevelPortrait     => _levelPortrait;
		public bool              IsAvailable       => _isAvailable;
		public List<EnemyScrObj> PossibleEnemies   => _possibleEnemies;
		public EnemyScrObj       Boss              => _boss;
		public int               TreasureGoldAmount => _treasureGoldAmount;
		public Sprite            TreasureSprite    => _treasureSprite;
		public ComicStoryScrObj  BossStory         => _bossStory;
		public TownMenu          TargetBuilding    => _targetBuilding;
		public List<string>      NpcDialogueLines  => _npcDialogueLines;

		private bool _isCompleted;
		public bool  IsCompleted => _isCompleted;
		public void  SetCompleted() => _isCompleted = true;
	}
}
