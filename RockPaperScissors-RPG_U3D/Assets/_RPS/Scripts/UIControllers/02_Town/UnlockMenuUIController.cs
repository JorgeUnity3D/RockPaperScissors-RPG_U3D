using Kapibara.Util.Extensions;
using Kapibara.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Kapibara.RPS
{
	/// <summary>
	/// Popup de desbloqueo de edificio. Muestra imagen, nombre y coste; el botón confirmar emite OnConfirmUnlock si hay fondos suficientes.
	/// </summary>
	public class UnlockMenuUIController : UIController
	{
		[Header("UI")]
		[SerializeField] private Image _buildingImage;
		[SerializeField] private TextMeshProUGUI _buildingText;
		[SerializeField] private TextMeshProUGUI _costText;
		[SerializeField] private Button _confirmUnlockButton;
		[SerializeField] private Button _cancelUnlockButton;

		[Header("DEBUG")]
		[SerializeField] private TownData _currentTownData;
		
		#region UNITY_LIFECYCLE

		private void Awake()
		{
			SetUp();
		}

		#endregion
		
		#region SETUP

		public override void SetUp()
		{
			Debug.Log($"[UnlockMenuUIController] SetUp() -> ");
			HideCanvas(0);
			_confirmUnlockButton.AddListener(ConfirmUnlockButton);
			_cancelUnlockButton.AddListener(CancelUnlockButton);
		}
		
		/// <summary>Rellena el popup con los datos del edificio a desbloquear y habilita el botón según si el jugador puede pagarlo.</summary>
		public void SetData(TownData townData, TownView townView, bool canAfford)
		{
			Debug.Log($"[UnlockMenuUIController] SetData() -> TownView: {townData.TownMenu}");
			_currentTownData = townData;
			_buildingImage.sprite = townView.BuildingIcon;
			_buildingText.text = _currentTownData.Name;
			_costText.text = _currentTownData.Cost.ToString();
			_confirmUnlockButton.interactable = canAfford;
		}

		#endregion
		
		#region CONTROL

		private void ConfirmUnlockButton()
		{
			Debug.Log($"[UnlockMenuUIController] ConfirmUnlockButton() -> ");
			AppEvents.OnConfirmUnlock?.Invoke(_currentTownData);
			Close();
		}

		private void CancelUnlockButton()
		{
			Debug.Log($"[UnlockMenuUIController] CancelUnlockButton() -> ");
			AppEvents.OnCancelUnlock?.Invoke();
			Close();
		}

		private void Close()
		{
			HideCanvas();
			_currentTownData = null;
		}
		
		#endregion
		
	}
}