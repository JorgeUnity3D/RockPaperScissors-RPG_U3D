using UnityEngine;

namespace Kapibara.RPS
{
	/// <summary>
	/// Gestiona el loop de un step de Tesoro. Muestra TreasureHUDUIController, espera la acción RPS
	/// del jugador vía PlayerHUDUIController, aplica el oro y dispara OnCombatFinished(true).
	/// </summary>
	public class TreasureStepManager : BaseManager
	{
		private static readonly Actions[] RpsActions = { Actions.ROCK, Actions.PAPER, Actions.SCISSOR };

		private TreasureHUDUIController _treasureHUD;
		private PlayerHUDUIController   _playerHUD;
		private Player                  _player;
		private int                     _goldReward;
		private Actions                 _treasureAction;

		#region SETUP

		public override void SetUp()
		{
			UIService uiService = ServiceLocator.Instance.GetService<UIService>();
			_treasureHUD = uiService.GetController<TreasureHUDUIController>();
			_playerHUD   = uiService.GetController<PlayerHUDUIController>();
			_player      = AppContext.Player;
		}

		protected override void Subscribe()   { }
		protected override void UnSubscribe() { }

		#endregion

		#region CONTROL

		public void Initialize(MapStep step)
		{
			_goldReward     = step.GoldAmount;
			_treasureAction = RpsActions[UnityEngine.Random.Range(0, RpsActions.Length)];

			AppEvents.OnTreasureActionSelected += OnActionSelected;

			_treasureHUD.SetData(step.TreasureSprite, step.GoldAmount);
			_treasureHUD.ShowCanvas();

			_playerHUD.SetTreasureActionsInteractable(true);

			Debug.Log($"[TreasureStepManager] Initialize() -> gold:{_goldReward}  treasureRoll:{_treasureAction}");
		}

		private void OnActionSelected(Actions playerAction)
		{
			AppEvents.OnTreasureActionSelected -= OnActionSelected;

			float multiplier = GetRewardMultiplier(playerAction, _treasureAction);
			int   finalGold  = Mathf.RoundToInt(_goldReward * multiplier);

			_player.Gold += finalGold;
			AppContext.CombatContext?.AddGold(finalGold);
			Debug.Log($"[TreasureStepManager] player:{playerAction}  treasure:{_treasureAction}  x{multiplier}  gold:{finalGold}  total:{_player.Gold}");

			_playerHUD.SetTreasureActionsInteractable(false);
			_treasureHUD.HideCanvas();

			AppEvents.OnGameContextUpdated?.Invoke();
			AppEvents.OnStepFinished?.Invoke(true);
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
