using Kapibara.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Kapibara.RPS
{
	public class StablesUIController : UIController
	{
		[Header("UI")]
		[SerializeField] private Button _watchAdButton;
		[SerializeField] private Button _buyGameButton;

		#region UNITY_LIFECYCLE

		private void Awake()
		{
			SetUp();
		}

		#endregion

		#region SETUP

		public override void SetUp()
		{
			HideCanvas(0);
			_watchAdButton.onClick.AddListener(OnWatchAdClicked);
			_buyGameButton.onClick.AddListener(OnBuyGameClicked);
		}

		#endregion

		#region CONTROL

		private void OnWatchAdClicked()
		{
			AppEvents.OnWatchAd?.Invoke();
		}

		private void OnBuyGameClicked()
		{
			AppEvents.OnBuyGame?.Invoke();
		}

		#endregion
	}
}
