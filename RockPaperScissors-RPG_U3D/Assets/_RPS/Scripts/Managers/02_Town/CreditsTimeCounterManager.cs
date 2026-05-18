using System.Collections;
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

		#region SETUP

		public override void SetUp()
		{
			Debug.Log($"[CreditsTimeCounterManager] SetUp() -> ");
			_creditsTimeCounterUIController = ServiceLocator.Instance.GetService<UIService>().GetController<CreditsTimeCounterUIController>();
			_creditTimeCounter = _creditsTimeCounterScrObj.Data;
		}

		public override void Initialize()
		{
			Debug.Log($"[CreditsTimeCounterManager] Initialize() -> ");
			_creditsTimeCounterUIController.SetData(_creditTimeCounter);
			StartTimeCounter();
			StartCoroutine(TickCoroutine());
		}

		protected override void Subscribe()
		{
			Debug.Log($"[CreditsTimeCounterManager] Subscribe() -> ");
			AppEvents.OnEarnCredit      += EarnCredit;
			AppEvents.OnTravelRequested += OnTravelRequested;
		}

		protected override void UnSubscribe()
		{
			Debug.Log($"[CreditsTimeCounterManager] UnSubscribe() -> ");
			AppEvents.OnEarnCredit      -= EarnCredit;
			AppEvents.OnTravelRequested -= OnTravelRequested;
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
			AppEvents.OnCreditsUpdated?.Invoke(_creditTimeCounter.CreditsLeft);
			StartTimeCounter();
		}

		private void OnTravelRequested(MapLevel level)
		{
			if (_creditTimeCounter.CreditsLeft <= 0)
			{
				Debug.LogWarning($"[CreditsTimeCounterManager] OnTravelRequested() -> No credits left.");
				return;
			}
			UseCredit();
			AppEvents.OnTravelConfirmed?.Invoke(level);
		}

		/// <summary>Añade un crédito sin superar el máximo; para el timer si se alcanza el máximo.</summary>
		[ContextMenu("EarnCredit")]
		public void EarnCredit()
		{
			Debug.Log($"[CreditsTimeCounterManager] EarnCredit() -> ");
			_creditTimeCounter.CreditsLeft = Mathf.Min(_creditTimeCounter.MaxCredits, _creditTimeCounter.CreditsLeft + 1);
			AppEvents.OnCreditsUpdated?.Invoke(_creditTimeCounter.CreditsLeft);
			if (_creditTimeCounter.CreditsAtMax)
				_creditTimeCounter.TimeIsRunning = false;
		}

		#endregion

		#region COROUTINES

		private IEnumerator TickCoroutine()
		{
			while (true)
			{
				yield return null;

				if (!_creditTimeCounter.TimeIsRunning) continue;

				if (_creditTimeCounter.TimeLeftInSeconds > 0)
				{
					_creditTimeCounter.TimeLeftInSeconds -= Time.deltaTime;
					AppEvents.OnTimeUpdated?.Invoke(_creditTimeCounter.TimeLeftInSeconds);
				}
				else
				{
					_creditTimeCounter.CreditsLeft++;
					AppEvents.OnCreditsUpdated?.Invoke(_creditTimeCounter.CreditsLeft);
					if (_creditTimeCounter.CreditsAtMax)
						_creditTimeCounter.TimeIsRunning = false;
					else
						_creditTimeCounter.TimeLeftInSeconds = _creditTimeCounter.HoursForACreditInSeconds;
				}
			}
		}

		#endregion
	}
}
