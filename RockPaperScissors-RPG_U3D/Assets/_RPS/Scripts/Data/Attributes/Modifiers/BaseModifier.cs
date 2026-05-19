using System;
using Kapibara.Util.NotificableFields;
using Kapibara.Util.Serialization;
using Newtonsoft.Json;
using UnityEngine;

namespace Kapibara.RPS
{
	/// <summary>
	/// Clase base para todos los modificadores de atributos. Define la estadística afectada, nivel y experiencia propios del modificador.
	/// </summary>
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

		/// <summary>Tipo de modificador; usado por el deserializador para reconstruir la subclase correcta.</summary>
		public ModifierType ModifierType { get; set; }

		/// <summary>Estadística del jugador que afecta este modificador.</summary>
		public Stats Stat
		{
			get => _stat;
			set => _stat = value;
		}

		/// <summary>Valor de bonificación base almacenado en el modificador.</summary>
		public int Modifier
		{
			get => _modifier.Value;
			set => _modifier.Value = value;
		}

		/// <summary>Nivel actual del modificador; asignarlo sincroniza la experiencia al umbral del nivel.</summary>
		public int Level
		{
			get => _level.Value;
			set
			{
				if (value < 1) return;
				_level.Value = value;
				_experience.Value = GameConsts.TRAINING_EXP_PER_LEVEL[_level.Value - 1];
			}
		}

		/// <summary>Experiencia acumulada; sube de nivel automáticamente al llegar al umbral. Soporta múltiples level-ups en una sola asignación.</summary>
		public int Experience
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
				float levelTop = GameConsts.TRAINING_EXP_PER_LEVEL[_level.Value];
				return (_experience.Value - levelBase) / (levelTop - levelBase);
			}
		}

		/// <summary>Valor efectivo que este modificador suma al atributo. Implementado por cada subclase.</summary>
		[JsonIgnore]
		public abstract int TotalModifier { get; }

		#endregion
	}
}