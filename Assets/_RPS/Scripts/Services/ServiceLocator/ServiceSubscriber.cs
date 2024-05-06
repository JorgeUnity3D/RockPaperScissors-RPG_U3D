using UnityEngine;

namespace Kapibara.RPS
{
	[DefaultExecutionOrder(-9989)]
	public abstract class ServiceSubscriber<T> : MonoBehaviour where T : MonoBehaviour
	{
		#region UNITY_LIFECYCLE

		protected virtual void Awake()
		{
			//Debug.Log($"[ServiceSubscriber<{(typeof(T)).Name}>] Awake() -> ");
			ServiceLocator.Instance.SubscribeService(this as T);
		}

		protected virtual void OnDestroy()
		{
			//Debug.Log($"[ServiceSubscriber<{(typeof(T)).Name}>] OnDestroy() -> ");
			ServiceLocator.Instance.UnsubscribeService<T>();
		}

        #endregion
	}
}