using Kapibara.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Kapibara.RPS
{
	/// <summary>
	/// Vista del menú de pausa. El SettingsButton es siempre visible; al pulsarlo muestra el overlay.
	/// Resume cierra el overlay. Options es placeholder. Exit abandona el nivel como derrota.
	/// </summary>
	public class PauseMenuUIController : UIController
	{
		[Header("Buttons")]
		[SerializeField] private Button _settingsButton;
		[SerializeField] private Button _resumeButton;
		[SerializeField] private Button _optionsButton;
		[SerializeField] private Button _exitButton;

		[Header("Overlay")]
		[SerializeField] private GameObject _panelBackground;
		[SerializeField] private GameObject _pausePanel;

		#region UNITY LIFECYCLE

		private void Awake()
		{
			SetUp();
		}

		#endregion

		#region SETUP

		public override void SetUp()
		{
			SetOverlayActive(false);
			_settingsButton.onClick.AddListener(() => SetOverlayActive(true));
			_resumeButton.onClick.AddListener(() => SetOverlayActive(false));
			_optionsButton.onClick.AddListener(OnOptionsPressed);
			_exitButton.onClick.AddListener(OnExitPressed);
		}

		#endregion

		#region CALLBACKS

		private void OnOptionsPressed()
		{
			AppEvents.OnOptionsRequested?.Invoke();
		}

		private void OnExitPressed()
		{
			SetOverlayActive(false);
			AppEvents.OnCombatExited?.Invoke();
		}

		#endregion

		#region HELPERS

		private void SetOverlayActive(bool active)
		{
			_panelBackground.SetActive(active);
			_pausePanel.SetActive(active);
		}

		#endregion
	}
}
