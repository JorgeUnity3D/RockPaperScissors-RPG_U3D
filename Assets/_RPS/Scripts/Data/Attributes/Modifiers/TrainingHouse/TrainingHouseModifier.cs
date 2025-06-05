using System;
using Kapibara.Util.NotificableFields;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS
{
	[Serializable]
	public class TrainingHouseModifier : BaseModifier
	{
		private NBool _isUnlocked;
		private NBool _isTraining;

		public bool IsUnlocked
		{
			get => _isUnlocked.Value;
			set => _isUnlocked.Value = value;
		}
		
		public bool IsTraining
		{
			get => _isTraining.Value;
			set => _isTraining.Value = value;
		}

		[JsonIgnore]
		public override int TotaModifier
		{
			get => IsUnlocked ? Modifier + (Level - 1) : 0;
		}
		
		#region CONSTRUCTOR

		public TrainingHouseModifier(Stats stat, int initialModifier = 3)
		{
			ModifierType = GameConsts.ATTRIBUTE_TYPE_VALUE[this.GetType()];
			_stat = stat;
			_modifier = new NInt(initialModifier);
			_level = new NInt(1);
			_experience = new NInt(0);
			_isUnlocked = new NBool(false);
			_isTraining = new NBool(false);
		}

		[JsonConstructor]
		public TrainingHouseModifier(Stats stat, int modifier, int level, int experience, bool isUnlocked, bool isTraining)
		{
			ModifierType = GameConsts.ATTRIBUTE_TYPE_VALUE[this.GetType()];;
			_stat = stat;
			_modifier = new NInt(modifier);
			_level = new NInt(level);
			_experience = new NInt(experience);
			_isUnlocked = new NBool(isUnlocked);
			_isTraining = new NBool(isTraining);
		}

		#endregion
		
	}
}