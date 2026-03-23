using System;
using System.Collections.Generic;
using Kapibara.Util.NotificableFields;
using Newtonsoft.Json;
using UnityEngine;

namespace Kapibara.RPS
{
	/// <summary>
	/// Contiene y serializa la lista de edificios de la ciudad. Envuelto en NList para activar guardado automático.
	/// </summary>
	[Serializable]
	public class TownContext
	{
		#region FIELDS

		[SerializeField] private NList<TownData> _townData;

		#endregion

		#region PROPERTIES

		/// <summary>Lista de todos los edificios de la ciudad con su estado de desbloqueo y progresión.</summary>
		public List<TownData> TownData
		{
			get => _townData.Value;
			set => _townData.Value = value;
		}

		#endregion

		#region CONSTRUCTORS

		public TownContext() { }

		[JsonConstructor]
		public TownContext(List<TownData> townData)
		{
			_townData = new NList<TownData>() { Value = townData ?? new List<TownData>() };
		}

		#endregion
	}
}
