using System;
using Doozy.Runtime.UIManager.Components;
using Kapibara.RPS.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
namespace Kapibara.RPS
{
	public class UnlockMenuUIController : BaseUIController
	{
		[SerializeField] private Image _buildingImage;
		[SerializeField] private TextMeshProUGUI _buildingText;
		[SerializeField] private TextMeshProUGUI _costText;
		[SerializeField] private UIButton _confirmUnlockButton;
		[SerializeField] private UIButton _cancelUnlockButton;

		[SerializeField] private TownView _currentTownView;
		
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
		
		public void SetData(TownView townView, TownData townData)
		{
			Debug.Log($"[UnlockMenuUIController] SetData() -> TownView: {townView.TownMenu}");
			_currentTownView = townView;
			_buildingImage.sprite = townData.buildingIcon;
			_buildingText.text = _currentTownView.Name;
			_costText.text = _currentTownView.Cost.ToString();
			_confirmUnlockButton.interactable = AppContext.Player.Gold >= townView.Cost;
		}

		#endregion
		
		#region CONTROL

		private void ConfirmUnlockButton()
		{
			Debug.Log($"[UnlockMenuUIController] ConfirmUnlockButton() -> ");
			AppEvents.OnConfirmUnlock?.Invoke(_currentTownView);
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
			_currentTownView = null;
		}
		
		#endregion
		
	}
}