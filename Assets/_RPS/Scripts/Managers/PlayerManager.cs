using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS
{

	public class PlayerManager : BaseManager
	{
		[SerializeField, ReadOnly] private UIService _uiService;
		#region SETUP
		
		public override void SetUp()
		{
			Debug.Log($"[PlayerManager] SetUp() -> ");
			AppEvents.OnConfirmUnlock += PayUnlockGoldCost;
		}

		protected override void Subscribe()
		{
            Debug.Log($"[PlayerManager] Subscribe() -> ");
		}
		
		protected override void UnSubscribe()
		{
            Debug.Log($"[PlayerManager] UnSubscribe() -> ");
		}

        #endregion

        #region CONTROL

		public override void Initialize()
		{
			Debug.Log($"[PlayerManager] Initialize() -> ");
		}

		private void PayUnlockGoldCost(TownView townView)
		{
			AppContext.Player.currentGold = Mathf.Max(0, AppContext.Player.currentGold - townView.cost);
			
		}
		
        #endregion

		
	}
}