using System;
using System.Collections.Generic;
using Kapibara.Util.NotificableFields;
using Newtonsoft.Json;

namespace Kapibara.RPS
{
	/// <summary>
	/// Estado persistente de un edificio de la ciudad: nivel, experiencia, coste de desbloqueo y flags de NPC.
	/// </summary>
	[Serializable]
	public class TownData
	{
		#region FIELDS

		private TownMenu _townMenu;
		private NString _name;
		private NString _message;
		private NInt _level;
		private NInt _experience;
		private NInt _cost;
		private NBool _isUnlocked;
		private NBool _npcUnlocked;
		private NBool _hasLevel;
		private NBool _hasTimeCounter;

		#endregion

		#region PROPERTIES

		/// <summary>Identificador del menú de ciudad al que pertenece este edificio.</summary>
		public TownMenu TownMenu
		{
			get => _townMenu;
			set => _townMenu = value;
		}

		/// <summary>Nombre visible del edificio.</summary>
		public string Name
		{
			get => _name.Value;
			set => _name.Value = value;
		}

		/// <summary>Mensaje descriptivo o diálogo asociado al edificio.</summary>
		public string Message
		{
			get => _message.Value;
			set => _message.Value = value;
		}

		/// <summary>Nivel actual del edificio; afecta a los modificadores que otorga.</summary>
		public int Level
		{
			get => _level.Value;
			set => _level.Value = value;
		}

		/// <summary>Experiencia acumulada en el nivel actual del edificio.</summary>
		public int Experience
		{
			get => _experience.Value;
			set => _experience.Value = value;
		}

		/// <summary>Coste en oro para desbloquear este edificio.</summary>
		public int Cost
		{
			get => _cost.Value;
			set => _cost.Value = value;
		}

		/// <summary>True si el edificio ha sido construido y su menú está accesible.</summary>
		public bool IsUnlocked
		{
			get => _isUnlocked.Value;
			set => _isUnlocked.Value = value;
		}

		/// <summary>True si el NPC de este edificio ha sido rescatado durante un combate. Se activa por evento de nivel.</summary>
		public bool NpcUnlocked
		{
			get => _npcUnlocked.Value;
			set => _npcUnlocked.Value = value;
		}

		/// <summary>True si este edificio tiene sistema de nivel y barra de progresión.</summary>
		public bool HasLevel
		{
			get => _hasLevel.Value;
			set => _hasLevel.Value = value;
		}

		/// <summary>True si este edificio tiene contador de tiempo (ej. regeneración de créditos).</summary>
		public bool HasTimeCounter
		{
			get => _hasTimeCounter.Value;
			set => _hasTimeCounter.Value = value;
		}

		/// <summary>Progreso normalizado [0,1] dentro del nivel actual, para la barra de progresión.</summary>
		public float LevelProgress
		{
			get
			{
				if (Level >= GameConsts.TRAINING_EXP_PER_LEVEL.Count) return 1f;
				float levelBase = GameConsts.TRAINING_EXP_PER_LEVEL[Level - 1];
				float levelTop  = GameConsts.TRAINING_EXP_PER_LEVEL[Level];
				return (Experience - levelBase) / (levelTop - levelBase);
			}
		}
		
		#endregion

		#region CONSTRUCTORS

		[JsonConstructor]
		public TownData(TownMenu townMenu, string name, string message, int level, int experience, int cost, bool isUnlocked,
			bool npcUnlocked, bool hasLevel, bool hasTimeCounter)
		{
			_townMenu = townMenu;
			_name = new NString(name);
			_message = new NString(message);
			_level = new NInt(level);
			_experience = new NInt(experience);
			_cost = new NInt(cost);
			_isUnlocked = new NBool(isUnlocked);
			_npcUnlocked = new NBool(npcUnlocked);
			_hasLevel = new NBool(hasLevel);
			_hasTimeCounter = new NBool(hasTimeCounter);
		}

		public TownData(TownMenu townMenu, bool isUnlocked = false, bool npcUnlocked = false, bool hasLevel = true,
			bool hasTimeCounter = true)
		{
			_townMenu = townMenu;
			_name = new NString(this.TownMenu.ToString());
			_message = new NString($"This is {this.TownMenu.ToString()}");
			_isUnlocked = new NBool(isUnlocked);
			_cost = new NInt(10);
			_npcUnlocked = new NBool(npcUnlocked);
			_hasLevel = new NBool(hasLevel);
			_level = new NInt(1);
			_experience = new NInt(0);
			_hasTimeCounter = new NBool(hasTimeCounter);
		}

		#endregion
	}
}