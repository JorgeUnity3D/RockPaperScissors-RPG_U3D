using System;
using UnityEngine;

namespace Kapibara.RPS
{
	/// <summary>
	/// Datos de configuración de una quest de la Biblioteca.
	/// TargetKills y RewardAmount se definen en el ScriptableObject.
	/// El progreso (kills actuales) vive en GameContext.LibraryQuests como List&lt;LibraryQuestProgress&gt;.
	/// </summary>
	[Serializable]
	public class LibraryQuestData
	{
		[SerializeField] private EnemyScrObj _targetEnemy;
		[SerializeField] private int _targetKills;
		[SerializeField] private Stats _rewardStat;
		[SerializeField] private int _rewardAmount;

		public EnemyScrObj TargetEnemy => _targetEnemy;
		public int TargetKills         => _targetKills;
		public Stats RewardStat        => _rewardStat;
		public int RewardAmount        => _rewardAmount;
	}
}
