using System;
using System.Collections.Generic;
using System.Linq;

namespace Kapibara.RPS
{
	public static class GameConsts
	{
        #region SCENES

		public const string INTRO_SCENE = "00_Intro";
		public const string MAIN_MENU_SCENE = "01_MainMenu";
		public const string TOWN_SCENE = "02_Town";
		public const string MAP_SCENE = "03_Map";
		public const string COMBAT_SCENE = "04_Combat";
		public const string LOADING_SCENE = "05_Loading";

		public static readonly Dictionary<GameScenes, string> SceneNames = new Dictionary<GameScenes, string>()
		{
			{
				GameScenes.INTRO, INTRO_SCENE
			},
			{
				GameScenes.MAIN_MENU, MAIN_MENU_SCENE
			},
			{
				GameScenes.TOWN, TOWN_SCENE
			},
			{
				GameScenes.MAP, MAP_SCENE
			},
			{
				GameScenes.COMBAT, COMBAT_SCENE
			},
			{
				GameScenes.LOAD, LOADING_SCENE
			}
		};

		public static readonly Dictionary<string, GameScenes> SceneEnums = new Dictionary<string, GameScenes>()
		{
			{
				INTRO_SCENE, GameScenes.INTRO
			},
			{
				MAIN_MENU_SCENE, GameScenes.MAIN_MENU
			},
			{
				TOWN_SCENE, GameScenes.TOWN
			},
			{
				MAP_SCENE, GameScenes.MAP
			},
			{
				COMBAT_SCENE, GameScenes.COMBAT
			},
			{
				LOADING_SCENE, GameScenes.LOAD
			}
		};

        #endregion

		#region ATTRIBUTE_MODIFIER

		public static readonly Dictionary<Type, ModifierType> ATTRIBUTE_TYPE_VALUE = new Dictionary<Type, ModifierType>()
		{
			{ typeof(TrainingHouseModifier), ModifierType.TRAININGHOUSE_MOD },
			{ typeof(PaperTreeModifier), ModifierType.PAPERTREE_MOD },
			{ typeof(ScissorBonfireModifier), ModifierType.SCISSORBONFIRE_MOD }
		};

		//todo: aux code, we should probably find a better place for this data
		public static readonly Dictionary<Stats, int> TRAINING_MOD_PRICES = new Dictionary<Stats, int>()
		{
			{ Stats.HEALTH, 10 },
			{ Stats.MENTALITY, 20 },
			{ Stats.ROCK, 30 },
			{ Stats.PAPER, 10 },
			{ Stats.SCISSOR, 20 },
			{ Stats.DEFENSE, 30 },
			{ Stats.THORNS, 10 },
			{ Stats.ENERGY_BASE, 20 },
			{ Stats.ENERGY_RECOVERY, 30 },
			{ Stats.CRIT, 10 },
			{ Stats.SUPERPOWER, 20 }
		};

		//public static readonly List<int> SCISSOR_MOD_PRICES = 
		
		public static readonly List<ScissorBonfireModLevel> SCISSOR_MODS = new List<ScissorBonfireModLevel>
		{
			new ScissorBonfireModLevel(0, 1, -1, 0, 2, -1, 0, 3, -1),
			new ScissorBonfireModLevel(1, -1, 0, 2, -1, 0, 3, -1, 0),
			new ScissorBonfireModLevel(-1, 0, 1, -1, 0, 2, -1, 0, 3),
			new ScissorBonfireModLevel(0, 2, -1, 0, 1, -1, 0, 3, -1),
			new ScissorBonfireModLevel(2, -1, 0, 1, -1, 0, 3, -1, 0),
			new ScissorBonfireModLevel(-1, 0, 2, -1, 0, 1, -1, 0, 3),
			new ScissorBonfireModLevel(0, 3, -1, 0, 2, -1, 0, 1, -1),
			new ScissorBonfireModLevel(3, -1, 0, 2, -1, 0, 1, -1, 0),
			new ScissorBonfireModLevel(-1, 0, 3, -1, 0, 2, -1, 0, 1),
			new ScissorBonfireModLevel(1, 0, -1, 3, 0, -1, 2, 0, -1)
		};
		
		public static List<int> LEVEL_PRICES_AUX = new List<int>()
		{
			10, 20, 30, 40, 50, 60, 70, 80, 90, 100
		};
		
		public static List<int> TRAINING_EXP_PER_LEVEL = new List<int>()
		{
			0, 10, 20, 30, 40, 50, 60, 70, 80, 90
		};

		#endregion
	}
}