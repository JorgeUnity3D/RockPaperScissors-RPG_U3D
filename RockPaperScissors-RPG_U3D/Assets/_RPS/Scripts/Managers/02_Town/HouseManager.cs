using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS
{
	/// <summary>
	/// Manager de la Casa del jugador. Pasa los datos del jugador al HouseUIController para su visualización.
	/// </summary>
	public class HouseManager : BaseManager, ITownBuilding
	{
		[Header("DEBUG")]
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

		public void OnMenuOpen()
		{
			Debug.Log($"[HouseManager] OnMenuOpen() -> ");
			_houseUIController.SetData(_player);
		}
		
		#endregion
	}
}