using System;
using System.Collections.Generic;
using Kapibara.RPS.Extensions;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Kapibara.RPS
{
	public class ScissorsBonfireUIController : BaseUIController
	{
		[Header("Level Holder")]
		[SerializeField] private TextMeshProUGUI _currentLevelText;
		[SerializeField] private TextMeshProUGUI _nextLevelText;
		[SerializeField] private TextMeshProUGUI _goldCostText;
		[SerializeField] private Button _levelUpButton;
		[Header("Stats Variation Holders")]
		[SerializeField] private ScissorBonfireDictionary _scissorBonfireDictionary;
		[SerializeField] private List<Sprite> _variationIcons;
		[SerializeField] private List<Color> _variationColors;
		
		#region UNITY_LIFECYCLE

		private void Awake()
		{
			SetUp();
		}

		#endregion

		#region SETUP

		public override void SetUp()
		{
			Debug.Log($"[ScissorsBonfireUIController] SetUp() -> ");
			HideCanvas(0);
			_levelUpButton.AddListener(ConfirmLevelUp);
		}
		
		#endregion

		#region CONTROL

		public void SetData(int playerLevel)
		{
			UpdateLevelUpView(playerLevel);
			UpdateLevelVariantsView(playerLevel);
		}
		
		public void UpdateLevelUpView(int playerLevel)
		{
			Debug.Log($"[ScissorsBonfireUIController] UpdateLevelUpView() -> level {playerLevel}");
			_currentLevelText.text = playerLevel.ToString();
			_nextLevelText.text = (playerLevel + 1).ToString();
			int cost = GameConsts.LEVEL_PRICES_AUX[playerLevel - 1];
			_goldCostText.text = cost.ToString(); 
			_levelUpButton.interactable = AppContext.Player.Gold >= cost;
			UpdateLevelVariantsView(playerLevel);
		}
		
		private void UpdateLevelVariantsView(int playerLevel)
		{
			_scissorBonfireDictionary.ForEach((sbd =>
			{
				Stats stat = sbd.Key;
				ScissorBonfireVariation scissorBonfireVariation = sbd.Value;
				int statVariation = GameConsts.SCISSOR_MODS[playerLevel][stat];
				int indexValue = statVariation == 0 ? 1 : statVariation > 0 ? 0 : 2;
				scissorBonfireVariation.SetData(_variationIcons[indexValue], _variationColors[indexValue]);
			}));
		}
		
		private void ConfirmLevelUp()
		{
			Debug.Log($"[ScissorsBonfireUIController] ConfirmLevelUp() -> ");
			AppEvents.OnConfirmLevelUp?.Invoke();
		}
		
		#endregion
		
	}
}