using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS
{
	/// <summary>
	/// Manager de la pantalla de viaje. Proporciona la lista de niveles del mapa al TravelUIController y gestiona la navegación a nivel seleccionado.
	/// </summary>
	public class TravelManager : BaseManager
	{
		[Header("DATA")]
		[SerializeField] private MapLevelScrObj _mapLevelScrObj;
		[Header("DEBUG")]
		[SerializeField, ReadOnly] private Player _player;
		[SerializeField, ReadOnly] private TravelUIController _travelUIController;
		[SerializeField, ReadOnly] private CreditsTimeCounterManager _creditsManager;

		#region SETUP

		public override void SetUp()
		{
			Debug.Log($"[TravelManager] SetUp() -> ");
			_travelUIController = ServiceLocator.Instance.GetService<UIService>().GetController<TravelUIController>();
			_creditsManager     = ServiceLocator.Instance.GetService<ManagerService>().GetManager<CreditsTimeCounterManager>();
			_player             = AppContext.Player;
		}

		protected override void Subscribe()
		{
			Debug.Log($"[TravelManager] Subscribe() -> Nothing to subscribe!");
		}

		protected override void UnSubscribe()
		{
			Debug.Log($"[TravelManager] UnSubscribe() -> Nothing to unsubscribe!");
		}

		#endregion

		#region CONTROL

		public override void Initialize()
		{
			Debug.Log($"[TravelManager] Initialize() -> ");
			_travelUIController.SetData(_player.Attributes, _mapLevelScrObj, TravelToLevel);
		}

		private void TravelToLevel(MapLevel level)
		{
			Debug.Log($"[TravelManager] TravelToLevel() -> {level.Level}.{level.LevelName}");

			if (_creditsManager.CreditsLeft <= 0)
			{
				Debug.LogWarning($"[TravelManager] TravelToLevel() -> No credits left.");
				return;
			}

			_creditsManager.UseCredit();
			AppContext.CombatContext = new CombatContext(level);
			ServiceLocator.Instance.GetService<SceneService>().LoadScene(GameScenes.COMBAT);
		}

		#endregion
	}
}
