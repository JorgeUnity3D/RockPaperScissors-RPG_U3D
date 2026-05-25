using System;
using System.ComponentModel;

namespace Kapibara.RPS
{
	/// <summary>Identifica cada escena del juego; usado por SceneService y GameManager para la navegación.</summary>
	public enum GameScenes
	{
		/// <summary>Pantalla de introducción / splash.</summary>
		INTRO = 0,
		/// <summary>Menú principal con opciones de nueva partida y carga.</summary>
		MAIN_MENU = 1,
		/// <summary>Escena de la ciudad con todos los edificios.</summary>
		TOWN = 2,
		/// <summary>Escena de combate (no implementada).</summary>
		COMBAT = 3,
		/// <summary>Pantalla de carga entre escenas.</summary>
		LOAD = 4
	}

	/// <summary>Identifica cada estadística del jugador; usado como clave en diccionarios y modificadores.</summary>
	[Serializable]
	public enum Stats
	{
		/// <summary>Puntos de vida máximos.</summary>
		[Description("Health")] HEALTH = 0,
		/// <summary>Estadística de mentalidad; influye en iniciativa y rollos mentales.</summary>
		[Description("Mental")] MENTALITY = 1,
		/// <summary>Potencia del movimiento Piedra.</summary>
		[Description("Rock")] ROCK = 2,
		/// <summary>Potencia del movimiento Papel.</summary>
		[Description("Paper")] PAPER = 3,
		/// <summary>Potencia del movimiento Tijera.</summary>
		[Description("Scissor")] SCISSOR = 4,
		/// <summary>Reducción de daño recibido.</summary>
		[Description("Defens")] DEFENSE = 5,
		/// <summary>Daño reflejado al atacante.</summary>
		[Description("Thorns")] THORNS = 6,
		/// <summary>Energía inicial al entrar en combate.</summary>
		[Description("Init_E")] INITIAL_ENERGY = 7,
		/// <summary>Energía recuperada por turno.</summary>
		[Description("En_Rec")] ENERGY_RECOVERY = 8,
		/// <summary>Probabilidad de golpe crítico.</summary>
		[Description("Crit")] CRIT = 9,
		/// <summary>Poder especial que se acumula durante el combate.</summary>
		[Description("Sp_Pow")] SUPERPOWER = 10
	}

	/// <summary>Identifica el origen de un modificador; usado por BaseModifierConverter para deserializar el tipo concreto.</summary>
	[Serializable]
	public enum ModifierType
	{
		/// <summary>Modificador procedente de la Casa de Entrenamiento.</summary>
		TRAININGHOUSE_MOD = 0,
		/// <summary>Modificador procedente del árbol Paper Tree.</summary>
		PAPERTREE_MOD = 1,
		/// <summary>Modificador procedente de la Hoguera de las Tijeras.</summary>
		SCISSORBONFIRE_MOD = 2,
		/// <summary>Modificador procedente de la Biblioteca.</summary>
		LIBRARY_MOD = 3
	}

	/// <summary>Identifica cada menú de edificio de la ciudad; usado como clave de navegación y en TownData.</summary>
	[Serializable]
	public enum TownMenu
	{
		/// <summary>Vista principal del mapa de la ciudad.</summary>
		MAIN = 0,
		/// <summary>Biblioteca.</summary>
		LIBRARY = 1,
		/// <summary>Árbol de habilidades Paper Tree.</summary>
		PAPER_TREE = 2,
		/// <summary>Hoguera de las Tijeras (subida de nivel).</summary>
		SCISSORS = 3,
		/// <summary>Establos.</summary>
		STABLES = 4,
		/// <summary>Herrería de Piedra.</summary>
		STONE_SMITHY = 5,
		/// <summary>Teatro.</summary>
		THEATER = 6,
		/// <summary>Casa de Entrenamiento.</summary>
		TRAINING_HOUSE = 7,
		/// <summary>Pantalla de viaje al mapa de niveles.</summary>
		TRAVEL = 8,
		/// <summary>Casa del jugador con resumen de estadísticas.</summary>
		HOUSE = 9,
		/// <summary>Menú de desbloqueo de edificio.</summary>
		UNLOCK = 10
	}

	/// <summary>Identificadores de nodos del árbol de habilidades Paper Tree.</summary>
	[Serializable]
	public enum SkillNode
	{
		R1 = 101, R2 = 102, R3 = 103, R4 = 104, R5 = 105, R6 = 106, R7 = 107,
		P1 = 201, P2 = 202, P3 = 203, P4 = 204, P5 = 205, P6 = 206, P7 = 207,
		S1 = 301, S2 = 302, S3 = 303, S4 = 304, S5 = 305, S6 = 306, S7 = 307,
		D1 = 401, D2 = 402, D3 = 403, D4 = 404, D5 = 405, D6 = 406, D7 = 407,
		E1 = 501, E2 = 502, E3 = 503, E4 = 504, E5 = 505, E6 = 506, E7 = 507,
	}
}