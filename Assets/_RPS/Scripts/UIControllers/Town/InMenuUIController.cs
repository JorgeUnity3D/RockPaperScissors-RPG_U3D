﻿using Doozy.Runtime.UIManager.Components;
using Kapibara.RPS.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace Kapibara.RPS
{
	public class InMenuUIController : BaseUIController
	{
		[SerializeField] private UIButton _backButton;
		[SerializeField] private TextMeshProUGUI _menuNameText;
		[SerializeField] private TextMeshProUGUI _menuLevelText;
		[SerializeField] private Slider _menuLevelSlider;
		[SerializeField] private GameObject _menuNPCHolder;
		[SerializeField] private TextMeshProUGUI _menuNPCText;
		[SerializeField] private Image _menuNPCImage;
		
		#region UNITY_LIFECYCLE

		private void Awake()
		{
			SetUp();
		}

		#endregion
		
		#region SETUP

		public override void SetUp()
		{
			Debug.Log($"[InMenuUIController] SetUp() -> ");
			HideCanvas(0);
			_backButton.AddListener(GoToMain);
		}
		
		public void SetData(TownView townView, TownData townData)
		{
			Debug.Log($"[InMenuUIController] SetData() -> townView {townView.name}");
			_menuNameText.text = townView.name;
			_menuLevelText.text = townView.level.ToString();
			_menuLevelSlider.value = townView.LevelProgress;
			//_menuNPCHolder.SetActive(townView.npc);
			_menuNPCText.text = townView.message + (townView.npcUnlocked ? "" : " but there's no NPC here!");
			_menuNPCImage.sprite = townView.npcUnlocked ? townData.npcIcon : townData.npcNotFoundIcon;
			
		}

		#endregion
		
		#region CONTROL

		private void GoToMain()
		{
			Debug.Log($"[InMenuUIController] SetData() -> Invoking: AppEvents.OnBackFromTownMenu");
			AppEvents.OnBackFromTownMenu?.Invoke();
		}

		#endregion
		
	}
}