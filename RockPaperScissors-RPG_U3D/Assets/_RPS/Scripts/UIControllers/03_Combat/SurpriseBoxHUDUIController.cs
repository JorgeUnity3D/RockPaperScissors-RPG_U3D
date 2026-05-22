using Kapibara.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Kapibara.RPS
{
	/// <summary>
	/// HUD de la Caja Sorpresa. Muestra el icono del efecto recibido y la cantidad.
	/// Pasiva: no lee AppContext ni GameConsts.
	/// </summary>
	public class SurpriseBoxHUDUIController : UIController
	{
		[Header("Content")]
		[SerializeField] private Image           _boxImage;
		[SerializeField] private Image           _effectIcon;
		[SerializeField] private TextMeshProUGUI _effectLabel;

		[Header("Icons")]
		[SerializeField] private Sprite _healSprite;
		[SerializeField] private Sprite _energySprite;

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

		public void SetData(bool isHeal, int amount)
		{
			_effectIcon.sprite = isHeal ? _healSprite : _energySprite;
			_effectLabel.text  = isHeal ? $"+{amount} HP" : $"+{amount} Energy";
		}

		#endregion
	}
}
