using Kapibara.RPS;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Kapibara.Util.NotificableFields
{
	[Serializable]
	public class NotificableField<T>
	{
		[SerializeField] private T _value;
		public event UnityAction<T> OnValueChanged;
		
		public T Value
		{
			get => _value;
			set
			{
				if (!Equals(_value, value))
				{
					_value = value;
					AppEvents.OnGameContextUpdated?.Invoke();
					OnValueChanged?.Invoke(_value);
				}
			}
		}
	}
}