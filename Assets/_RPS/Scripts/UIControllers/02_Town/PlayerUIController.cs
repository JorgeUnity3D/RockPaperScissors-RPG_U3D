using Kapibara.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Kapibara.RPS
{
	public class PlayerUIController : UIController
	{
		[Header("UI")]
		[SerializeField] private Button _settingsButton;
		[SerializeField] private TextMeshProUGUI _playerGoldText;

		#region UNITY_LIFECYCLE

		private void Awake()
		{
			SetUp();
		}

		#endregion

		#region SETUP

		public override void SetUp()
		{
			Debug.Log($"[PlayerUIController] SetUp() -> ");
			
		}

		public void SetData(Player player)
		{
			Debug.Log($"[PlayerUIController] SetData() -> ");
		}

		#endregion

		#region CONTROL

		public void UpdatePlayerGold(int gold)
		{
			Debug.Log($"[PlayerUIController] UpdatePlayerGold() -> ");
			_playerGoldText.text = gold.ToString();
		}

		#endregion

	}
}
