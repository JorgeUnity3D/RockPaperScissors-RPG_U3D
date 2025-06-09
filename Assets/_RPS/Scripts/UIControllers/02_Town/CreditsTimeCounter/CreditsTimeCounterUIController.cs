using Kapibara.UI;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Kapibara.RPS
{
	public class CreditsTimeCounterUIController : UIController
	{
		[Header("UI")]
		[SerializeField] private List<Image> _credits;
		[SerializeField] private Sprite _usedCredit;
		[SerializeField] private Sprite _unusedCredit;
		[SerializeField] private GameObject _timeCounterHolder;
		[SerializeField] private TextMeshProUGUI _timeCounterText;
		[Header("DEBUG")]
		[SerializeField, ReadOnly] private CreditTimeCounter _creditTimeCounter;


		#region UNITY_LIFECYCLE

		private void Awake()
		{
			SetUp();
		}

		private void Update()
		{
			if (_creditTimeCounter == null)
			{
				return;
			}
			SetCreditsUI(_creditTimeCounter.CreditsLeft);
			SetTimeCounterUI(_creditTimeCounter.TimeLeftInSeconds);
		}

		#endregion

		#region SETUP

		public override void SetUp()
		{
			Debug.Log($"[CreditsTimeCounterUIController] SetUp() -> ");
			//AppEvents.OnCreditsUpdated += SetCreditsUI;
			//AppEvents.OnTimeUpdated += SetTimeCounterUI;
		}

		public void SetData(CreditTimeCounter creditTimeCounter)
		{
			Debug.Log($"[CreditsTimeCounterUIController] SetData() -> ");
			_creditTimeCounter = creditTimeCounter;
		}

		#endregion

		#region CONTROL
		private void SetCreditsUI(int creditsLeft)
		{
			//Debug.Log($"[CreditsTimeCounterUIController] SetCreditsUI() -> ");			
			for (int i = 0; i < _credits.Count; i++)
			{
				_credits[i].sprite = creditsLeft == 0 ? 
										_usedCredit : i < creditsLeft ? 
											_unusedCredit : _usedCredit;
			}
			//if (!Manager.CreditsAtMax)
			//{
			//	Manager.StartTimeCounter();
			//}

			//timeCounterText.transform.parent.gameObject.SetActive(!Manager.CreditsAtMax);
		}

		private void SetTimeCounterUI(float timeToDisplay)
		{
			//Debug.Log($"[CreditsTimeCounterUIController] SetTimeCounterUI() -> ");
			TimeSpan t = TimeSpan.FromSeconds(timeToDisplay);
			_timeCounterText.text = string.Format("{0:00}:{1:00}:{2:00}", t.Hours, t.Minutes, t.Seconds);
		}

		#endregion
	}
}