using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS
{
	/// <summary>
	/// Manager de la pantalla de viaje. Proporciona la lista de niveles del mapa al TravelUIController y gestiona la navegación a nivel seleccionado.
	/// </summary>
	public class TravelManager : BaseManager, ITownBuilding
	{
		private MapLevelScrObj _mapLevelScrObj;

		[Header("DEBUG")]
		[SerializeField, ReadOnly] private Player _player;
		[SerializeField, ReadOnly] private TravelUIController _travelUIController;

		#region SETUP

		public override void SetUp()
		{
			Debug.Log($"[TravelManager] SetUp() -> ");
			_mapLevelScrObj = ServiceLocator.Instance.GetService<StaticDataService>().MapLevels;
			_travelUIController = ServiceLocator.Instance.GetService<UIService>().GetController<TravelUIController>();
			_player             = AppContext.Player;
		}

		protected override void Subscribe()
		{
			Debug.Log($"[TravelManager] Subscribe() -> ");
			AppEvents.OnTravelConfirmed += OnTravelConfirmed;
		}

		protected override void UnSubscribe()
		{
			Debug.Log($"[TravelManager] UnSubscribe() -> ");
			AppEvents.OnTravelConfirmed -= OnTravelConfirmed;
		}

		#endregion

		#region CONTROL

		public void OnMenuOpen()
		{
			Debug.Log($"[TravelManager] OnMenuOpen() -> ");
			_travelUIController.SetData(_player.Attributes, _mapLevelScrObj);
		}

		private void OnTravelConfirmed(MapLevel level)
		{
			Debug.Log($"[TravelManager] OnTravelConfirmed() -> {level.Level}.{level.LevelName}");
			AppContext.Player.CurrentEnergy = AppContext.Player.InitialEnergy;
			AppContext.CombatContext = new CombatContext(level);
			ServiceLocator.Instance.GetService<SceneService>().LoadScene(GameScenes.COMBAT);
		}

		#endregion
	}
}
