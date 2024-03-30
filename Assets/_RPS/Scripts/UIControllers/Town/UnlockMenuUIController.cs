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

		[FormerlySerializedAs("currentTownData2"),FormerlySerializedAs("_currentTownView"),SerializeField] private TownData currentTownData;
		
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
		
		public void SetData(TownData townData, TownView townView)
		{
			Debug.Log($"[UnlockMenuUIController] SetData() -> TownView: {townData.TownMenu}");
			currentTownData = townData;
			_buildingImage.sprite = townView.buildingIcon;
			_buildingText.text = currentTownData.Name;
			_costText.text = currentTownData.Cost.ToString();
			_confirmUnlockButton.interactable = AppContext.Player.Gold >= townData.Cost;
		}

		#endregion
		
		#region CONTROL

		private void ConfirmUnlockButton()
		{
			Debug.Log($"[UnlockMenuUIController] ConfirmUnlockButton() -> ");
			AppEvents.OnConfirmUnlock?.Invoke(currentTownData);
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
			currentTownData = null;
		}
		
		#endregion
		
	}
}