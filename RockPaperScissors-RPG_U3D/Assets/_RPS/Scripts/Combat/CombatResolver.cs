using UnityEngine;

namespace Kapibara.RPS
{
	/// <summary>
	/// Funciones puras de resolución de combate. Sin estado — todo via parámetros.
	/// CombatManager orquesta el loop y llama a estas funciones para ambos lados (jugador y enemigo).
	/// </summary>
	public static class CombatResolver
	{
		/// <summary>Tirada de variabilidad: 1D(level × 0.16 + 4).</summary>
		public static int VariabilityRoll(int level)
		{
			int max = Mathf.FloorToInt(level * 0.16f + 4);
			return RNGGenerator.Roll1D(max);
		}

		/// <summary>
		/// Daño base antes del multiplicador: variabilidad + stat de la acción elegida.
		/// Devuelve 0 para ENERGY y NONE.
		/// </summary>
		public static int DamageRoll(Actions action, int rock, int paper, int scissor, int defense, int level)
		{
			if (action == Actions.ENERGY || action == Actions.NONE)
				return 0;

			int stat;
			switch (action)
			{
				case Actions.ROCK:    stat = rock;    break;
				case Actions.PAPER:   stat = paper;   break;
				case Actions.SCISSOR: stat = scissor; break;
				case Actions.DEFENSE: stat = defense; break;
				default:              stat = 0;        break;
			}
			return VariabilityRoll(level) + stat;
		}

		/// <summary>
		/// Bonus de daño crítico. Devuelve 0 si no hay crit (1D100 > crit).
		/// </summary>
		public static int CritBonus(Actions action, int rock, int paper, int scissor, int defense, int energyRecovery, int crit)
		{
			if (RNGGenerator.Roll1D(100) > crit)
				return 0;

			float stat;
			switch (action)
			{
				case Actions.ROCK:    stat = rock           * 0.5f; break;
				case Actions.PAPER:   stat = paper          * 0.5f; break;
				case Actions.SCISSOR: stat = scissor        * 0.5f; break;
				case Actions.DEFENSE: stat = defense        * 0.5f; break;
				case Actions.ENERGY:  stat = energyRecovery * 0.5f; break;
				default:              stat = 0f;                     break;
			}
			return Mathf.FloorToInt(stat);
		}

		/// <summary>Daño de espinas devuelto al atacante.</summary>
		public static int ThornsRoll(int thorns, float multiplier, int critBonus, int level)
		{
			int roll = Mathf.FloorToInt(multiplier * thorns) + thorns + VariabilityRoll(level);
			if (critBonus > 0)
				roll += Mathf.FloorToInt(thorns * 0.5f);
			return roll;
		}

		/// <summary>
		/// Multiplicador de daño según la comparación de acciones.
		/// Pasa isSuper=true cuando el atacante o el defensor ejecutan un super-ataque (energía = 100).
		/// </summary>
		public static float GetMultiplier(Actions attackerAction, Actions defenderAction,
		                                  bool attackerIsSuper = false, bool defenderIsSuper = false)
		{
			float multiplier = 1f;

			if (defenderAction != attackerAction && defenderAction != Actions.DEFENSE)
			{
				switch (attackerAction)
				{
					case Actions.ROCK:
						if (defenderAction == Actions.SCISSOR || defenderAction == Actions.ENERGY) multiplier = 1.2f;
						else if (defenderAction == Actions.PAPER)                                  multiplier = 0.8f;
						break;
					case Actions.PAPER:
						if (defenderAction == Actions.ROCK || defenderAction == Actions.ENERGY) multiplier = 1.2f;
						else if (defenderAction == Actions.SCISSOR)                              multiplier = 0.8f;
						break;
					case Actions.SCISSOR:
						if (defenderAction == Actions.PAPER || defenderAction == Actions.ENERGY) multiplier = 1.2f;
						else if (defenderAction == Actions.ROCK)                                 multiplier = 0.8f;
						break;
					case Actions.DEFENSE:
						if (defenderAction != Actions.ENERGY) multiplier = 1.5f;
						break;
					case Actions.ENERGY:
						if (defenderAction != Actions.ENERGY) multiplier = 0.8f;
						break;
				}
			}

			if (attackerIsSuper && attackerAction != Actions.ENERGY)
			{
				if (defenderIsSuper && defenderAction != attackerAction
				    && defenderAction != Actions.DEFENSE && defenderAction != Actions.ENERGY)
				{
					switch (attackerAction)
					{
						case Actions.ROCK:
							if      (defenderAction == Actions.SCISSOR) multiplier = 2.2f;
							else if (defenderAction == Actions.PAPER)   multiplier = 1.8f;
							break;
						case Actions.PAPER:
							if      (defenderAction == Actions.ROCK)    multiplier = 2.2f;
							else if (defenderAction == Actions.SCISSOR) multiplier = 1.8f;
							break;
						case Actions.SCISSOR:
							if      (defenderAction == Actions.PAPER) multiplier = 2.2f;
							else if (defenderAction == Actions.ROCK)  multiplier = 1.8f;
							break;
						case Actions.DEFENSE:
							multiplier = 3f;
							break;
					}
				}
				else
				{
					multiplier = 2f;
				}
			}

			return multiplier;
		}
	}
}
