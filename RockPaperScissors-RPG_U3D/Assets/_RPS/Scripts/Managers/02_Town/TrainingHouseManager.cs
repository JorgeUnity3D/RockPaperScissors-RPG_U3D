using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS
{
	/// <summary>
	/// Manager de la Casa de Entrenamiento. Gestiona la selección de estadística a entrenar y el desbloqueo de slots de entrenamiento.
	/// </summary>
	public class TrainingHouseManager : BaseManager
	{
		[SerializeField, ReadOnly] private List<StatAttribute> _trainingAttributes;
		
		[SerializeField, ReadOnly] private TrainingHouseUIController _trainingHouseUIController;
		
		#region SETUP
		
		public override void SetUp()
		{
			Debug.Log($"[TrainingHouseManager] SetUp() -> ");
			_trainingHouseUIController = ServiceLocator.Instance.GetService<UIService>().GetController<TrainingHouseUIController>();
			_trainingAttributes = AppContext.Attributes;
		}

		protected override void Subscribe()
		{
			Debug.Log($"[TrainingHouseManager] Subscribe() -> ");
			AppEvents.OnTrainingSelected += SelectTrainingStat;
			AppEvents.OnTrainingUnlocked += UnlockTraining;
		}
		
		protected override void UnSubscribe()
		{
			Debug.Log($"[TrainingHouseManager] UnSubscribe() -> ");
			AppEvents.OnTrainingSelected -= SelectTrainingStat;
			AppEvents.OnTrainingUnlocked -= UnlockTraining;
		}

        #endregion

        #region CONTROL

		public override void Initialize()
		{
			Debug.Log($"[TrainingHouseManager] Initialize() -> ");
			_trainingHouseUIController.SetData(_trainingAttributes, AppContext.Player.Gold, GameConsts.TRAINING_MOD_PRICES);
		}
		
		private void SelectTrainingStat(TrainingHouseModifier trainingHouseModifier)
		{
			Debug.Log($"[TrainingHouseManager] SelectTrainingStat() -> stat {trainingHouseModifier.Stat}");
			foreach (StatAttribute trainingAttribute in _trainingAttributes)
			{
				if (trainingAttribute.GetModifier<TrainingHouseModifier>().IsTraining)
				{
					trainingAttribute.GetModifier<TrainingHouseModifier>().IsTraining = false;
				}
				else
				{
					if (trainingAttribute.GetModifier<TrainingHouseModifier>().Stat == trainingHouseModifier.Stat)
					{
						trainingAttribute.GetModifier<TrainingHouseModifier>().IsTraining = true;
					}	
				}
			}
			_trainingHouseUIController.SelectTrainingButton(trainingHouseModifier.Stat);
		}

		private void UnlockTraining(TrainingHouseModifier trainingHouseModifier)
		{
			Debug.Log($"[TrainingHouseManager] UnlockTraining() -> stat {trainingHouseModifier.Stat}");
			int trainingCost = GameConsts.TRAINING_MOD_PRICES[trainingHouseModifier.Stat];
			AppContext.Player.Gold = Mathf.Max(0, AppContext.Player.Gold - trainingCost);
			trainingHouseModifier.IsUnlocked = true;
			_trainingHouseUIController.UpdateView(trainingHouseModifier, AppContext.Player.Gold);
		}
		
		#endregion

		#region DEBUG

		[FoldoutGroup("DEBUG"), Button("Add Experience")]
		private void Debug_AddExperience(Stats stat)
		{
			TrainingHouseModifier modifier = GetTrainingModifier(stat);
			if (modifier == null) return;
			modifier.Experience++;
			_trainingHouseUIController.UpdateView(modifier, AppContext.Player.Gold);
		}

		[FoldoutGroup("DEBUG"), Button("Add Level")]
		private void Debug_AddLevel(Stats stat)
		{
			TrainingHouseModifier modifier = GetTrainingModifier(stat);
			if (modifier == null) return;
			modifier.Level++;
			_trainingHouseUIController.UpdateView(modifier, AppContext.Player.Gold);
		}

		private TrainingHouseModifier GetTrainingModifier(Stats stat)
		{
			StatAttribute attribute = _trainingAttributes.Find(a => a.Stat == stat);
			return attribute?.GetModifier<TrainingHouseModifier>();
		}

		#endregion
	}
}