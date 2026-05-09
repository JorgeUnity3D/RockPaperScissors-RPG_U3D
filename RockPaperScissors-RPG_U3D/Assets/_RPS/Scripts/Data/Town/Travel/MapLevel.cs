using System;
using System.Collections.Generic;
using UnityEngine;

namespace Kapibara.RPS
{
	[Serializable]
	public class MapLevel
	{
		[SerializeField] private int           _level;
		[SerializeField] private string        _levelName;
		[SerializeField] private Sprite        _levelIcon;
		[SerializeField] private Sprite        _levelPortrait;
		[SerializeField] private List<MapStep> _steps;
		[SerializeField] private bool          _isAvailable;

		public int           Level         => _level;
		public string        LevelName     => _levelName;
		public Sprite        LevelIcon     => _levelIcon;
		public Sprite        LevelPortrait => _levelPortrait;
		public List<MapStep> Steps         => _steps;
		public bool          IsAvailable   => _isAvailable;
		public int           StepCount     => _steps?.Count ?? 0;
	}
}
