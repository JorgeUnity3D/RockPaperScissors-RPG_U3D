using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS
{
	[CreateAssetMenu(fileName = "Player", menuName = "RPSRPG/Player")]
	public class PlayerScrObj : SerializedScriptableObject
	{
		[HideLabel, SerializeField] private Player _data;

		public Player Data => _data;
	}
}
