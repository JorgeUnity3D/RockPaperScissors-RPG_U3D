using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS
{
	/// <summary>
	/// Manager de los Establos. Gestiona el panel IAP/monetización sobre el sistema de créditos:
	/// WatchAd dispara AppEvents.OnEarnCredit (CreditsTimeCounterManager lo consume) + añade EXP al edificio.
	/// BuyGame es placeholder IAP (Phase 7).
	/// El nivel del edificio y su barra de progreso los muestra InMenuUIController (TownManager).
	/// </summary>
	public class StablesManager : BaseManager
	{
		[Header("DEBUG")]
		[SerializeField, ReadOnly] private StablesUIController _stablesUIController;
		[SerializeField, ReadOnly] private TownData _stablesData;

		#region SETUP

		public override void SetUp()
		{
			Debug.Log($"[StablesManager] SetUp() -> ");
			_stablesUIController = ServiceLocator.Instance.GetService<UIService>().GetController<StablesUIController>();
			_stablesData = AppContext.TownData.Find(t => t.TownMenu == TownMenu.STABLES);
		}

		protected override void Subscribe()
		{
			Debug.Log($"[StablesManager] Subscribe() -> Nothing to subscribe!");
		}

		protected override void UnSubscribe()
		{
			Debug.Log($"[StablesManager] UnSubscribe() -> Nothing to unsubscribe!");
		}

		#endregion

		#region CONTROL

		public override void Initialize()
		{
			Debug.Log($"[StablesManager] Initialize() -> ");
			_stablesUIController.SetData(WatchAd, BuyGame);
		}

		/// <summary>
		/// El jugador ve un anuncio: gana 1 crédito (caballo) vía evento y añade 1 EXP al edificio.
		/// El Slider de progreso en InMenuUIController se actualiza al reabrir el edificio (Phase 6 conectará la actualización reactiva).
		/// </summary>
		private void WatchAd()
		{
			Debug.Log($"[StablesManager] WatchAd() -> ");
			AppEvents.OnEarnCredit?.Invoke();
			AddStablesExp(1);
		}

		/// <summary>
		/// El jugador compra el juego completo (IAP). Placeholder hasta Phase 7.
		/// </summary>
		private void BuyGame()
		{
			Debug.Log($"[StablesManager] BuyGame() -> TODO: IAP not implemented.");
		}

		/// <summary>
		/// Añade EXP al edificio establo, capado al techo del nivel actual.
		/// Level-up del edificio diferido a Phase 6.
		/// </summary>
		private void AddStablesExp(int amount)
		{
			int levelCeiling = GameConsts.TRAINING_EXP_PER_LEVEL[_stablesData.Level];
			int newExp = _stablesData.Experience + amount;
			_stablesData.Experience = newExp < levelCeiling ? newExp : levelCeiling;
			AppEvents.OnGameContextUpdated?.Invoke();
		}

		#endregion
	}
}
