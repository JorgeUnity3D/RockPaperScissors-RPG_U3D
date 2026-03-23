using System;
using UnityEngine;

namespace Kapibara.RPS
{
	/// <summary>
	/// Datos de configuración de una quest de la Biblioteca.
	/// TargetKills y RewardAmount se definen en el ScriptableObject.
	/// El progreso (kills actuales) vive en Player._libraryKillCounts, indexado por posición en la lista plana de quests.
	/// </summary>
	[Serializable]
	public class LibraryQuestData
	{
		[SerializeField] private EnemyId _enemyId;
		[SerializeField] private int _targetKills;
		[SerializeField] private Stats _rewardStat;
		[SerializeField] private int _rewardAmount;

		public EnemyId EnemyId   => _enemyId;
		public int TargetKills   => _targetKills;
		public Stats RewardStat  => _rewardStat;
		public int RewardAmount  => _rewardAmount;
	}
}
