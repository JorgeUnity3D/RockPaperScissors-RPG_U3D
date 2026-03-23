using System;
using System.Collections.Generic;
using System.Linq;

namespace Kapibara.RPS
{
	/// <summary>
	/// Constantes y tablas de datos estáticos del juego: nombres de escenas, precios, modificadores por nivel y experiencia.
	/// </summary>
	public static class GameConsts
	{
        #region SCENES

		/// <summary>Nombre del archivo de escena de introducción.</summary>
		public const string INTRO_SCENE = "00_Intro";
		/// <summary>Nombre del archivo de escena del menú principal.</summary>
		public const string MAIN_MENU_SCENE = "01_MainMenu";
		/// <summary>Nombre del archivo de escena de la ciudad.</summary>
		public const string TOWN_SCENE = "02_Town";
		/// <summary>Nombre del archivo de escena del mapa.</summary>
		public const string MAP_SCENE = "03_Map";
		/// <summary>Nombre del archivo de escena de combate.</summary>
		public const string COMBAT_SCENE = "04_Combat";
		/// <summary>Nombre del archivo de escena de carga.</summary>
		public const string LOADING_SCENE = "05_Loading";

		/// <summary>Mapeo de enum GameScenes a nombre de escena para cargar con SceneManager.</summary>
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

		/// <summary>Mapeo inverso de nombre de escena a enum GameScenes; usado en InitializeScene.</summary>
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

		/// <summary>Mapeo de tipo C# de modificador a su ModifierType enum; usado en constructores de modificadores.</summary>
		public static readonly Dictionary<Type, ModifierType> ATTRIBUTE_TYPE_VALUE = new Dictionary<Type, ModifierType>()
		{
			{ typeof(TrainingHouseModifier), ModifierType.TRAININGHOUSE_MOD },
			{ typeof(PaperTreeModifier), ModifierType.PAPERTREE_MOD },
			{ typeof(ScissorBonfireModifier), ModifierType.SCISSORBONFIRE_MOD },
			{ typeof(LibraryModifier), ModifierType.LIBRARY_MOD}
		};

		//todo: aux code, we should probably find a better place for this data
		/// <summary>Coste en oro para desbloquear el entrenamiento de cada estadística en la Casa de Entrenamiento.</summary>
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
		
		/// <summary>Variaciones de estadísticas por nivel de la Hoguera de las Tijeras. Indexado 0-based; nivel 1 usa índice 1. Solo hay 10 entradas (niveles 1-9 seguros).</summary>
		public static readonly List<ScissorBonfireModLevel> SCISSOR_MODS = new List<ScissorBonfireModLevel>
		{
			new ScissorBonfireModLevel(0, 1, -1, 0, 2, 2, 0, 3, 3),
			new ScissorBonfireModLevel(1, -1, 0, 2, 2, 0, 3, 3, 0),
			new ScissorBonfireModLevel(-1, 0, 1, 2, 0, 2, 3, 0, 3),
			new ScissorBonfireModLevel(0, 2, 2, 0, 1, -1, 0, 3, 3),
			new ScissorBonfireModLevel(2, 3, 0, 1, -1, 0, 3, 2, 0),
			new ScissorBonfireModLevel(2, 0, 2, 3, 0, 1, -1, 0, 3),
			new ScissorBonfireModLevel(0, 3, 2, 0, 2, 3, 0, 1, -1),
			new ScissorBonfireModLevel(3, 3, 0, 2, 2, 0, 1, -1, 0),
			new ScissorBonfireModLevel(2, 0, 3, 2, 0, 2, -1, 0, 1),
			new ScissorBonfireModLevel(1, 0, 2, 3, 0, 3, 2, 0, -1)
		};
		
		/// <summary>Coste en oro para cada subida de nivel de la Hoguera de las Tijeras, indexado desde el nivel actual.</summary>
		public static List<int> LEVEL_PRICES_AUX = new List<int>()
		{
			10, 20, 30, 40, 50, 60, 70, 80, 90, 100
		};
		
		/// <summary>Experiencia acumulada requerida para cada umbral de nivel de entrenamiento/modificador. Índice 0 = nivel 1.</summary>
		public static List<int> TRAINING_EXP_PER_LEVEL = new List<int>()
		{
			0, 10, 20, 30, 40, 50, 60, 70, 80, 90
		};

		/// <summary>Multiplicador de coste de mejora de la Herrería de Piedra. Coste = (nivelActual + 1) × este valor.</summary>
		public const int STONE_SMITHY_COST_PER_LEVEL = 10;

		#endregion

		#region COMIC_PLAYER

		/// <summary>Duración en segundos de la animación de entrada de cada viñeta.</summary>
		public const float COMIC_VIGNETTE_DURATION = 0.4f;

		/// <summary>Distancia en píxeles del desplazamiento inicial en animaciones de slide de viñeta.</summary>
		public const float COMIC_SLIDE_DISTANCE = 200f;

		#endregion
	}
}