using System;
using UnityEngine;

namespace Kapibara.RPS
{
	[Serializable]
	public class MapLevel
	{
		[SerializeField] private int        _level;
		[SerializeField] private string     _levelName;
		[SerializeField] private int        _steps        = 10;
		[SerializeField] private int        _reward       = 10;
		[SerializeField] private Sprite     _levelIcon;
		[SerializeField] private Sprite     _levelPortrait;
		[SerializeField] private EnemyScrObj _enemy;
		[SerializeField] private bool       _isAvailable;
		[SerializeField] private bool       _isSpecialLevel;

		public int        Level        => _level;
		public string     LevelName    => _levelName;
		public int        Steps        => _steps;
		public int        Reward       => _reward;
		public Sprite     LevelIcon    => _levelIcon;
		public Sprite     LevelPortrait => _levelPortrait;
		public EnemyScrObj Enemy       => _enemy;
		public bool       IsAvailable  => _isAvailable;
		public bool       IsSpecialLevel => _isSpecialLevel;
	}
}
