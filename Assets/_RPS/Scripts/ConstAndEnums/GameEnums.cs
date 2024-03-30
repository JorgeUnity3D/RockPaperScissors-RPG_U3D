using System;
using UnityEngine;

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
	public enum Stat
	{
		ROCK = 0,
		PAPER = 1,
		SCISSORS = 2,
		DEFENSE = 3,
		ENERGY = 4
	}

	[Serializable]
	public enum ModifierType
	{
		TRAINING = 0,
		SKILLTREE = 1
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
}