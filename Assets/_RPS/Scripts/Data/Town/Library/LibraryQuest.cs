using Kapibara.Util.NotificableFields;
using UnityEngine;

namespace Kapibara.RPS
{
	public class LibraryQuest
	{
		[SerializeField] private EnemyId _enemyId;
		[SerializeField] private int _targetKills;
		private int _currentKills;
		[SerializeField] private NAttribute _rewardAttribute;
	}
}
