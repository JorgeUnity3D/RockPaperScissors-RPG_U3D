using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS
{
	[CreateAssetMenu(fileName = "StoneSmithyData", menuName = "RPSRPG/Town/StoneSmithy")]
	public class StoneSmithyScrObj : SerializedScriptableObject
	{
		[HideLabel, SerializeField] private StoneSmithyData _data;
		public StoneSmithyData Data => _data;
	}
}
