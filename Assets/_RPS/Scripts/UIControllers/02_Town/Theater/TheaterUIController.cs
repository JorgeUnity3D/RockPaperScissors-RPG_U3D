using Kapibara.UI;
using UnityEngine;

namespace Kapibara.RPS
{
	public class TheaterUIController : BaseUIController
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