using System;
using System.Collections.Generic;
using UnityEngine;

namespace Kapibara.RPS
{
	[Serializable]
	public class NotificableField<T>
	{
		[SerializeField] private T _value;

		public T Value
		{
			get => _value;
			set
			{
				if (!Equals(_value, value))
				{
					_value = value;
					AppEvents.OnGameContextUpdated?.Invoke();
				}
			}
		}
	}
}