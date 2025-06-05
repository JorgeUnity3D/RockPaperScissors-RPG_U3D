using System;
using System.Collections.Generic;
using Kapibara.Util.NotificableFields;
using Newtonsoft.Json;
using UnityEngine;

namespace Kapibara.RPS
{
	[Serializable]
	public class StatAttribute
	{
		#region FIELDS

		[SerializeField] private Stats _stat;
		[SerializeField] private NInt _attributeValue;
		[HideInInspector] private NList<BaseModifier> _modifiers;

		#endregion

		#region PROPERTIES

		public Stats Stat
		{
			get => _stat;
			set => _stat = value;
		}

		public int AttributeValue
		{
			get => _attributeValue.Value;
			set => _attributeValue.Value = value;
		}

		public List<BaseModifier> Modifiers
		{
			get => _modifiers.Value;
			set => _modifiers.Value = value;
		}

		public int TotalValue
		{
			get
			{
				int total = _attributeValue.Value;
				foreach (BaseModifier modifier in _modifiers.Value)
				{
					total += modifier.TotaModifier;
				}
				return total;
			}
		}

		#endregion

		#region CONSTRUCTORS

		public StatAttribute(Stats stat, int attributeValue)
		{
			_stat = stat;
			_attributeValue = new NInt(attributeValue);
			_modifiers = new NList<BaseModifier>() { Value = new List<BaseModifier>() };
		}

		[JsonConstructor]
		public StatAttribute(Stats stat, int attributeValue, List<BaseModifier> modifiers = null)
		{
			_stat = stat;
			_attributeValue = new NInt(attributeValue);
			_modifiers = new NList<BaseModifier>() { Value = new List<BaseModifier>(modifiers) };
		}

		#endregion

		#region CONTROL

		public void AddModifier(BaseModifier baseModifier)
		{
			if (_modifiers.Value == null)
			{
				_modifiers.Value = new List<BaseModifier>();
			}
			BaseModifier existingModifier = null;
			foreach (BaseModifier modifier in _modifiers.Value)
			{
				if (modifier.GetType() == baseModifier.GetType())
				{
					existingModifier = modifier;
					break;
				}
			}
			if (existingModifier != null)
			{
				_modifiers.Value.Remove(existingModifier);
			}
			_modifiers.Value.Add(baseModifier);
		}

		public void AddModifiers<T>(List<T> modifiers) where T : BaseModifier
		{
			foreach (T modifier in modifiers)
			{
				AddModifier(modifier);
			}
		}

		public bool RemoveModifier(BaseModifier baseModifier)
		{
			return _modifiers.Value.Remove(baseModifier);
		}

		public T GetModifier<T>() where T : BaseModifier
		{
			T result = null;
			foreach (BaseModifier modifier in _modifiers.Value)
			{
				if (modifier is T specificType)
				{
					result = specificType;
				}
			}

			return result;
		}

		#endregion
	}
}