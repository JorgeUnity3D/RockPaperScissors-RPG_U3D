using Kapibara.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Kapibara.RPS
{
	/// <summary>
	/// Vista del resultado de combate. Pasiva: muestra victoria/derrota, oro y EXP ganados.
	/// El botón Continuar dispara OnCombatFinished para que GameManager cargue Town.
	/// </summary>
	public class CombatResultUIController : UIController
	{
		[Header("Result")]
		[SerializeField] private TextMeshProUGUI _resultLabel;
		[SerializeField] private TextMeshProUGUI _goldLabel;
		[SerializeField] private TextMeshProUGUI _expLabel;

		[Header("Actions")]
		[SerializeField] private Button _continueButton;

		private bool _playerWins;

		#region UNITY LIFECYCLE

		private void Awake()
		{
			SetUp();
		}

		#endregion

		#region SETUP

		public override void SetUp()
		{
			HideCanvas(0);
			_continueButton.onClick.AddListener(OnContinuePressed);
		}

		public void SetData(bool playerWins, int goldEarned, int expEarned)
		{
			_playerWins       = playerWins;
			_resultLabel.text = playerWins ? "Victoria" : "Derrota";
			_goldLabel.text   = playerWins ? $"+{goldEarned} oro" : string.Empty;
			_expLabel.text    = expEarned > 0 ? $"+{expEarned} EXP entrenamiento" : string.Empty;
		}

		#endregion

		#region CALLBACKS

		private void OnContinuePressed()
		{
			AppEvents.OnCombatFinished?.Invoke(_playerWins);
		}

		#endregion
	}
}
