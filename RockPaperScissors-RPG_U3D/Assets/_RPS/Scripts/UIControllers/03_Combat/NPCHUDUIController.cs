using Kapibara.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Kapibara.RPS
{
	/// <summary>
	/// HUD del step de NPC. Pasiva: muestra imagen y diálogo recibidos de NPCStepManager.
	/// La navegación Prev/Next vive en PlayerHUDUIController.
	/// </summary>
	public class NPCHUDUIController : UIController
	{
		[Header("NPC Zone")]
		[SerializeField] private Image           _npcImage;
		[SerializeField] private GameObject      _npcDialogueBubble;
		[SerializeField] private TextMeshProUGUI _npcDialogueText;
		[SerializeField] private GameObject      _playerDialogueBubble;
		[SerializeField] private TextMeshProUGUI _playerDialogueText;

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
			_playerDialogueBubble.SetActive(false);
		}

		public void SetData(Sprite npcSprite)
		{
			_npcImage.sprite = npcSprite;
		}

		#endregion

		#region DIALOGUE

		public void SetDialogueLine(string line, bool isPlayer)
		{
			_npcDialogueBubble.SetActive(!isPlayer);
			_playerDialogueBubble.SetActive(isPlayer);

			if (isPlayer)
				_playerDialogueText.text = line;
			else
				_npcDialogueText.text = line;
		}

		#endregion
	}
}
