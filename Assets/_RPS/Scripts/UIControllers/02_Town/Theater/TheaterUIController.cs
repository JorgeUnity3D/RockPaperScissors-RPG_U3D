using Kapibara.UI;
using UnityEngine;

namespace Kapibara.RPS
{
	public class TheaterUIController : UIController
	{
		#region UNITY_LIFECYCLE

		private void Awake()
		{
			SetUp();
		}

		#endregion
		
		#region SETUP

		public override void SetUp()
		{
			Debug.Log($"[TheaterUIController] SetUp() -> ");
			HideCanvas(0);
		}		

		#endregion
		
		#region CONTROL

		

		#endregion
	}
}