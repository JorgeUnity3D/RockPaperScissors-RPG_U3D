using UnityEngine;

namespace Kapibara.RPS
{
	public class BaseManager : MonoBehaviour
	{
		#region UNITY_LIFECYCLE

		protected virtual void Awake()
		{
			SetUp();
			Subscribe();
		}

		protected virtual void OnDestroy()
		{
			UnSubscribe();
		}

        #endregion

	    #region SETUP

		public virtual void SetUp() { }

		protected virtual void Subscribe() { }

		protected virtual void UnSubscribe() { }

		#endregion

		#region CONTROL

		public virtual void Initialize() { }

		#endregion
	}
}