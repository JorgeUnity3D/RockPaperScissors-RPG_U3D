using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS
{
	[DefaultExecutionOrder(-20)]
	public class ServiceLocator : SingletonMonoBehaviour<ServiceLocator>
	{
		[SerializeField, ReadOnly] private ServiceDictionary _services;

    #region UNITY_LIFECYCE

		protected override void Awake()
		{
			base.Awake();
			_services = new ServiceDictionary();
		}

    #endregion

    #region CONTROL

		public T GetService<T>() where T : Component
		{
			var type = typeof(T);

			if (_services.ContainsKey(type))
			{
				return _services[type] as T;
			}

			Debug.Log($"[ServiceLocator] GetService() -> Error: Service of type {type.Name} not found");
			return null;
		}

		public void SubscribeService<T>(T service) where T : Component
		{
			var type = typeof(T);

			if (!_services.ContainsKey(type))
			{
				_services.Add(type, service);
			}
			else
			{
				//Debug.Log($"[ServiceLocator] SubscribeService() -> Error: Service of type {type.Name} is already registered");
			}
		}

		public void UnsubscribeService<T>() where T : Component
		{
			var type = typeof(T);

			if (_services.ContainsKey(type))
			{
				_services.Remove(type);
			}
			else
			{
				Debug.Log($"[ServiceLocator] UnsubscribeService() -> Error: Service of type {type.Name} not found");

			}
		}

    #endregion
	}
}