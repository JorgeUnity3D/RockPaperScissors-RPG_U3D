using Kapibara.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Kapibara.RPS
{
	/// <summary>
	/// HUD del step de Tesoro. Pasiva: muestra imagen y gold recibidos de TreasureStepManager.
	/// Las acciones RPS viven en PlayerHUDUIController.
	/// </summary>
	public class TreasureHUDUIController : UIController
	{
		[Header("Treasure Zone")]
		[SerializeField] private Image           _treasureImage;
		[SerializeField] private TextMeshProUGUI _goldText;

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
		}

		public void SetData(Sprite treasureSprite, int goldAmount)
		{
			_treasureImage.sprite = treasureSprite;
			_goldText.text        = $"+{goldAmount} oro";
		}

		#endregion
	}
}
