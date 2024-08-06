using System;
using System.ComponentModel;

namespace Kapibara.RPS
{
	public enum GameScenes
	{
		INTRO = 0,
		MAIN_MENU = 1,
		TOWN = 2,
		MAP = 3,
		COMBAT = 4,
		LOAD = 5
	}

	[Serializable]
	public enum Stats
	{
		[Description("Health")] HEALTH = 0,
		[Description("Mental")] MENTALITY = 1,
		[Description("Rock")] ROCK = 2,
		[Description("Paper")] PAPER = 3,
		[Description("Scisso")] SCISSOR = 4,
		[Description("Defens")] DEFENSE = 5,
		[Description("Thorns")] THORNS = 6,
		[Description("En_Bse")] ENERGY_BASE = 7,
		[Description("En_Rec")] ENERGY_RECOVERY = 8,
		[Description("Crit")] CRIT = 9,
		[Description("Sp_Pow")] SUPERPOWER = 10
	}

	[Serializable]
	public enum ModifierType
	{
		TRAININGHOUSE_MOD = 0,
		PAPERTREE_MOD = 1,
		SCISSORBONFIRE_MOD = 2
	}

	[Serializable]
	public enum TownMenu
	{
		MAIN = 0,
		LIBRARY = 1,
		PAPER_TREE = 2,
		SCISSORS = 3,
		STABLES = 4,
		STONE_SMITHY = 5,
		THEATER = 6,
		TRAINING_HOUSE = 7,
		TRAVEL = 8,
		HOUSE = 9,
		UNLOCK = 10
	}

	[Serializable]
	public enum SkillNode
	{
		SKT_01,
		SKT_02,
		SKT_03,
		SKT_04,
		SKT_05,
		SKT_06,
		SKT_07,
		SKT_08,
		SKT_09,
		SKT_10,
		SKT_11,
		SKT_12
	}
}