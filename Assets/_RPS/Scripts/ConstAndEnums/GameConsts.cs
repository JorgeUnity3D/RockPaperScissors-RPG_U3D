using System;
using System.Collections.Generic;

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
			{ typeof(TrainingModifier), ModifierType.TRAINING },
			{ typeof(SkillTreeModifier), ModifierType.SKILLTREE }
		};

		#endregion
	}
}