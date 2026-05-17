using DG.Tweening;
using Kapibara.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Kapibara.RPS
{
	/// <summary>
	/// Vista del step de Tesoro. Pasiva: renderiza datos recibidos de TreasureStepManager y emite
	/// AppEvents al pulsar los botones RPS.
	/// </summary>
	public class TreasureUIController : UIController
	{
		[Header("Player Bars")]
		[SerializeField] private Image           _playerHPFill;
		[SerializeField] private Image           _playerHPGhost;
		[SerializeField] private TextMeshProUGUI _playerHPText;

		[Header("Treasure Zone")]
		[SerializeField] private Image           _treasureImage;
		[SerializeField] private TextMeshProUGUI _goldText;

		[Header("Center")]
		[SerializeField] private Button          _settingsButton;

		[Header("Actions")]
		[SerializeField] private Button          _rockButton;
		[SerializeField] private Button          _paperButton;
		[SerializeField] private Button          _scissorButton;

		private int _playerMaxHP;

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

			_rockButton.onClick.AddListener(    () => AppEvents.OnTreasureActionSelected?.Invoke(Actions.ROCK));
			_paperButton.onClick.AddListener(   () => AppEvents.OnTreasureActionSelected?.Invoke(Actions.PAPER));
			_scissorButton.onClick.AddListener( () => AppEvents.OnTreasureActionSelected?.Invoke(Actions.SCISSOR));
		}

		public void SetData(Sprite treasureSprite, int goldAmount, int playerMaxHP, int playerCurrentHP)
		{
			_treasureImage.sprite = treasureSprite;
			_goldText.text        = $"+{goldAmount} oro";

			_playerMaxHP             = playerMaxHP;
			float fill               = playerMaxHP > 0 ? (float)playerCurrentHP / playerMaxHP : 1f;
			_playerHPFill.fillAmount = fill;

			if (_playerHPGhost != null) _playerHPGhost.fillAmount = fill;
			if (_playerHPText  != null) _playerHPText.text        = $"{playerCurrentHP}/{playerMaxHP}";
		}

		#endregion

		#region REFRESH

		public void SetActionsInteractable(bool interactable)
		{
			_rockButton.interactable    = interactable;
			_paperButton.interactable   = interactable;
			_scissorButton.interactable = interactable;
		}

		#endregion
	}
}
