using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS
{
	/// <summary>
	/// Gestiona los créditos de viaje. El estado (créditos y timer) vive en GameContext (JSON persistente).
	/// El timer usa un Unix timestamp de expiración, lo que hace que cuente durante combat y entre sesiones de app.
	/// </summary>
	public class CreditsTimeCounterManager : BaseManager
	{
		[Header("DATA")]
		[SerializeField] private CreditsTimeCounterScrObj _creditsTimeCounterScrObj;
		[Header("DEBUG")]
		[SerializeField, ReadOnly] private CreditsTimeCounterUIController _creditsTimeCounterUIController;

		private float     _displayTimeLeft;
		private Coroutine _tickCoroutine;

		#region SETUP

		public override void SetUp()
		{
			Debug.Log($"[CreditsTimeCounterManager] SetUp() -> ");
			_creditsTimeCounterUIController = ServiceLocator.Instance.GetService<UIService>().GetController<CreditsTimeCounterUIController>();
		}

		public override void Initialize()
		{
			Debug.Log($"[CreditsTimeCounterManager] Initialize() -> ");
			ApplyElapsedTime();
			RefreshUI();
			if (HasActiveTimer())
				StartTick();
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

		/// <summary>Calcula cuántos créditos se generaron offline/durante combat desde el último timestamp guardado.</summary>
		private void ApplyElapsedTime()
		{
			GameContext ctx = AppContext.GameContext;
			if (ctx == null) return;

			string expiresStr = ctx.CreditTimerExpiresUnix;
			if (expiresStr == "0") { _displayTimeLeft = 0f; return; }

			long expiresUnix   = long.Parse(expiresStr);
			long nowUnix       = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
			long secsPerCredit = (long)_creditsTimeCounterScrObj.Data.HoursForACreditInSeconds;
			int  maxCredits    = _creditsTimeCounterScrObj.Data.MaxCredits;

			while (nowUnix >= expiresUnix && ctx.CreditsLeft < maxCredits)
			{
				ctx.CreditsLeft++;
				expiresUnix += secsPerCredit;
			}

			if (ctx.CreditsLeft >= maxCredits)
			{
				ctx.CreditTimerExpiresUnix = "0";
				_displayTimeLeft = 0f;
			}
			else
			{
				ctx.CreditTimerExpiresUnix = expiresUnix.ToString();
				_displayTimeLeft = expiresUnix - nowUnix;
			}

			AppEvents.OnGameContextUpdated?.Invoke();
		}

		private void RefreshUI()
		{
			GameContext ctx = AppContext.GameContext;
			if (ctx == null) return;

			int maxCredits = _creditsTimeCounterScrObj.Data.MaxCredits;
			_creditsTimeCounterUIController.SetData(ctx.CreditsLeft, maxCredits, _displayTimeLeft);
			AppEvents.OnCreditsUpdated?.Invoke(ctx.CreditsLeft);
		}

		private bool HasActiveTimer()
		{
			GameContext ctx = AppContext.GameContext;
			return ctx != null && ctx.CreditTimerExpiresUnix != "0";
		}

		[Button("UseCredit")]
		public void UseCredit()
		{
			Debug.Log($"[CreditsTimeCounterManager] UseCredit() -> ");
			GameContext ctx = AppContext.GameContext;
			if (ctx == null) return;

			ctx.CreditsLeft = Mathf.Max(0, ctx.CreditsLeft - 1);
			AppEvents.OnCreditsUpdated?.Invoke(ctx.CreditsLeft);

			if (ctx.CreditTimerExpiresUnix == "0")
			{
				long secsPerCredit = (long)_creditsTimeCounterScrObj.Data.HoursForACreditInSeconds;
				ctx.CreditTimerExpiresUnix = (DateTimeOffset.UtcNow.ToUnixTimeSeconds() + secsPerCredit).ToString();
				_displayTimeLeft = secsPerCredit;
				StartTick();
			}

			AppEvents.OnGameContextUpdated?.Invoke();
		}

		[Button("EarnCredit")]
		private void EarnCredit()
		{
			Debug.Log($"[CreditsTimeCounterManager] EarnCredit() -> ");
			GameContext ctx = AppContext.GameContext;
			if (ctx == null) return;

			int maxCredits = _creditsTimeCounterScrObj.Data.MaxCredits;
			ctx.CreditsLeft = Mathf.Min(maxCredits, ctx.CreditsLeft + 1);
			AppEvents.OnCreditsUpdated?.Invoke(ctx.CreditsLeft);

			if (ctx.CreditsLeft >= maxCredits)
			{
				ctx.CreditTimerExpiresUnix = "0";
				_displayTimeLeft = 0f;
			}
			else
			{
				long secsPerCredit = (long)_creditsTimeCounterScrObj.Data.HoursForACreditInSeconds;
				ctx.CreditTimerExpiresUnix = (DateTimeOffset.UtcNow.ToUnixTimeSeconds() + secsPerCredit).ToString();
				_displayTimeLeft = secsPerCredit;
			}

			AppEvents.OnGameContextUpdated?.Invoke();
		}

		private void OnTravelRequested(MapLevel level)
		{
			GameContext ctx = AppContext.GameContext;
			if (ctx == null || ctx.CreditsLeft <= 0)
			{
				Debug.LogWarning($"[CreditsTimeCounterManager] OnTravelRequested() -> No credits left.");
				return;
			}
			UseCredit();
			AppEvents.OnTravelConfirmed?.Invoke(level);
		}

		#endregion

		#region COROUTINES

		private void StartTick()
		{
			if (_tickCoroutine != null) return;
			_tickCoroutine = StartCoroutine(TickCoroutine());
		}

		private IEnumerator TickCoroutine()
		{
			while (HasActiveTimer())
			{
				yield return null;
				_displayTimeLeft -= Time.deltaTime;
				AppEvents.OnTimeUpdated?.Invoke(Mathf.Max(0f, _displayTimeLeft));

				if (_displayTimeLeft <= 0f)
					EarnCredit();
			}
			_tickCoroutine = null;
		}

		#endregion
	}
}
