using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Kapibara.RPS
{
	/// <summary>
	/// Estado runtime de una quest de Biblioteca. Serializable en GameContext.
	/// Se crea una vez (materialización) y vive en el save de la partida.
	/// </summary>
	[Serializable]
	public class LibraryQuestProgress
	{
		[SerializeField] private string _enemyId;
		[SerializeField] private string _enemyDisplayName;
		[SerializeField] private int    _currentKills;
		[SerializeField] private int    _targetKills;
		[SerializeField] private Stats  _rewardStat;
		[SerializeField] private int    _rewardAmount;
		[SerializeField] private bool   _isCompleted;
		[SerializeField] private int    _pageIndex;

		public string EnemyId          => _enemyId;
		public string EnemyDisplayName => _enemyDisplayName;
		public int TargetKills         => _targetKills;
		public Stats RewardStat        => _rewardStat;
		public int RewardAmount        => _rewardAmount;
		public int PageIndex           => _pageIndex;

		public int CurrentKills
		{
			get => _currentKills;
			set => _currentKills = value;
		}

		public bool IsCompleted
		{
			get => _isCompleted;
			set => _isCompleted = value;
		}

		public LibraryQuestProgress(string enemyId, string enemyDisplayName, int targetKills, Stats rewardStat, int rewardAmount, int pageIndex)
		{
			_enemyId          = enemyId;
			_enemyDisplayName = enemyDisplayName;
			_currentKills     = 0;
			_targetKills      = targetKills;
			_rewardStat       = rewardStat;
			_rewardAmount     = rewardAmount;
			_isCompleted      = false;
			_pageIndex        = pageIndex;
		}

		[JsonConstructor]
		public LibraryQuestProgress(string enemyId, string enemyDisplayName, int currentKills, int targetKills, Stats rewardStat, int rewardAmount, bool isCompleted, int pageIndex)
		{
			_enemyId          = enemyId;
			_enemyDisplayName = enemyDisplayName;
			_currentKills     = currentKills;
			_targetKills      = targetKills;
			_rewardStat       = rewardStat;
			_rewardAmount     = rewardAmount;
			_isCompleted      = isCompleted;
			_pageIndex        = pageIndex;
		}
	}
}
