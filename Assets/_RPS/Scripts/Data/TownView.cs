using System;
using System.Collections.Generic;
using Doozy.Runtime.Signals;
using Doozy.Runtime.UIManager;
using Doozy.Runtime.UIManager.Components;
using Doozy.Runtime.UIManager.Containers;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;

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
		public TownMenu townMenu;
		public string name;
		public string message;
		public int level;
		public int experience;
		public int cost;
		public bool isUnlocked;
		public bool npc;
		public bool npcUnlocked;
		public bool hasLevel;
		public bool hasTimeCounter;

		public float LevelProgress
		{
			get
			{
				List<int> experiencePerLevel = new List<int>()
				{
					0, 10, 20, 30, 40, 50, 60, 70, 80, 90
				};
				float levelBase = experiencePerLevel[level - 1];
				float levelTop = experiencePerLevel[level];
				return (experience - levelBase) / (levelTop - levelBase);
			}
		}

		[JsonConstructor]
		public TownView(TownMenu townMenu, string name, string message, int level, int experience, int cost, bool isUnlocked, bool npc, bool npcUnlocked, bool hasLevel, bool hasTimeCounter)
		{
			this.townMenu = townMenu;
			this.name = name;
			this.message = message;
			this.level = level;
			this.experience = experience;
			this.cost = cost;
			this.isUnlocked = isUnlocked;
			this.npc = npc;
			this.npcUnlocked = npcUnlocked;
			this.hasLevel = hasLevel;
			this.hasTimeCounter = hasTimeCounter;
		}

		public TownView(TownMenu townMenu, bool isUnlocked = false, bool npc = true, bool npcUnlocked = false, bool hasLevel = true)
		{
			this.townMenu = townMenu;
			this.name = this.townMenu.ToString();
			this.message = $"This is {this.townMenu.ToString()}";
			this.isUnlocked = isUnlocked;
			this.cost = 10;
			this.npc = npc;
			this.npcUnlocked = npcUnlocked;
			this.hasLevel = hasLevel;
			this.level = 1;
			this.experience = 0;
		}
	}
}