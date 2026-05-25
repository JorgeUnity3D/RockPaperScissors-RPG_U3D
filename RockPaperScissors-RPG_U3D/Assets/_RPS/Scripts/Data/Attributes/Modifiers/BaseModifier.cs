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

		/// <summary>Nivel actual del modificador.</summary>
		public virtual int Level
		{
			get => _level.Value;
			set
			{
				if (value < 1) return;
				_level.Value = value;
			}
		}

		/// <summary>Experiencia acumulada del modificador.</summary>
		public virtual int Experience
		{
			get => _experience.Value;
			set => _experience.Value = value;
		}

		/// <summary>Valor efectivo que este modificador suma al atributo. Implementado por cada subclase.</summary>
		[JsonIgnore]
		public abstract int TotalModifier { get; }

		#endregion
	}
}