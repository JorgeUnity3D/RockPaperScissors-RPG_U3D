using System.Collections.Generic;
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
			List<int> unlocked  = AppContext.GameContext.UnlockedLevelIndexes;
			List<int> completed = AppContext.GameContext.CompletedLevelIndexes;
			foreach (MapLevel level in _mapLevelScrObj.Data)
			{
				level.ResetCompleted();
				if (!level.IsAvailable && unlocked.Contains(level.Level))
					level.SetAvailable();
				if (completed.Contains(level.Level))
					level.SetCompleted();
			}
			_travelUIController.SetData(_player.Attributes, _mapLevelScrObj);
		}

		private void OnTravelConfirmed(MapLevel level)
		{
			Debug.Log($"[TravelManager] OnTravelConfirmed() -> {level.Level}.{level.LevelName}");
			AppContext.Player.CurrentEnergy = Mathf.Min(GameConsts.COMBAT_MAX_ENERGY, AppContext.Player.InitialEnergy.TotalValue);
			AppContext.CombatContext = new CombatContext(level);
			ServiceLocator.Instance.GetService<SceneService>().LoadScene(GameScenes.COMBAT);
		}

		#endregion
	}
}
