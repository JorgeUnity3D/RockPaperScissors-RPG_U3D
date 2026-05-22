using UnityEngine;

namespace Kapibara.RPS
{
	/// <summary>
	/// Regla de comportamiento de un NPC. Compartible entre enemies.
	/// PRIMARY  → obligatorio, cancela action roll y mentality roll.
	/// SECONDARY → cancela action roll; los tertiary pueden sobreescribirlo.
	/// TERTIARY  → solo actúa cuando el enemigo gana el mentality roll (lee la mente del jugador).
	/// </summary>
	[CreateAssetMenu(fileName = "Gambit", menuName = "RPSRPG/Combat/Gambit")]
	public class GambitScrObj : ScriptableObject
	{
		[Header("Type")]
		[SerializeField] private GambitType      _type;

		[Header("Condition")]
		[SerializeField] private GambitCondition _condition;
		[SerializeField] private int             _threshold;
		[SerializeField, Range(0f, 1f)]
		                 private float           _hpPercent;
		[SerializeField] private Actions         _matchAction;

		[Header("Result")]
		[SerializeField] private Actions         _resultAction;

		public GambitType      Type         => _type;
		public Actions         ResultAction => _resultAction;

		/// <summary>
		/// Evalúa si este gambit se activa dado el estado actual de combate.
		/// playerAction solo es relevante para condiciones PLAYER_ACTION_IS / PLAYER_ACTION_IS_NOT.
		/// </summary>
		public bool Evaluate(Enemy enemy, int currentRound, Actions playerAction = Actions.NONE)
		{
			switch (_condition)
			{
				case GambitCondition.ALWAYS:
					return true;

				case GambitCondition.ENERGY_ZERO:
					return enemy.CurrentEnergy == 0;

				case GambitCondition.ENERGY_BELOW:
					return enemy.CurrentEnergy < _threshold;

				case GambitCondition.HP_BELOW_PERCENT:
					return enemy.MaxHealth > 0 && enemy.CurrentHealth < enemy.MaxHealth * _hpPercent;

				case GambitCondition.ROUND_EQUALS:
					return currentRound == _threshold;

				case GambitCondition.ROUND_GE:
					return currentRound >= _threshold;

				case GambitCondition.PLAYER_ACTION_IS:
					return playerAction != Actions.NONE && playerAction == _matchAction;

				case GambitCondition.PLAYER_ACTION_IS_NOT:
					return playerAction != Actions.NONE && playerAction != _matchAction;

				default:
					return false;
			}
		}
	}
}
