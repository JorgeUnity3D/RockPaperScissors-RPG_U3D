using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS
{
	public class HouseManager : BaseManager
	{
		[SerializeField, ReadOnly] private Player _player;
		
		[SerializeField, ReadOnly] private HouseUIController _houseUIController;
		
		#region SETUP
		
		public override void SetUp()
		{
			Debug.Log($"[HouseManager] SetUp() -> ");
			_houseUIController = ServiceLocator.Instance.GetService<UIService>().GetController<HouseUIController>();
			_player = AppContext.Player;
		}
		

		protected override void Subscribe()
		{
			Debug.Log($"[HouseManager] Subscribe() ->  Nothing to subscribe!");
		}
		
		protected override void UnSubscribe()
		{
			Debug.Log($"[HouseManager] UnSubscribe() ->  Nothing to unsubscribe!");
		}

        #endregion

        #region CONTROL

		public override void Initialize()
		{
			Debug.Log($"[HouseManager] Initialize() -> ");
			_houseUIController.SetData(_player);
		}
		
		#endregion
	}
}