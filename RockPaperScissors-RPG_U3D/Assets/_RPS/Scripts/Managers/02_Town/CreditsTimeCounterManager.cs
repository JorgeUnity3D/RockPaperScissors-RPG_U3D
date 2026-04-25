using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS
{
	/// <summary>
	/// Manager del contador de créditos por tiempo. Actualiza el contador en Update y expone métodos para consumir y ganar créditos.
	/// </summary>
	public class CreditsTimeCounterManager : BaseManager
	{
		[Header("DATA")]
		[SerializeField] private CreditsTimeCounterScrObj _creditsTimeCounterScrObj;
		[Header("DEBUG")]
		[SerializeField, ReadOnly] private CreditTimeCounter _creditTimeCounter;
		[SerializeField, ReadOnly] private CreditsTimeCounterUIController _creditsTimeCounterUIController;

		#region UNITY_LIFECYCLE
		
		private void Update()
		{
			if (_creditTimeCounter.TimeIsRunning)
			{
				if (_creditTimeCounter.TimeLeftInSeconds > 0)
				{
					_creditTimeCounter.TimeLeftInSeconds -= Time.deltaTime;
					//AppEvents.OnTimeUpdated?.Invoke(_creditTimeCounter.TimeLeftInSeconds);
				}
				else
				{
					_creditTimeCounter.TimeIsRunning = false;
					_creditTimeCounter.CreditsLeft++;
					//AppEvents.OnCreditsUpdated?.Invoke(_creditsLeft);
				}
			}
		}

		#endregion

		#region SETUP

		public override void SetUp()
		{
			Debug.Log($"[CreditsTimeCounterManager] SetUp() -> ");
			_creditsTimeCounterUIController = ServiceLocator.Instance.GetService<UIService>().GetController<CreditsTimeCounterUIController>();			
			_creditTimeCounter = _creditsTimeCounterScrObj.Data;
			_creditsTimeCounterUIController.SetData(_creditTimeCounter);
			StartTimeCounter();
			//AppEvents.OnCreditsUpdated.Invoke(_creditsLeft);	
		}

		protected override void Subscribe()
		{
			Debug.Log($"[CreditsTimeCounterManager] Subscribe() -> ");
			AppEvents.OnEarnCredit += EarnCredit;
		}

		protected override void UnSubscribe()
		{
			Debug.Log($"[CreditsTimeCounterManager] UnSubscribe() -> ");
			AppEvents.OnEarnCredit -= EarnCredit;
		}

		#endregion

		#region CONTROL

		public int CreditsLeft => _creditTimeCounter.CreditsLeft;

		/// <summary>Inicia el contador si no está en marcha; si ya estaba, no lo reinicia.</summary>
		public void StartTimeCounter()
		{
			Debug.Log($"[CreditsTimeCounterManager] StartTimeCounter() -> ");

			if (!_creditTimeCounter.TimeIsRunning)
			{
				_creditTimeCounter.TimeLeftInSeconds = _creditTimeCounter.HoursForACreditInSeconds;
			}
			_creditTimeCounter.TimeIsRunning = true;
		}

		/// <summary>Resta un crédito al jugador sin bajar de cero.</summary>
		[ContextMenu("UseCredit")]
		public void UseCredit()
		{
			Debug.Log($"[CreditsTimeCounterManager] UseCredit() -> ");
			_creditTimeCounter.CreditsLeft = Mathf.Max(0, _creditTimeCounter.CreditsLeft - 1);
			//AppEvents.OnCreditsUpdated?.Invoke(_creditTimeCounter.CreditsLeft);
		}

		/// <summary>Añade un crédito sin superar el máximo permitido.</summary>
		[ContextMenu("EarnCredit")]
		public void EarnCredit()
		{
			Debug.Log($"[CreditsTimeCounterManager] EarnCredit() -> ");
			_creditTimeCounter.CreditsLeft = Mathf.Min(_creditTimeCounter.MaxCredits, _creditTimeCounter.CreditsLeft + 1);
			//AppEvents.OnCreditsUpdated?.Invoke(_creditTimeCounter.CreditsLeft);
		}

		#endregion
	}
}
