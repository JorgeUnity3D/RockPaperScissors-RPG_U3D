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
	public class StablesManager : BaseManager, ITownBuilding
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
			Debug.Log($"[StablesManager] Subscribe() -> ");
			AppEvents.OnWatchAd += WatchAd;
			AppEvents.OnBuyGame += BuyGame;
		}

		protected override void UnSubscribe()
		{
			Debug.Log($"[StablesManager] UnSubscribe() -> ");
			AppEvents.OnWatchAd -= WatchAd;
			AppEvents.OnBuyGame -= BuyGame;
		}

		#endregion

		#region CONTROL

		public void OnMenuOpen()
		{
			Debug.Log($"[StablesManager] OnMenuOpen() -> ");
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

		private void AddStablesExp(int amount)
		{
			_stablesData.Experience += amount;
			int maxLevel = GameConsts.TRAINING_EXP_PER_LEVEL.Count - 1;
			while (_stablesData.Level < maxLevel &&
			       _stablesData.Experience >= GameConsts.TRAINING_EXP_PER_LEVEL[_stablesData.Level])
			{
				_stablesData.Level++;
			}
			if (_stablesData.Level >= maxLevel)
				_stablesData.Experience = Mathf.Min(_stablesData.Experience, GameConsts.TRAINING_EXP_PER_LEVEL[maxLevel]);
			AppEvents.OnGameContextUpdated?.Invoke();
			AppEvents.OnBuildingExpUpdated?.Invoke(TownMenu.STABLES);
		}

		#endregion
	}
}
