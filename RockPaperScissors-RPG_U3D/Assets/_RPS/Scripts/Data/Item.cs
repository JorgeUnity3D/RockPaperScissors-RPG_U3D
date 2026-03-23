using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS
{
	/// <summary>
	/// Datos de un consumible. Los campos de juego se serializan en el save del jugador.
	/// El icono vive en el ScriptableObject (JsonIgnore) y se inyecta en runtime.
	/// </summary>
	[Serializable]
	public class Item
	{
		public string itemName;
		public ItemType itemType;
		public int cost;
		public bool targetIsPlayer;
		public Stats affectedStat;
		/// <summary>Valor que aporta el item en cada nivel. Índice 0 = nivel 1.</summary>
		public List<int> amountsPerLevel;
		public int level;

		[JsonIgnore, PreviewField, HideLabel]
		public Sprite icon;

		/// <summary>Crea una copia independiente. Necesario para no mutar el asset del ScriptableObject.</summary>
		public Item Clone()
		{
			return new Item
			{
				itemName = itemName,
				itemType = itemType,
				cost = cost,
				targetIsPlayer = targetIsPlayer,
				affectedStat = affectedStat,
				amountsPerLevel = amountsPerLevel != null ? new List<int>(amountsPerLevel) : null,
				level = level,
				icon = icon
			};
		}
	}
}
