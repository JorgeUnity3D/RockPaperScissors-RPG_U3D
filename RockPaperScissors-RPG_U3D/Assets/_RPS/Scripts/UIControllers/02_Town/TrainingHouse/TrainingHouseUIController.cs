using System.Collections.Generic;
using Kapibara.Util.Extensions;
using Kapibara.UI;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Kapibara.RPS
{
	/// <summary>
	/// Vista de la Casa de Entrenamiento. Muestra los botones de estadísticas entrenables y el panel de info/desbloqueo del stat seleccionado.
	/// </summary>
	public class TrainingHouseUIController : UIController
	{
		[Header("UI")]
		[Header("Stats Buttons")]
		[SerializeField] private TrainingDictionary _trainingDictionary;
		[Header("Stat Info")]
		[SerializeField] private GameObject _statInfoHolder;
		[SerializeField] private TextMeshProUGUI _statNameText;
		[SerializeField] private Image _statIconImage;
		[SerializeField] private TextMeshProUGUI _statLevelText;
		[SerializeField] private TextMeshProUGUI _statExperienceText;
		[SerializeField] private Slider _statLevelProgressSlider;
		[SerializeField] private TextMeshProUGUI _statBonusText;
		[SerializeField] private Button _selectTrainingButton;
		[Header("Unlock Stat")]
		[SerializeField] private GameObject _unlockStatHolder;
		[SerializeField] private TextMeshProUGUI _unlockStatNameText;
		[SerializeField] private Image _unlockStatIconImage;
		[SerializeField] private TextMeshProUGUI _unlockCostText;
		[SerializeField] private Button _unlockStatButton;
		[Header("Icons")]
		[SerializeField] private IconsScrObj _iconsScrObj;
		[SerializeField, ReadOnly] private IconsDictionary _icons;

		private int _playerGold;
		private Dictionary<Stats, int> _trainingCosts;

		#region UNITY_LIFECYCLE

		private void Awake()
		{
			SetUp();
		}

		#endregion
		
		#region SETUP

		public override void SetUp()
		{
			Debug.Log($"[TrainingHouseUIController] SetUp() -> ");
			HideCanvas(0);
			_icons = _iconsScrObj.Data;
		}		

		/// <summary>Inicializa todos los botones de entrenamiento con el estado actual de cada modificador.</summary>
		public void SetData(List<StatAttribute> attributes, int playerGold, Dictionary<Stats, int> trainingCosts)
		{
			Debug.Log($"[TrainingHouseUIController] SetData() -> ");
			_playerGold = playerGold;
			_trainingCosts = trainingCosts;
			foreach (StatAttribute attribute in attributes)
			{
				TrainingHouseModifier trainingHouseModifier = attribute.GetModifier<TrainingHouseModifier>();
				if (trainingHouseModifier != null)
				{
					UpdateTrainingButton(trainingHouseModifier);
				}
			}
		}
		
		#endregion
		
		#region CONTROL

		/// <summary>Refresca el botón y el panel de detalle para el modificador indicado tras un cambio de estado.</summary>
		public void UpdateView(TrainingHouseModifier trainingHouseModifier, int playerGold)
		{
			Debug.Log($"[TrainingHouseUIController] UpdateView() -> stat {trainingHouseModifier.Stat}");
			_playerGold = playerGold;
			UpdateTrainingButton(trainingHouseModifier);
			SetStatView(trainingHouseModifier);
		}
		
		private void UpdateTrainingButton(TrainingHouseModifier trainingHouseModifier)
		{
			Debug.Log($"[TrainingHouseUIController] UpdateTrainingButton() -> stat {trainingHouseModifier.Stat}");
			TrainingButton trainingButton = _trainingDictionary[trainingHouseModifier.Stat];
			trainingButton.SetButtonData(trainingHouseModifier, _icons[trainingHouseModifier.Stat], SetStatView);
		}

		private void SetStatView(TrainingHouseModifier trainingHouseModifier)
		{
			Debug.Log($"[TrainingHouseUIController] UpdateTrainingButton() -> stat {trainingHouseModifier.Stat} is unlocked {trainingHouseModifier.IsUnlocked}");
			_statInfoHolder.SetActive(trainingHouseModifier.IsUnlocked);
			_unlockStatHolder.SetActive(!trainingHouseModifier.IsUnlocked);
			if (trainingHouseModifier.IsUnlocked)
			{
				UpdateUnlockedView(trainingHouseModifier);
			}
			else
			{
				UpdateLockedView(trainingHouseModifier);
			}
			}

		private void UpdateUnlockedView(TrainingHouseModifier trainingHouseModifier)
		{
			Debug.Log($"[TrainingHouseUIController] UpdateUnlockedView() -> stat {trainingHouseModifier.Stat}");
			_statNameText.text = trainingHouseModifier.Stat.ToString();
			_statIconImage.sprite = _icons[trainingHouseModifier.Stat];
			_statLevelText.text = "Lv. " +trainingHouseModifier.Level;
			_statExperienceText.text = "Exp: " +trainingHouseModifier.Experience;
			_statLevelProgressSlider.value = trainingHouseModifier.LevelProgress;
			_statBonusText.text = "Bonus: " + trainingHouseModifier.TotaModifier; 
			_selectTrainingButton.AddListener(() =>
			{
				AppEvents.OnTrainingSelected?.Invoke(trainingHouseModifier);
			}, true);
		}

		private void UpdateLockedView(TrainingHouseModifier trainingHouseModifier)
		{
			Debug.Log($"[TrainingHouseUIController] UpdateLockedView() -> stat {trainingHouseModifier.Stat}");
			_unlockStatNameText.text = trainingHouseModifier.Stat.ToString();
			_unlockStatIconImage.sprite = _icons[trainingHouseModifier.Stat];
			int trainingCost = _trainingCosts[trainingHouseModifier.Stat];
			_unlockCostText.text = trainingCost.ToString();
			_unlockStatButton.interactable = _playerGold >= trainingCost;
			_unlockStatButton.AddListener(() =>
			{
				AppEvents.OnTrainingUnlocked?.Invoke(trainingHouseModifier);
			}, true);
		}
		
		/// <summary>Marca visualmente como seleccionado el botón de la estadística indicada y deselecciona el resto.</summary>
		public void SelectTrainingButton(Stats trainingModifierStat)
		{
			Debug.Log($"[TrainingHouseUIController] UpdateTrainingButton() -> stat {trainingModifierStat}");
			foreach (KeyValuePair<Stats,TrainingButton> trainigDictEntry in _trainingDictionary)
			{
				trainigDictEntry.Value.SetSelectionOverlay(trainigDictEntry.Key == trainingModifierStat);
			}
		}
		
		#endregion

	}
}