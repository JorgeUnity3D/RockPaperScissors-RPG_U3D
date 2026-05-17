using Kapibara.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Kapibara.RPS
{
	/// <summary>
	/// Vista del step de NPC. Pasiva: renderiza imagen y diálogo recibidos de NPCStepManager
	/// y emite AppEvents al pulsar los botones de navegación.
	/// </summary>
	public class NPCUIController : UIController
	{
		[Header("Player Bars")]
		[SerializeField] private Image           _playerHPFill;
		[SerializeField] private Image           _playerHPGhost;
		[SerializeField] private TextMeshProUGUI _playerHPText;

		[Header("NPC Zone")]
		[SerializeField] private Image           _npcImage;
		[SerializeField] private TextMeshProUGUI _dialogueText;

		[Header("Center")]
		[SerializeField] private Button          _settingsButton;

		[Header("Actions")]
		[SerializeField] private Button          _prevButton;
		[SerializeField] private Button          _nextButton;

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

			_prevButton.onClick.AddListener(() => AppEvents.OnNPCDialoguePrev?.Invoke());
			_nextButton.onClick.AddListener(() => AppEvents.OnNPCDialogueNext?.Invoke());
		}

		public void SetData(Sprite npcSprite, int playerMaxHP, int playerCurrentHP)
		{
			_npcImage.sprite = npcSprite;

			_playerMaxHP             = playerMaxHP;
			float fill               = playerMaxHP > 0 ? (float)playerCurrentHP / playerMaxHP : 1f;
			_playerHPFill.fillAmount = fill;

			if (_playerHPGhost != null) _playerHPGhost.fillAmount = fill;
			if (_playerHPText  != null) _playerHPText.text        = $"{playerCurrentHP}/{playerMaxHP}";
		}

		#endregion

		#region REFRESH

		public void SetDialogueLine(string line)
		{
			_dialogueText.text = line;
		}

		public void SetPrevInteractable(bool interactable)
		{
			_prevButton.interactable = interactable;
		}

		#endregion
	}
}
