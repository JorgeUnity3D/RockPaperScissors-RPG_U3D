using DG.Tweening;
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
		[SerializeField] private Image           _playerHPGhost;
		[SerializeField] private Image           _playerEnergyGhost;
		[SerializeField] private TextMeshProUGUI _playerHPText;
		[SerializeField] private TextMeshProUGUI _playerEnergyText;

		[Header("Enemy Bars")]
		[SerializeField] private Image           _enemyHPFill;
		[SerializeField] private Image           _enemyEnergyFill;
		[SerializeField] private Image           _enemyHPGhost;
		[SerializeField] private Image           _enemyEnergyGhost;
		[SerializeField] private TextMeshProUGUI _enemyHPText;
		[SerializeField] private TextMeshProUGUI _enemyEnergyText;

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

			_playerHPFill.fillAmount     = 1f;
			_playerEnergyFill.fillAmount = 1f;
			_enemyHPFill.fillAmount      = 1f;
			_enemyEnergyFill.fillAmount  = 1f;

			if (_playerHPGhost     != null) _playerHPGhost.fillAmount     = 1f;
			if (_playerEnergyGhost != null) _playerEnergyGhost.fillAmount = 1f;
			if (_enemyHPGhost      != null) _enemyHPGhost.fillAmount      = 1f;
			if (_enemyEnergyGhost  != null) _enemyEnergyGhost.fillAmount  = 1f;

			if (_playerHPText     != null) _playerHPText.text     = $"{playerMaxHP}/{playerMaxHP}";
			if (_playerEnergyText != null) _playerEnergyText.text = $"{playerMaxEnergy}/{playerMaxEnergy}";
			if (_enemyHPText      != null) _enemyHPText.text      = $"{enemyMaxHP}/{enemyMaxHP}";
			if (_enemyEnergyText  != null) _enemyEnergyText.text  = $"{enemyMaxEnergy}/{enemyMaxEnergy}";
		}

		#endregion

		#region REFRESH

		public void RefreshPlayerBars(int currentHP, int currentEnergy)
		{
			float hpFill     = _playerMaxHP     > 0 ? (float)currentHP     / _playerMaxHP     : 0f;
			float energyFill = _playerMaxEnergy > 0 ? (float)currentEnergy / _playerMaxEnergy : 0f;

			AnimateFill(_playerHPFill,     _playerHPGhost,     hpFill);
			AnimateFill(_playerEnergyFill, _playerEnergyGhost, energyFill);

			if (_playerHPText     != null) _playerHPText.text     = $"{currentHP}/{_playerMaxHP}";
			if (_playerEnergyText != null) _playerEnergyText.text = $"{currentEnergy}/{_playerMaxEnergy}";
		}

		public void RefreshEnemyBars(int currentHP, int currentEnergy)
		{
			float hpFill     = _enemyMaxHP     > 0 ? (float)currentHP     / _enemyMaxHP     : 0f;
			float energyFill = _enemyMaxEnergy > 0 ? (float)currentEnergy / _enemyMaxEnergy : 0f;

			AnimateFill(_enemyHPFill,     _enemyHPGhost,     hpFill);
			AnimateFill(_enemyEnergyFill, _enemyEnergyGhost, energyFill);

			if (_enemyHPText     != null) _enemyHPText.text     = $"{currentHP}/{_enemyMaxHP}";
			if (_enemyEnergyText != null) _enemyEnergyText.text = $"{currentEnergy}/{_enemyMaxEnergy}";
		}

		private void AnimateFill(Image fill, Image ghost, float target)
		{
			fill.DOKill();
			fill.DOFillAmount(target, GameConsts.COMBAT_BAR_ANIM).SetEase(Ease.OutQuad);

			if (ghost == null) return;
			ghost.DOKill();
			ghost.DOFillAmount(target, GameConsts.COMBAT_BAR_GHOST_DUR)
			     .SetDelay(GameConsts.COMBAT_BAR_GHOST_DELAY)
			     .SetEase(Ease.OutQuad);
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
			_enemyThoughtIcon.sprite  = languageIcon;
			_enemyThoughtIcon.enabled = languageIcon != null;
		}

		public void HideBubbles()
		{
			_playerActionBubble.SetActive(false);
			_playerThoughtBubble.SetActive(false);
			_enemyActionBubble.SetActive(false);
			_enemyThoughtBubble.SetActive(false);
			if (_enemyThoughtIcon != null) _enemyThoughtIcon.enabled = true;
		}

		#endregion
	}
}
