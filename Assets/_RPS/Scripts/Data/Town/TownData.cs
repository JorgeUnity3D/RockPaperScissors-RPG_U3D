using System;
using System.Collections.Generic;
using Doozy.Runtime.Signals;
using Doozy.Runtime.UIManager;
using Doozy.Runtime.UIManager.Components;
using Doozy.Runtime.UIManager.Containers;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Kapibara.RPS
{
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
		private NBool _hasNpc;
		private NBool _npcUnlocked;
		private NBool _hasLevel;
		private NBool _hasTimeCounter;

		#endregion

		#region PROPERTIES

		public TownMenu TownMenu
		{
			get => _townMenu;
			set => _townMenu = value;
		}

		public string Name
		{
			get => _name.Value;
			set => _name.Value = value;
		}

		public string Message
		{
			get => _message.Value;
			set => _message.Value = value;
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

		public int Cost
		{
			get => _cost.Value;
			set => _cost.Value = value;
		}

		public bool IsUnlocked
		{
			get => _isUnlocked.Value;
			set => _isUnlocked.Value = value;
		}

		public bool HasNpc
		{
			get => _hasNpc.Value;
			set => _hasNpc.Value = value;
		}
		public bool NpcUnlocked
		{
			get => _npcUnlocked.Value;
			set => _npcUnlocked.Value = value;
		}

		public bool HasLevel
		{
			get => _hasLevel.Value;
			set => _hasLevel.Value = value;
		}

		public bool HasTimeCounter
		{
			get => _hasTimeCounter.Value;
			set => _hasTimeCounter.Value = value;
		}

		public float LevelProgress
		{
			get
			{
				List<int> experiencePerLevel = new List<int>()
				{
					0, 10, 20, 30, 40, 50, 60, 70, 80, 90
				};
				float levelBase = experiencePerLevel[Level - 1];
				float levelTop = experiencePerLevel[Level];
				return (Experience - levelBase) / (levelTop - levelBase);
			}
		}
		
		#endregion

		#region CONSTRUCTORS

		[JsonConstructor]
		public TownData(TownMenu townMenu, string name, string message, int level, int experience, int cost, bool isUnlocked, bool hasNpc,
			bool npcUnlocked, bool hasLevel, bool hasTimeCounter)
		{
			_townMenu = townMenu;
			_name = new NString(name);
			_message = new NString(message);
			_level = new NInt(level);
			_experience = new NInt(experience);
			_cost = new NInt(cost);
			_isUnlocked = new NBool(isUnlocked);
			_hasNpc = new NBool(hasNpc);
			_npcUnlocked = new NBool(npcUnlocked);
			_hasLevel = new NBool(hasLevel);
			_hasTimeCounter = new NBool(hasTimeCounter);
		}

		public TownData(TownMenu townMenu, bool isUnlocked = false, bool hasNpc = true, bool npcUnlocked = false, bool hasLevel = true,
			bool hasTimeCounter = true)
		{
			_townMenu = townMenu;
			_name = new NString(this.TownMenu.ToString());
			_message = new NString($"This is {this.TownMenu.ToString()}");
			_isUnlocked = new NBool(isUnlocked);
			_cost = new NInt(10);
			_hasNpc = new NBool(hasNpc);
			_npcUnlocked = new NBool(npcUnlocked);
			_hasLevel = new NBool(hasLevel);
			_level = new NInt(1);
			_experience = new NInt(0);
			_hasTimeCounter = new NBool(hasTimeCounter);
		}

		#endregion
	}
}