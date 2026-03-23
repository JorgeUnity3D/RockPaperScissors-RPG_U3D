using Kapibara.UI;
using UnityEngine;
using UnityEngine.Events;
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
		}

		#endregion

		#region CONTROL

		/// <summary>
		/// Wira los botones de monetización. Llamar desde StablesManager.Initialize().
		/// </summary>
		public void SetData(UnityAction onWatchAd, UnityAction onBuyGame)
		{
			_watchAdButton.onClick.RemoveAllListeners();
			_watchAdButton.onClick.AddListener(onWatchAd);

			_buyGameButton.onClick.RemoveAllListeners();
			_buyGameButton.onClick.AddListener(onBuyGame);
		}

		#endregion
	}
}
