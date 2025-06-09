using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Kapibara.RPS
{
	[Serializable]
	public class CreditTimeCounter
	{
		[SerializeField] private int _maxCredits = 5;
		[SerializeField] private float _hoursForACredit = 0.25f;
		[SerializeField, ReadOnly] private int _creditsLeft;
		[SerializeField, ReadOnly] private float _timeLeftInSeconds;
		[SerializeField, ReadOnly] private bool _timeIsRunning;
		
		public int MaxCredits
		{
			get => _maxCredits;
		}

		public float HoursForACredit
		{
			get => _hoursForACredit;
		}

		public float HoursForACreditInSeconds
		{
			get => _hoursForACredit * 60 * 60;
		}

		public int CreditsLeft
		{
			get => _creditsLeft;
			set => _creditsLeft = value;
		}
		public bool CreditsAtMax
		{
			get => _creditsLeft == _maxCredits;
		}

		public float TimeLeftInSeconds
		{
			get => _timeLeftInSeconds;
			set => _timeLeftInSeconds = value;
		}

		public bool TimeIsRunning
		{
			get => _timeIsRunning;
			set => _timeIsRunning = value;
		}
	}
}
