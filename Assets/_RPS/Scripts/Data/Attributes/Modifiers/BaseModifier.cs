using System;
using Kapibara.Util.NotificableFields;
using Kapibara.Util.Serialization;
using Newtonsoft.Json;
using UnityEngine;

namespace Kapibara.RPS
{
	[Serializable, JsonConverter(typeof(BaseModifierConverter))]
	public abstract class BaseModifier
	{
		#region FIELDS

		[SerializeField] protected Stats _stat;
		[SerializeField] protected NInt _modifier;
		[SerializeField] protected NInt _level;
		[SerializeField] protected NInt _experience;

		#endregion

		#region PROPERTIES

		public ModifierType ModifierType { get; set; }

		public Stats Stat
		{
			get => _stat;
			set => _stat = value;
		}

		public int Modifier
		{
			get => _modifier.Value;
			set => _modifier.Value = value;
		}

		public int Level
		{
			get => _level.Value;
			set
			{
				_level.Value = value;
				_experience.Value = GameConsts.TRAINING_EXP_PER_LEVEL[_level.Value - 1]; 
			}
		}

		public int Experience
		{
			get => _experience.Value;
			set
			{
				_experience.Value = value;
				if (LevelProgress == 1)
				{
					_level.Value++;
				}
			}
		}

		public float LevelProgress
		{
			get
			{
				//todo: will break when Level == experiencePerLevel.Count -> actually we need to move experiencePerLevel elsewhere
				float levelBase = GameConsts.TRAINING_EXP_PER_LEVEL[_level.Value - 1];
				float levelTop = GameConsts.TRAINING_EXP_PER_LEVEL[_level.Value];
				return (_experience.Value - levelBase) / (levelTop - levelBase);
			}
		}

		[JsonIgnore]
		public abstract int TotaModifier { get; }

		#endregion
	}
}