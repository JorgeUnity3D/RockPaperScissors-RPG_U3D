using Kapibara.RPS;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Kapibara.Util.NotificableFields
{
	/// <summary>
	/// Campo observable genérico. Cada escritura en Value dispara OnGameContextUpdated (guardado a disco) y OnValueChanged.
	/// No asignar en loops o código por frame.
	/// </summary>
	[Serializable]
	public class NotificableField<T>
	{
		[SerializeField] private T _value;
		/// <summary>Evento que se dispara tras cada cambio de valor, pasando el nuevo valor.</summary>
		public event UnityAction<T> OnValueChanged;

		/// <summary>Valor almacenado. Asignarlo desencadena la serialización a disco si el valor cambia.</summary>
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