using System;
using Kapibara.Util.Singleton;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS
{
	/// <summary>
	/// Registro central de servicios. Inicializado antes que cualquier otro MonoBehaviour (order -9999). Singleton persistente gestionado por GameCore.
	/// </summary>
	[DefaultExecutionOrder(-9999)]
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

		/// <summary>Devuelve el servicio registrado del tipo T, o null si no está registrado.</summary>
		public T GetService<T>() where T : Component
		{
			Type type = typeof(T);

			if (_services.ContainsKey(type))
			{
				return _services[type] as T;
			}

			Debug.Log($"[ServiceLocator] GetService() -> Error: Service of type {type.Name} not found");
			return null;
		}

		/// <summary>Registra un servicio en el localizador. Llamado automáticamente por ServiceSubscriber en Awake.</summary>
		public void SubscribeService<T>(T service) where T : Component
		{
			Type type = typeof(T);

			if (!_services.ContainsKey(type))
			{
				_services.Add(type, service);
			}
			else
			{
				//Debug.Log($"[ServiceLocator] SubscribeService() -> Error: Service of type {type.Name} is already registered");
			}
		}

		/// <summary>Elimina el registro de un servicio. Llamado automáticamente por ServiceSubscriber en OnDestroy.</summary>
		public void UnsubscribeService<T>() where T : Component
		{
			Type type = typeof(T);

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