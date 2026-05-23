using System;
using UnityEngine;

namespace Kapibara.RPS
{
	/// <summary>Solo configuración de diseño. El estado mutable (créditos y timer) vive en GameContext.</summary>
	[Serializable]
	public class CreditTimeCounter
	{
		[SerializeField] private int   _startingCredits = 5;
		[SerializeField] private int   _maxCredits      = 5;
		[SerializeField] private float _hoursForACredit = 0.25f;

		public int   StartingCredits          => _startingCredits;
		public int   MaxCredits               => _maxCredits;
		public float HoursForACredit          => _hoursForACredit;
		public float HoursForACreditInSeconds => _hoursForACredit * 3600f;
	}
}
