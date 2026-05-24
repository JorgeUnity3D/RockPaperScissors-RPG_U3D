using System.Collections.Generic;
using Kapibara.Util.Extensions;
using Kapibara.UI;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Kapibara.RPS
{
	/// <summary>
	/// Vista de la Hoguera de las Tijeras. Muestra el nivel actual, el coste de subida y las variaciones de estadística por nivel.
	/// </summary>
	public class ScissorsBonfireUIController : UIController
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

		/// <summary>Alimenta la vista con los datos iniciales del nivel del jugador.</summary>
		public void SetData(int playerLevel, int levelUpCost, bool canAfford, ScissorBonfireModLevel statVariations)
		{
			UpdateLevelUpView(playerLevel, levelUpCost, canAfford, statVariations);
		}

		/// <summary>Actualiza textos, botón y variaciones de estadística para reflejar el estado actual de nivel.</summary>
		public void UpdateLevelUpView(int playerLevel, int levelUpCost, bool canAfford, ScissorBonfireModLevel statVariations)
		{
			Debug.Log($"[ScissorsBonfireUIController] UpdateLevelUpView() -> level {playerLevel}");
			_currentLevelText.text = playerLevel.ToString();
			_nextLevelText.text = (playerLevel + 1).ToString();
			_goldCostText.text = levelUpCost.ToString();
			_levelUpButton.interactable = canAfford;
			UpdateLevelVariantsView(statVariations);
		}

		private void UpdateLevelVariantsView(ScissorBonfireModLevel statVariations)
		{
			if (statVariations == null) return;
			_scissorBonfireDictionary.ForEach((sbd =>
			{
				Stats stat = sbd.Key;
				ScissorBonfireVariation scissorBonfireVariation = sbd.Value;
				int statVariation = statVariations[stat];
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