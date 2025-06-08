using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Kapibara.RPS
{
	[Serializable]
	public class MapLevel
	{
		[SerializeField] private int _level;
		[SerializeField] private string _levelName;
		[SerializeField] private int _steps = 10;
		[SerializeField] private int _reward = 10;
		[SerializeField] private Sprite _levelIcon;
		[SerializeField] private Sprite _levelPortrait;
		[SerializeField] private List<EnemyDataObject> _levelEnemies;
		[SerializeField] private bool _isAvailable;
		[SerializeField] private bool _isSpecialLevel;

		public int Level
		{
			get => _level;
		}

		public string LevelName
		{
			get => _levelName;
		}

		public int Steps
		{
			get => _steps;
		}

		public int Reward
		{
			get => _reward;
		}

		public Sprite LevelIcon
		{
			get => _levelIcon;
		}

		public Sprite LevelPortrait
		{
			get => _levelPortrait;
		}

		public List<EnemyDataObject> LevelEnemies
		{
			get => _levelEnemies;
		}

		public bool IsAvailable
		{
			get => _isAvailable;
		}

		public bool IsSpecialLevel
		{
			get => _isSpecialLevel;
		}

	}
}