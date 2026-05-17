using UnityEngine;

namespace Kapibara.RPS
{
	/// <summary>
	/// Gestiona el loop de un step de Tesoro. Muestra TreasureUIController, espera la acción RPS
	/// del jugador, aplica el oro y dispara OnCombatFinished(true).
	/// </summary>
	public class TreasureStepManager : MonoBehaviour
	{
		private TreasureUIController _treasureUI;
		private Player               _player;
		private int                  _goldReward;

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
			_goldReward = step.GoldAmount;

			AppEvents.OnTreasureActionSelected += OnActionSelected;

			_treasureUI.SetData(step.TreasureSprite, step.GoldAmount, _player.MaxHealth.TotalValue, _player.MaxHealth.TotalValue);
			_treasureUI.SetActionsInteractable(true);
			_treasureUI.ShowCanvas();

			Debug.Log($"[TreasureStepManager] Initialize() -> gold:{_goldReward}");
		}

		private void OnActionSelected(Actions action)
		{
			AppEvents.OnTreasureActionSelected -= OnActionSelected;

			_player.Gold += _goldReward;
			Debug.Log($"[TreasureStepManager] Collected → action:{action}  gold:{_goldReward}  totalGold:{_player.Gold}");

			_treasureUI.SetActionsInteractable(false);
			_treasureUI.HideCanvas();

			AppEvents.OnCombatFinished?.Invoke(true);
		}

		#endregion
	}
}
