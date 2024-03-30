using System;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS
{
	[Serializable]
	public class TrainingModifier : BaseModifier
	{
		private NInt _trainingData;

		public int TrainingData
		{
			get => _trainingData.Value;
			set => _trainingData.Value = value;
		}
		
		#region CONSTRUCTOR

		public TrainingModifier(int initialModifier)
		{
			ModifierType = GameConsts.ATTRIBUTE_TYPE_VALUE[this.GetType()];
			_modifier = new NInt(initialModifier);
			_level = new NInt(1);
			_experience = new NInt(0);
			_trainingData = new NInt(0);
		}

		[JsonConstructor]
		public TrainingModifier(int modifier, int level, int experience, int trainingData)
		{
			ModifierType = GameConsts.ATTRIBUTE_TYPE_VALUE[this.GetType()];;
			_modifier = new NInt(modifier);
			_level = new NInt(level);
			_experience = new NInt(experience);
			_trainingData = new NInt(trainingData);
		}

		#endregion

		#region CONTROL

		public override int Apply(int baseValue)
		{
			return baseValue + _modifier.Value;
		}

		#endregion
	}
}