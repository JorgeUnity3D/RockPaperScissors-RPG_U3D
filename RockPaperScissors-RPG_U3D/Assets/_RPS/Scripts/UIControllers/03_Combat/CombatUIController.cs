using Kapibara.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Kapibara.RPS
{
	/// <summary>
	/// Vista del combate. Pasiva: renderiza datos recibidos del CombatManager y emite AppEvents al pulsar botones.
	/// </summary>
	public class CombatUIController : UIController
	{
		[Header("Player Bars")]
		[SerializeField] private Image           _playerHPFill;
		[SerializeField] private Image           _playerEnergyFill;

		[Header("Enemy Bars")]
		[SerializeField] private Image           _enemyHPFill;
		[SerializeField] private Image           _enemyEnergyFill;

		[Header("Player Zone")]
		[SerializeField] private Image           _playerSprite;
		[SerializeField] private GameObject      _playerThoughtBubble;
		[SerializeField] private Image           _playerThoughtIcon;
		[SerializeField] private GameObject      _playerActionBubble;
		[SerializeField] private Image           _playerActionIcon;
		[SerializeField] private TextMeshProUGUI _playerDebugText;

		[Header("Enemy Zone")]
		[SerializeField] private Image           _enemySprite;
		[SerializeField] private GameObject      _enemyActionBubble;
		[SerializeField] private Image           _enemyActionIcon;
		[SerializeField] private TextMeshProUGUI _enemyDebugText;
		[SerializeField] private GameObject      _enemyThoughtBubble;
		[SerializeField] private Image           _enemyThoughtIcon;

		[Header("Center")]
		[SerializeField] private Button          _settingsButton;

		[Header("Actions")]
		[SerializeField] private Button          _rockButton;
		[SerializeField] private Button          _paperButton;
		[SerializeField] private Button          _scissorButton;
		[SerializeField] private Button          _defenseButton;
		[SerializeField] private Button          _energyButton;

		private int _playerMaxHP;
		private int _playerMaxEnergy;
		private int _enemyMaxHP;
		private int _enemyMaxEnergy;

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

			_rockButton.onClick.AddListener(    () => AppEvents.OnCombatActionSelected?.Invoke(Actions.ROCK));
			_paperButton.onClick.AddListener(   () => AppEvents.OnCombatActionSelected?.Invoke(Actions.PAPER));
			_scissorButton.onClick.AddListener( () => AppEvents.OnCombatActionSelected?.Invoke(Actions.SCISSOR));
			_defenseButton.onClick.AddListener( () => AppEvents.OnCombatActionSelected?.Invoke(Actions.DEFENSE));
			_energyButton.onClick.AddListener(  () => AppEvents.OnCombatActionSelected?.Invoke(Actions.ENERGY));

			HideBubbles();
		}

		public void SetData(Sprite enemySprite,
		                    int playerMaxHP, int playerMaxEnergy,
		                    int enemyMaxHP,  int enemyMaxEnergy)
		{
			_enemySprite.sprite = enemySprite;

			_playerMaxHP     = playerMaxHP;
			_playerMaxEnergy = playerMaxEnergy;
			_enemyMaxHP      = enemyMaxHP;
			_enemyMaxEnergy  = enemyMaxEnergy;

			RefreshPlayerBars(playerMaxHP, playerMaxEnergy);
			RefreshEnemyBars(enemyMaxHP,   enemyMaxEnergy);
		}

		#endregion

		#region REFRESH

		public void RefreshPlayerBars(int currentHP, int currentEnergy)
		{
			_playerHPFill.fillAmount     = _playerMaxHP     > 0 ? (float)currentHP     / _playerMaxHP     : 0f;
			_playerEnergyFill.fillAmount = _playerMaxEnergy > 0 ? (float)currentEnergy / _playerMaxEnergy : 0f;
		}

		public void RefreshEnemyBars(int currentHP, int currentEnergy)
		{
			_enemyHPFill.fillAmount     = _enemyMaxHP     > 0 ? (float)currentHP     / _enemyMaxHP     : 0f;
			_enemyEnergyFill.fillAmount = _enemyMaxEnergy > 0 ? (float)currentEnergy / _enemyMaxEnergy : 0f;
		}

		public void SetActionsInteractable(bool interactable)
		{
			_rockButton.interactable    = interactable;
			_paperButton.interactable   = interactable;
			_scissorButton.interactable = interactable;
			_defenseButton.interactable = interactable;
			_energyButton.interactable  = interactable;
		}

		#endregion

		#region BUBBLES

		public void ShowPlayerActionBubble(Sprite actionIcon, int debugDamage)
		{
			_playerActionBubble.SetActive(true);
			_playerActionIcon.sprite = actionIcon;
			_playerDebugText.text    = debugDamage.ToString();
		}

		public void ShowEnemyActionBubble(Sprite actionIcon, int debugDamage)
		{
			_enemyActionBubble.SetActive(true);
			_enemyActionIcon.sprite = actionIcon;
			_enemyDebugText.text    = debugDamage.ToString();
		}

		public void ShowPlayerThoughtBubble(Sprite languageIcon)
		{
			_playerThoughtBubble.SetActive(true);
			_playerThoughtIcon.sprite = languageIcon;
		}

		public void ShowEnemyThoughtBubble(Sprite languageIcon)
		{
			_enemyThoughtBubble.SetActive(true);
			_enemyThoughtIcon.sprite = languageIcon;
		}

		public void HideBubbles()
		{
			_playerActionBubble.SetActive(false);
			_playerThoughtBubble.SetActive(false);
			_enemyActionBubble.SetActive(false);
			_enemyThoughtBubble.SetActive(false);
		}

		#endregion
	}
}
