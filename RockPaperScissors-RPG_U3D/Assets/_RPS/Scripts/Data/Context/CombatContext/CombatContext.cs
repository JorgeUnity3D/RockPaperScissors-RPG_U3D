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
			List<MapStep> steps        = new List<MapStep>();
			bool          treasureUsed = false;

			for (int i = 0; i < 4; i++)
				steps.Add(GenerateCombatOrTreasure(level, ref treasureUsed));

			steps.Add(new MapStep()); // SURPRISE_BOX — step 5

			for (int i = 0; i < 4; i++)
				steps.Add(GenerateCombatOrTreasure(level, ref treasureUsed));

			if (level.Boss != null)
				steps.Add(new MapStep(MapStepType.BOSS, level.Boss));
			else
				Debug.LogError($"[CombatContext] Level '{level.LevelName}' has no boss assigned — boss step skipped.");

			if (!level.IsCompleted)
				steps.Add(new MapStep(level.TargetBuilding, level.NpcSprite, level.NpcDialogueLines));

			return steps;
		}

		private static MapStep GenerateCombatOrTreasure(MapLevel level, ref bool treasureUsed)
		{
			if (level.IsCompleted && !treasureUsed && Random.value < GameConsts.COMBAT_TREASURE_CHANCE)
			{
				treasureUsed = true;
				return new MapStep(level.TreasureGoldAmount, level.TreasureSprite);
			}

			if (level.PossibleEnemies == null || level.PossibleEnemies.Count == 0)
			{
				Debug.LogError($"[CombatContext] Level '{level.LevelName}' has no enemies assigned — cannot generate combat step.");
				return new MapStep(); // SURPRISE_BOX fallback — will be skipped immediately
			}

			EnemyScrObj enemy = level.PossibleEnemies[Random.Range(0, level.PossibleEnemies.Count)];
			return new MapStep(MapStepType.COMBAT, enemy);
		}
	}
}
