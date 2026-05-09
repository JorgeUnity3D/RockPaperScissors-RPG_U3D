using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS
{
	[CreateAssetMenu(fileName = "Enemy", menuName = "RPSRPG/Enemies/Enemy")]
	public class EnemyScrObj : SerializedScriptableObject
	{
		[HideLabel, SerializeField] private EnemyData _data;

		public EnemyData Data => _data;
	}
}
