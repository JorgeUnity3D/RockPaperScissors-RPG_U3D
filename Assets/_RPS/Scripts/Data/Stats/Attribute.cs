using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Serialization;

namespace Kapibara.RPS
{
	[Serializable]
	public class Attribute
	{
		#region FIELDS

		[SerializeField] private NotificableField<int> _attributeValue;
		[SerializeField] private NotificableField<List<BaseModifier>> _modifiers;

		#endregion

		#region PROPERTIES

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
					total = modifier.Apply(total);
				}
				return total;
			}
		}

		#endregion

		#region CONSTRUCTORS

		public Attribute(int attributeValue)
		{
			_attributeValue = new NotificableField<int>() { Value = attributeValue };
			_modifiers = new NotificableField<List<BaseModifier>>(){ Value = new List<BaseModifier>() };
		}

		[JsonConstructor]
		public Attribute(int attributeValue, List<BaseModifier> modifiers)
		{
			_attributeValue = new NotificableField<int>() { Value = attributeValue };
			_modifiers = new NotificableField<List<BaseModifier>>() { Value = new List<BaseModifier>(modifiers) };
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