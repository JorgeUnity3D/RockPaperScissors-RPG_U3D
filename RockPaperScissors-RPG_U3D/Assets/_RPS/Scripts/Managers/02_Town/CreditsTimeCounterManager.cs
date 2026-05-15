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
			if (!_creditTimeCounter.TimeIsRunning) return;

			if (_creditTimeCounter.TimeLeftInSeconds > 0)
			{
				_creditTimeCounter.TimeLeftInSeconds -= Time.deltaTime;
			}
			else
			{
				_creditTimeCounter.CreditsLeft++;
				if (_creditTimeCounter.CreditsAtMax)
				{
					_creditTimeCounter.TimeIsRunning = false;
				}
				else
				{
					_creditTimeCounter.TimeLeftInSeconds = _creditTimeCounter.HoursForACreditInSeconds;
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

		/// <summary>Inicia el contador solo si los créditos no están al máximo y el timer no estaba ya en marcha.</summary>
		public void StartTimeCounter()
		{
			Debug.Log($"[CreditsTimeCounterManager] StartTimeCounter() -> ");
			if (_creditTimeCounter.CreditsAtMax) return;

			if (!_creditTimeCounter.TimeIsRunning)
			{
				_creditTimeCounter.TimeLeftInSeconds = _creditTimeCounter.HoursForACreditInSeconds;
				_creditTimeCounter.TimeIsRunning     = true;
			}
		}

		/// <summary>Resta un crédito al jugador sin bajar de cero e inicia el timer de recarga.</summary>
		[ContextMenu("UseCredit")]
		public void UseCredit()
		{
			Debug.Log($"[CreditsTimeCounterManager] UseCredit() -> ");
			_creditTimeCounter.CreditsLeft = Mathf.Max(0, _creditTimeCounter.CreditsLeft - 1);
			StartTimeCounter();
		}

		/// <summary>Añade un crédito sin superar el máximo; para el timer si se alcanza el máximo.</summary>
		[ContextMenu("EarnCredit")]
		public void EarnCredit()
		{
			Debug.Log($"[CreditsTimeCounterManager] EarnCredit() -> ");
			_creditTimeCounter.CreditsLeft = Mathf.Min(_creditTimeCounter.MaxCredits, _creditTimeCounter.CreditsLeft + 1);
			if (_creditTimeCounter.CreditsAtMax)
				_creditTimeCounter.TimeIsRunning = false;
		}

		#endregion
	}
}
