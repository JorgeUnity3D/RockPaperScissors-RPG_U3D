using Sirenix.OdinInspector;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

namespace Kapibara.RPS
{
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
			Debug.Log($"[CreditsTimeCounterManager] Subscribe() -> Nothing to subscribe!");
		}

		protected override void UnSubscribe()
		{
			Debug.Log($"[CreditsTimeCounterManager] UnSubscribe() -> Nothing to unsubscribe!");
		}

		#endregion

		#region CONTROL

		public void StartTimeCounter()
		{
			Debug.Log($"[CreditsTimeCounterManager] StartTimeCounter() -> ");

			if (!_creditTimeCounter.TimeIsRunning)
			{
				_creditTimeCounter.TimeLeftInSeconds = _creditTimeCounter.HoursForACreditInSeconds;
			}
			_creditTimeCounter.TimeIsRunning = true;
		}

		[ContextMenu("UseCredit")]
		public void UseCredit()
		{
			Debug.Log($"[CreditsTimeCounterManager] UseCredit() -> ");
			_creditTimeCounter.CreditsLeft = Mathf.Max(0, _creditTimeCounter.CreditsLeft - 1);
			//AppEvents.OnCreditsUpdated?.Invoke(_creditTimeCounter.CreditsLeft);
		}

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
