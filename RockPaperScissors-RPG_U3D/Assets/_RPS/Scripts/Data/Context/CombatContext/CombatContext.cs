using System.Collections.Generic;
using UnityEngine;

namespace Kapibara.RPS
{
	/// <summary>
	/// Estado transient de la sesión de combate activa. No se serializa a disco.
	/// TravelManager lo crea antes de cargar la escena Combat; StepManager lo consume y avanza CurrentStepIndex.
	/// Los steps se generan dinámicamente en el constructor según el estado del nivel.
	/// </summary>
	public class CombatContext
	{
		public MapLevel      SelectedLevel    { get; private set; }
		public int           CurrentStepIndex { get; set; }
		public List<MapStep> GeneratedSteps   { get; private set; }
		public MapStep       CurrentStep      => GeneratedSteps[CurrentStepIndex];
		public int           StepCount        => GeneratedSteps.Count;
		public int           TotalGoldEarned  { get; private set; }
		public int           TotalTrainingExp { get; private set; }

		public void AddGold(int amount)        { TotalGoldEarned  += amount; }
		public void AddTrainingExp(int amount) { TotalTrainingExp += amount; }

		public CombatContext(MapLevel level)
		{
			SelectedLevel    = level;
			CurrentStepIndex = 0;
			GeneratedSteps   = GenerateSteps(level);
		}

		private static List<MapStep> GenerateSteps(MapLevel level)
		{
			// 8 combat steps; on completed levels, one random position becomes Treasure
			List<MapStep> combatSteps = new List<MapStep>();
			for (int i = 0; i < 8; i++)
				combatSteps.Add(GenerateCombat(level));

			if (level.IsCompleted && level.TreasureGoldAmount > 0)
			{
				int treasureSlot = Random.Range(0, combatSteps.Count);
				combatSteps[treasureSlot] = new MapStep(level.TreasureGoldAmount, level.TreasureSprite);
				Debug.Log($"[CombatContext] Completed level — Treasure placed at combat slot {treasureSlot}");
			}

			List<MapStep> steps = new List<MapStep>();
			for (int i = 0; i < 4; i++) steps.Add(combatSteps[i]);
			steps.Add(new MapStep()); // SURPRISE_BOX — step 5
			for (int i = 4; i < 8; i++) steps.Add(combatSteps[i]);

			if (level.Boss != null)
				steps.Add(new MapStep(MapStepType.BOSS, level.Boss));
			else
				Debug.LogError($"[CombatContext] Level '{level.LevelName}' has no boss assigned — boss step skipped.");

			if (!level.IsCompleted)
				steps.Add(new MapStep(level.TargetBuilding, level.NpcDialogueLines));

			return steps;
		}

		private static MapStep GenerateCombat(MapLevel level)
		{
			if (level.PossibleEnemies == null || level.PossibleEnemies.Count == 0)
			{
				Debug.LogError($"[CombatContext] Level '{level.LevelName}' has no enemies assigned — cannot generate combat step.");
				return new MapStep();
			}
			return new MapStep(MapStepType.COMBAT, level.PossibleEnemies[Random.Range(0, level.PossibleEnemies.Count)]);
		}
	}
}
