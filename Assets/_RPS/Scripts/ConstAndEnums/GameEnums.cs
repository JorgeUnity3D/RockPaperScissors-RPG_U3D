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
		//Rock-Nodes
		R01,
		R02,
		R03,
		R04,
		R05,
		R06,
		R07,
		R08,
		R09,
		R10,
		R11,
		R12,
		//Paper-Nodes
		P01,
		P02,
		P03,
		P04,
		P05,
		P06,
		P07,
		P08,
		P09,
		P10,
		P11,
		P12,
		//Scissor-Nodes
		S01,
		S02,
		S03,
		S04,
		S05,
		S06,
		S07,
		S08,
		S09,
		S10,
		S11,
		S12,
		//Defense-Nodes
		D01,
		D02,
		D03,
		D04,
		D05,
		D06,
		D07,
		D08,
		D09,
		D10,
		D11,
		D12,
		//EnergyRecovery-Nodes
		E01,
		E02,
		E03,
		E04,
		E05,
		E06,
		E07,
		E08,
		E09,
		E10,
		E11,
		E12
	}
}