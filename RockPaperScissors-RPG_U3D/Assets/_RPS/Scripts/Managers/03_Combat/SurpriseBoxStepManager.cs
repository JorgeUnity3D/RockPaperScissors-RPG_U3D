using UnityEngine;

namespace Kapibara.RPS
{
	/// <summary>
	/// Gestiona el step de Caja Sorpresa. Elige un efecto aleatorio (HP o energía),
	/// lo aplica al jugador y espera a que pulse Continue en PlayerHUDUIController.
	/// </summary>
	public class SurpriseBoxStepManager : BaseManager
	{
		private SurpriseBoxHUDUIController _surpriseBoxHUD;
		private PlayerHUDUIController      _playerHUD;
		private Player                     _player;

		#region SETUP

		public override void SetUp()
		{
			UIService uiService = ServiceLocator.Instance.GetService<UIService>();
			_surpriseBoxHUD = uiService.GetController<SurpriseBoxHUDUIController>();
			_playerHUD      = uiService.GetController<PlayerHUDUIController>();
			_player         = AppContext.Player;
		}

		protected override void Subscribe()
		{
			AppEvents.OnSurpriseBoxCollected += OnCollected;
		}

		protected override void UnSubscribe()
		{
			AppEvents.OnSurpriseBoxCollected -= OnCollected;
		}

		#endregion

		#region CONTROL

		public override void Initialize()
		{
			bool isHeal = Random.value < 0.5f;
			int  amount;

			if (isHeal)
			{
				amount = Mathf.RoundToInt(_player.MaxHealth.TotalValue * GameConsts.SURPRISE_BOX_HEAL_PERCENT);
				_player.CurrentHealth = Mathf.Min(_player.MaxHealth.TotalValue, _player.CurrentHealth + amount);
			}
			else
			{
				amount = GameConsts.SURPRISE_BOX_ENERGY_AMOUNT;
				_player.CurrentEnergy = Mathf.Min(GameConsts.COMBAT_MAX_ENERGY, _player.CurrentEnergy + amount);
			}

			Debug.Log($"[SurpriseBoxStepManager] Effect: {(isHeal ? "HEAL" : "ENERGY")} +{amount}  HP:{_player.CurrentHealth}/{_player.MaxHealth.TotalValue}  Energy:{_player.CurrentEnergy}/{GameConsts.COMBAT_MAX_ENERGY}");

			_playerHUD.RefreshBars(_player.CurrentHealth, _player.CurrentEnergy);

			if (_surpriseBoxHUD != null)
			{
				_surpriseBoxHUD.SetData(isHeal, amount);
				_surpriseBoxHUD.ShowCanvas();
			}
		}

		private void OnCollected()
		{
			if (_surpriseBoxHUD != null) _surpriseBoxHUD.HideCanvas();
			AppEvents.OnStepFinished?.Invoke(true);
		}

		#endregion
	}
}
