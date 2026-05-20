using DG.Tweening;
using Kapibara.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Kapibara.RPS
{
	/// <summary>
	/// HUD persistente del jugador durante toda la escena de combate.
	/// Muestra barras de vida/energía, gestiona los grupos de acciones según el step activo
	/// y emite AppEvents al interactuar. Pasiva: no lee AppContext ni GameConsts.
	/// </summary>
	public class PlayerHUDUIController : UIController
	{
		[Header("Player Bars")]
		[SerializeField] private Image           _playerHPFill;
		[SerializeField] private Image           _playerEnergyFill;
		[SerializeField] private Image           _playerHPGhost;
		[SerializeField] private Image           _playerEnergyGhost;
		[SerializeField] private TextMeshProUGUI _playerHPText;
		[SerializeField] private TextMeshProUGUI _playerEnergyText;

		[Header("Player Zone")]
		[SerializeField] private Image           _playerSprite;
		[SerializeField] private GameObject      _playerThoughtBubble;
		[SerializeField] private Image           _playerThoughtIcon;
		[SerializeField] private GameObject      _playerActionBubble;
		[SerializeField] private Image           _playerActionIcon;
		[SerializeField] private TextMeshProUGUI _playerDebugText;

		[Header("Actions - Combat")]
		[SerializeField] private GameObject      _combatActionsGroup;
		[SerializeField] private Button          _rockButton;
		[SerializeField] private Button          _paperButton;
		[SerializeField] private Button          _scissorButton;
		[SerializeField] private Button          _defenseButton;
		[SerializeField] private Button          _energyButton;

		[Header("Actions - Treasure")]
		[SerializeField] private GameObject      _treasureActionsGroup;
		[SerializeField] private Button          _treasureRockButton;
		[SerializeField] private Button          _treasurePaperButton;
		[SerializeField] private Button          _treasureScissorButton;

		[Header("Actions - NPC")]
		[SerializeField] private GameObject      _npcActionsGroup;
		[SerializeField] private Button          _prevButton;
		[SerializeField] private Button          _nextButton;

		[Header("Actions - Backpack")]
		[SerializeField] private GameObject      _backpackActionsGroup;
		[SerializeField] private Button          _openBackpackButton;
		[SerializeField] private Button          _shurikenButton;
		[SerializeField] private Button          _potionButton;
		[SerializeField] private Button          _torchButton;
		[SerializeField] private Button          _closeBackpackButton;

		private int      _playerMaxHP;
		private int      _playerMaxEnergy;
		private Actions  _pendingAction;
		private Language _commonLanguage;

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

			_rockButton.onClick.AddListener(    () => OnCombatActionPressed(Actions.ROCK));
			_paperButton.onClick.AddListener(   () => OnCombatActionPressed(Actions.PAPER));
			_scissorButton.onClick.AddListener( () => OnCombatActionPressed(Actions.SCISSOR));
			_defenseButton.onClick.AddListener( () => OnCombatActionPressed(Actions.DEFENSE));
			_energyButton.onClick.AddListener(  () => OnCombatActionPressed(Actions.ENERGY));

			_treasureRockButton.onClick.AddListener(    () => AppEvents.OnTreasureActionSelected?.Invoke(Actions.ROCK));
			_treasurePaperButton.onClick.AddListener(   () => AppEvents.OnTreasureActionSelected?.Invoke(Actions.PAPER));
			_treasureScissorButton.onClick.AddListener( () => AppEvents.OnTreasureActionSelected?.Invoke(Actions.SCISSOR));

			_prevButton.onClick.AddListener(() => AppEvents.OnNPCDialoguePrev?.Invoke());
			_nextButton.onClick.AddListener(() => AppEvents.OnNPCDialogueNext?.Invoke());

			_openBackpackButton.onClick.AddListener(OpenBackpack);
			_closeBackpackButton.onClick.AddListener(CloseBackpack);
			_shurikenButton.onClick.AddListener(() => OnItemPressed(ItemType.SHURIKEN));
			_potionButton.onClick.AddListener(  () => OnItemPressed(ItemType.HEALTH_POTION));
			_torchButton.onClick.AddListener(   () => OnItemPressed(ItemType.TORCH));

			HidePlayerBubbles();
		}

		public void SetData(int playerMaxHP, int playerMaxEnergy)
		{
			_playerMaxHP     = playerMaxHP;
			_playerMaxEnergy = playerMaxEnergy;

			_playerHPFill.fillAmount     = 1f;
			_playerEnergyFill.fillAmount = 1f;

			if (_playerHPGhost     != null) _playerHPGhost.fillAmount     = 1f;
			if (_playerEnergyGhost != null) _playerEnergyGhost.fillAmount = 1f;

			if (_playerHPText     != null) _playerHPText.text     = $"{playerMaxHP}/{playerMaxHP}";
			if (_playerEnergyText != null) _playerEnergyText.text = $"{playerMaxEnergy}/{playerMaxEnergy}";
		}

		public void SetCommonLanguage(Language language)
		{
			_commonLanguage = language;
		}

		public void SetStep(MapStepType stepType)
		{
			bool isCombat = stepType == MapStepType.COMBAT || stepType == MapStepType.BOSS;
			_combatActionsGroup.SetActive(isCombat);
			_backpackActionsGroup.SetActive(false);
			_treasureActionsGroup.SetActive(stepType == MapStepType.TREASURE);
			_npcActionsGroup.SetActive(stepType == MapStepType.NPC_RESCUE);
		}

		public void SetBackpackData(int attackLevel, int healLevel, int energyLevel)
		{
			_shurikenButton.interactable = attackLevel > 0;
			_potionButton.interactable   = healLevel   > 0;
			_torchButton.interactable    = energyLevel > 0;
		}

		public void SetItemUsed(ItemType type)
		{
			switch (type)
			{
				case ItemType.SHURIKEN:      _shurikenButton.interactable = false; break;
				case ItemType.HEALTH_POTION: _potionButton.interactable   = false; break;
				case ItemType.TORCH:         _torchButton.interactable    = false; break;
			}
		}

		#endregion

		#region REFRESH

		public void RefreshBars(int currentHP, int currentEnergy)
		{
			float hpFill     = _playerMaxHP     > 0 ? (float)currentHP     / _playerMaxHP     : 0f;
			float energyFill = _playerMaxEnergy > 0 ? (float)currentEnergy / _playerMaxEnergy : 0f;

			AnimateFill(_playerHPFill,     _playerHPGhost,     hpFill);
			AnimateFill(_playerEnergyFill, _playerEnergyGhost, energyFill);

			if (_playerHPText     != null) _playerHPText.text     = $"{currentHP}/{_playerMaxHP}";
			if (_playerEnergyText != null) _playerEnergyText.text = $"{currentEnergy}/{_playerMaxEnergy}";
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

		public void SetCombatActionsInteractable(bool interactable)
		{
			_rockButton.interactable         = interactable;
			_paperButton.interactable        = interactable;
			_scissorButton.interactable      = interactable;
			_defenseButton.interactable      = interactable;
			_energyButton.interactable       = interactable;
			_openBackpackButton.interactable = interactable;
		}

		public void SetTreasureActionsInteractable(bool interactable)
		{
			_treasureRockButton.interactable    = interactable;
			_treasurePaperButton.interactable   = interactable;
			_treasureScissorButton.interactable = interactable;
		}

		public void SetNPCPrevInteractable(bool interactable)
		{
			_prevButton.interactable = interactable;
		}

		#endregion

		#region BUBBLES

		public void ShowPlayerActionBubble(Sprite actionIcon, int debugDamage)
		{
			_playerActionBubble.SetActive(true);
			_playerActionIcon.sprite = actionIcon;
			_playerDebugText.text    = debugDamage.ToString();
		}

		public void HidePlayerBubbles()
		{
			_pendingAction = Actions.NONE;
			_playerThoughtBubble.SetActive(false);
			_playerActionBubble.SetActive(false);
		}

		#endregion

		#region INPUT

		private void OnCombatActionPressed(Actions action)
		{
			if (_pendingAction == action)
			{
				_pendingAction = Actions.NONE;
				_playerThoughtBubble.SetActive(false);
				AppEvents.OnCombatActionSelected?.Invoke(action);
			}
			else
			{
				_pendingAction = action;
				_playerThoughtBubble.SetActive(true);
				_playerThoughtIcon.sprite = GetActionIconCommon(action);
			}
		}

		private Sprite GetActionIconCommon(Actions action)
		{
			if (_commonLanguage == null)
			{
				Debug.LogWarning("[PlayerHUDUIController] _commonLanguage not set. Call SetCommonLanguage() before combat starts.");
				return null;
			}
			return _commonLanguage.GetActionIcon(action);
		}

		private void OpenBackpack()
		{
			_combatActionsGroup.SetActive(false);
			_backpackActionsGroup.SetActive(true);
		}

		private void CloseBackpack()
		{
			_backpackActionsGroup.SetActive(false);
			_combatActionsGroup.SetActive(true);
		}

		private void OnItemPressed(ItemType type)
		{
			CloseBackpack();
			AppEvents.OnBackpackItemUsed?.Invoke(type);
		}

		#endregion
	}
}
