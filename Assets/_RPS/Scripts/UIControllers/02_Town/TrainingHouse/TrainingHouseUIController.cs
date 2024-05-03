using System.Collections.Generic;
using Doozy.Runtime.UIManager.Components;
using Kapibara.RPS.Extensions;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Kapibara.RPS
{
	public class TrainingHouseUIController : BaseUIController
	{
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
		[SerializeField] private UIButton _selectTrainingButton;
		[Header("Unlock Stat")]
		[SerializeField] private GameObject _unlockStatHolder;
		[SerializeField] private TextMeshProUGUI _unlockStatNameText;
		[SerializeField] private Image _unlockStatIconImage;
		[SerializeField] private TextMeshProUGUI _unlockCostText;
		[SerializeField] private UIButton _unlockStatButton;
		[Header("Icons")]
		[SerializeField] private IconsScrObj _iconsScrObj;
		[SerializeField, ReadOnly] private IconsDictionary _icons;
		[Header("Debug")]
		[SerializeField] private Button _addExperienceButton;
		[SerializeField] private Button _addLevelButton;

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

		public void SetData(List<Attribute> attributes)
		{
			Debug.Log($"[TrainingHouseUIController] SetData() -> ");
			foreach (Attribute attribute in attributes)
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

		public void UpdateView(TrainingHouseModifier trainingHouseModifier)
		{
			Debug.Log($"[TrainingHouseUIController] UpdateView() -> stat {trainingHouseModifier.Stat}");
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
			UpdateDebugView(trainingHouseModifier);
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
			_unlockStatIconImage.sprite = _icons[trainingHouseModifier.Stat];;
			//todo: where to store cost?
			int trainingCost = GameConsts.TRAINING_MOD_PRICES[trainingHouseModifier.Stat];
			_unlockCostText.text = trainingCost.ToString();
			_unlockStatButton.interactable = AppContext.Player.Gold >= trainingCost;
			_unlockStatButton.AddListener(() =>
			{
				AppEvents.OnTrainingUnlocked?.Invoke(trainingHouseModifier);
			}, true);
		}
		
		public void SelectTrainingButton(Stats trainingModifierStat)
		{
			Debug.Log($"[TrainingHouseUIController] UpdateTrainingButton() -> stat {trainingModifierStat}");
			foreach (KeyValuePair<Stats,TrainingButton> trainigDictEntry in _trainingDictionary)
			{
				trainigDictEntry.Value.SetSelectionOverlay(trainigDictEntry.Key == trainingModifierStat);
			}
		}
		
		#endregion
		
		#region DEBUG

		private void UpdateDebugView(TrainingHouseModifier trainingHouseModifier)
		{
			bool isUnlocked = trainingHouseModifier.IsUnlocked;
			_addExperienceButton.gameObject.SetActive(isUnlocked);
			_addLevelButton.gameObject.SetActive(isUnlocked);
			
			_addExperienceButton.AddListener(() =>
			{
				trainingHouseModifier.Experience ++;
				UpdateUnlockedView(trainingHouseModifier);
				_trainingDictionary[trainingHouseModifier.Stat].SetLevel(trainingHouseModifier.Level, trainingHouseModifier.IsUnlocked);
			}, true);	
			_addLevelButton.AddListener(() =>
			{
				trainingHouseModifier.Level++;
				UpdateUnlockedView(trainingHouseModifier);
				_trainingDictionary[trainingHouseModifier.Stat].SetLevel(trainingHouseModifier.Level, trainingHouseModifier.IsUnlocked);
			}, true);	
		}

		#endregion
		
	}
}