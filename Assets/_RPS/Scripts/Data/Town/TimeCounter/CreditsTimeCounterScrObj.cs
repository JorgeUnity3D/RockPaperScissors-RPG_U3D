using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS
{
	[CreateAssetMenu(fileName = "CreditsTimeCounter", menuName = "RPSRPG/CreditsTimeCounter")]
	public class CreditsTimeCounterScrObj : SerializedScriptableObject
	{
		[HideLabel, SerializeField] private CreditTimeCounter _data;
		public CreditTimeCounter Data
		{
			get => _data;
		}
	}
}