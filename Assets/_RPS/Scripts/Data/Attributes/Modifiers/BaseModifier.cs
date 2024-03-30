using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Kapibara.RPS
{
	[Serializable, JsonConverter(typeof(BaseModifierConverter))]
	public abstract class BaseModifier
	{
		#region FIELDS

		[SerializeField] protected NInt _modifier;
		[SerializeField] protected NInt _level;
		[SerializeField] protected NInt _experience;

		#endregion

		#region PROPERTIES
		
		public ModifierType ModifierType { get; set; }
		
		public int Modifier
		{
			get => _modifier.Value;
			set => _modifier.Value = value;
		}
		
		public int Level
		{
			get => _level.Value;
			set => _level.Value = value;
		}
		
		public int Experience
		{
			get => _experience.Value;
			set => _experience.Value = value;
		}

		#endregion
		
		#region CONTROL
		
		public abstract int Apply(int baseValue);
		
		#endregion

	}
}