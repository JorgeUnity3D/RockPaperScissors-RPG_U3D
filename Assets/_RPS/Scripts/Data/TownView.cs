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
	public class TownData
	{
		public TownMenu townMenu;
		[PreviewField] public Sprite buildingIcon;
		[PreviewField] public Sprite notBuiltIcon;
		[PreviewField] public Sprite npcIcon;
		[PreviewField] public Sprite npcNotFoundIcon;
	}

	[Serializable]
	public class TownView
	{
		#region FIELDS

		private TownMenu _townMenu;
		private NotificableField<string> _name;
		private NotificableField<string> _message;
		private NotificableField<int> _level;
		private NotificableField<int> _experience;
		private NotificableField<int> _cost;
		private NotificableField<bool> _isUnlocked;
		private NotificableField<bool> _hasNpc;
		private NotificableField<bool> _npcUnlocked;
		private NotificableField<bool> _hasLevel;
		private NotificableField<bool> _hasTimeCounter;

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
		public TownView(TownMenu townMenu, string name, string message, int level, int experience, int cost, bool isUnlocked, bool hasNpc,
			bool npcUnlocked, bool hasLevel, bool hasTimeCounter)
		{
			_townMenu = townMenu;
			_name = new NotificableField<string> { Value = name };
			_message = new NotificableField<string> { Value = message };
			_level = new NotificableField<int> { Value = level };
			_experience = new NotificableField<int> { Value = experience };
			_cost = new NotificableField<int> { Value = cost };
			_isUnlocked = new NotificableField<bool> { Value = isUnlocked };
			_hasNpc = new NotificableField<bool> { Value = hasNpc };
			_npcUnlocked = new NotificableField<bool> { Value = npcUnlocked };
			_hasLevel = new NotificableField<bool> { Value = hasLevel };
			_hasTimeCounter = new NotificableField<bool> { Value = hasTimeCounter };
		}

		public TownView(TownMenu townMenu, bool isUnlocked = false, bool hasNpc = true, bool npcUnlocked = false, bool hasLevel = true,
			bool hasTimeCounter = true)
		{
			_townMenu = townMenu;
			_name = new NotificableField<string> { Value = this.TownMenu.ToString() };
			_message = new NotificableField<string> { Value = $"This is {this.TownMenu.ToString()}" };
			_isUnlocked = new NotificableField<bool> { Value = isUnlocked };
			_cost = new NotificableField<int> { Value = 10 };
			_hasNpc = new NotificableField<bool> { Value = hasNpc };
			_npcUnlocked = new NotificableField<bool> { Value = npcUnlocked };
			_hasLevel = new NotificableField<bool> { Value = hasLevel };
			_level = new NotificableField<int> { Value = 1 };
			_experience = new NotificableField<int> { Value = 0 };
			_hasTimeCounter = new NotificableField<bool> { Value = hasTimeCounter };
		}

		#endregion
	}
}