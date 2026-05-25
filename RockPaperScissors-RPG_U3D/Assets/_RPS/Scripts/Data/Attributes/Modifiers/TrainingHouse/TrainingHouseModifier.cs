using System;
using Kapibara.Util.NotificableFields;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS
{
	/// <summary>
	/// Modificador de entrenamiento de un atributo. Solo aporta su bonificación si está desbloqueado (IsUnlocked).
	/// </summary>
	[Serializable]
	public class TrainingHouseModifier : BaseModifier
	{
		private NBool _isUnlocked;
		private NBool _isTraining;

		/// <summary>True si el jugador ha pagado para desbloquear el entrenamiento de esta estadística.</summary>
		public bool IsUnlocked
		{
			get => _isUnlocked.Value;
			set => _isUnlocked.Value = value;
		}

		/// <summary>True si esta estadística es la seleccionada actualmente para ganar experiencia de entrenamiento.</summary>
		public bool IsTraining
		{
			get => _isTraining.Value;
			set => _isTraining.Value = value;
		}

		/// <summary>Nivel actual; al asignarlo sincroniza la experiencia al umbral del nivel.</summary>
		public override int Level
		{
			get => _level.Value;
			set
			{
				if (value < 1) return;
				_level.Value = value;
				_experience.Value = GameConsts.TRAINING_EXP_PER_LEVEL[_level.Value - 1];
			}
		}

		/// <summary>Experiencia acumulada; sube de nivel automáticamente al llegar al umbral.</summary>
		public override int Experience
		{
			get => _experience.Value;
			set
			{
				_experience.Value = value;
				while (_level.Value < GameConsts.TRAINING_EXP_PER_LEVEL.Count - 1 && LevelProgress >= 1f)
					_level.Value++;
			}
		}

		/// <summary>Progreso normalizado [0,1] dentro del nivel actual.</summary>
		public float LevelProgress
		{
			get
			{
				float levelBase = GameConsts.TRAINING_EXP_PER_LEVEL[_level.Value - 1];
				float levelTop  = GameConsts.TRAINING_EXP_PER_LEVEL[_level.Value];
				return (_experience.Value - levelBase) / (levelTop - levelBase);
			}
		}

		[JsonIgnore]
		public override int TotalModifier
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