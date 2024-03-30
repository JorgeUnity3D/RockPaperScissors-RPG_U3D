using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS
{
	[CreateAssetMenu(fileName = "TrainingView", menuName = "RPSRPG/TrainingView")]
	public class TrainingViewScrObj : SerializedScriptableObject
	{
		[HideReferenceObjectPicker, HideLabel, SerializeField] private List<TrainingView> _data;
		public List<TrainingView> Data => _data;
	}
}