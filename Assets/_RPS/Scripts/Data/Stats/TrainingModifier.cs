using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Kapibara.RPS
{
	[Serializable]
	public class TrainingModifier : BaseModifier
	{
		private NotificableField<int> _trainingData;

		public int TrainingData
		{
			get => _trainingData.Value;
			set => _trainingData.Value = value;
		}
		
		#region CONSTRUCTOR

		public TrainingModifier(int initialModifier)
		{
			ObjType = 0;
			_modifier = new NotificableField<int>() { Value = initialModifier };
			_level = new NotificableField<int>() { Value = 1 };
			_experience = new NotificableField<int>() { Value = 0 };
			_trainingData = new NotificableField<int>() { Value = 0 };
		}

		[JsonConstructor]
		public TrainingModifier(int modifier, int level, int experience, int trainingData)
		{
			ObjType = 0;
			_modifier = new NotificableField<int>() { Value = modifier };
			_level = new NotificableField<int>() { Value = level };
			_experience = new NotificableField<int>() { Value = experience };
			_trainingData = new NotificableField<int>() { Value = trainingData };
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