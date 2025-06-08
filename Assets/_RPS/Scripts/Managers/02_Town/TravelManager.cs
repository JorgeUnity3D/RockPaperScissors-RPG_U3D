using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

namespace Kapibara.RPS
{
	public class TravelManager : BaseManager
	{
		[Header("DATA")]
		[SerializeField] private MapLevelScrObj _mapLevelScrObj;
		[Header("DEBUG")]
		[SerializeField, ReadOnly] private Player _player;
		[SerializeField, ReadOnly] private TravelUIController _travelUIController;

		#region SETUP

		public override void SetUp()
		{
			Debug.Log($"[TravelManager] SetUp() -> ");
			_travelUIController = ServiceLocator.Instance.GetService<UIService>().GetController<TravelUIController>();
			_player = AppContext.Player;
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

		}

		#endregion
	}
}
