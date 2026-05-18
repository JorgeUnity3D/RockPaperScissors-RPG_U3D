using UnityEngine;

namespace Kapibara.RPS
{
	/// <summary>
	/// Gestiona el loop de un step de Tesoro. Muestra TreasureUIController, espera la acción RPS
	/// del jugador, aplica el oro y dispara OnCombatFinished(true).
	/// </summary>
	public class TreasureStepManager : MonoBehaviour
	{
		private static readonly Actions[] RpsActions = { Actions.ROCK, Actions.PAPER, Actions.SCISSOR };

		private TreasureUIController _treasureUI;
		private Player               _player;
		private int                  _goldReward;
		private Actions              _treasureAction;

		#region UNITY LIFECYCLE

		private void Awake()
		{
			_treasureUI = ServiceLocator.Instance.GetService<UIService>().GetController<TreasureUIController>();
			_player     = AppContext.Player;
		}

		#endregion

		#region CONTROL

		public void Initialize(MapStep step)
		{
			_goldReward     = step.GoldAmount;
			_treasureAction = RpsActions[UnityEngine.Random.Range(0, RpsActions.Length)];

			AppEvents.OnTreasureActionSelected += OnActionSelected;

			_treasureUI.SetData(step.TreasureSprite, step.GoldAmount, _player.CurrentHealth, _player.MaxHealth.TotalValue);
			_treasureUI.SetActionsInteractable(true);
			_treasureUI.ShowCanvas();

			Debug.Log($"[TreasureStepManager] Initialize() -> gold:{_goldReward}  treasureRoll:{_treasureAction}");
		}

		private void OnActionSelected(Actions playerAction)
		{
			AppEvents.OnTreasureActionSelected -= OnActionSelected;

			float multiplier = GetRewardMultiplier(playerAction, _treasureAction);
			int   finalGold  = Mathf.RoundToInt(_goldReward * multiplier);

			_player.Gold += finalGold;
			Debug.Log($"[TreasureStepManager] player:{playerAction}  treasure:{_treasureAction}  x{multiplier}  gold:{finalGold}  total:{_player.Gold}");

			_treasureUI.SetActionsInteractable(false);
			_treasureUI.HideCanvas();

			AppEvents.OnGameContextUpdated?.Invoke();
			AppEvents.OnCombatFinished?.Invoke(true);
		}

		private float GetRewardMultiplier(Actions player, Actions treasure)
		{
			if (player == treasure) return 1.0f;

			bool win = (player == Actions.ROCK    && treasure == Actions.SCISSOR) ||
			           (player == Actions.PAPER   && treasure == Actions.ROCK)    ||
			           (player == Actions.SCISSOR && treasure == Actions.PAPER);

			return win ? 1.2f : 0.8f;
		}

		#endregion
	}
}
