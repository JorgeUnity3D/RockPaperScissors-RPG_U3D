using UnityEngine;

namespace Kapibara.RPS
{
	public class TheaterManager : BaseManager
	{

		#region SETUP
		
		public override void SetUp()
		{
			Debug.Log($"[TheaterManager] SetUp() -> ");
			
		}
		

		protected override void Subscribe()
		{
			Debug.Log($"[TheaterManager] Subscribe() -> ");
		}
		
		protected override void UnSubscribe()
		{
			Debug.Log($"[TheaterManager] UnSubscribe() -> ");
		}

        #endregion

        #region CONTROL

		public override void Initialize()
		{
			Debug.Log($"[TheaterManager] Initialize() -> ");
			
		}
		
		#endregion
	}
}