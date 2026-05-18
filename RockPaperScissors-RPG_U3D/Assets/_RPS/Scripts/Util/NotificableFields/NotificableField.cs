using System;
using UnityEngine;
using UnityEngine.Events;

namespace Kapibara.Util.NotificableFields
{
	/// <summary>
	/// Campo observable genérico. Cada escritura en Value dispara OnValueChanged para notificar a la UI.
	/// El guardado a disco es responsabilidad de los managers vía AppEvents.OnGameContextUpdated.
	/// </summary>
	[Serializable]
	public class NotificableField<T>
	{
		[SerializeField] private T _value;
		/// <summary>Evento que se dispara tras cada cambio de valor, pasando el nuevo valor.</summary>
		public event UnityAction<T> OnValueChanged;

		/// <summary>Valor almacenado. Asignarlo notifica a los suscriptores locales si el valor cambia.</summary>
		public T Value
		{
			get => _value;
			set
			{
				if (!Equals(_value, value))
				{
					_value = value;
					OnValueChanged?.Invoke(_value);
				}
			}
		}
	}
}