using UnityEngine;

namespace Kapibara.RPS
{
	/// <summary>
	/// Clase base para todos los managers de escena. SetUp() y Subscribe() se llaman en Awake; Initialize() es llamado por GameManager al cargar la escena.
	/// </summary>
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

		/// <summary>Resuelve referencias de servicios y UIControllers. Llamado en Awake antes de Subscribe.</summary>
		public virtual void SetUp() { }

		/// <summary>Registra listeners en AppEvents. Llamado en Awake tras SetUp.</summary>
		protected virtual void Subscribe() { }

		/// <summary>Elimina listeners registrados en Subscribe. Llamado en OnDestroy.</summary>
		protected virtual void UnSubscribe() { }

		#endregion

		#region CONTROL

		/// <summary>Inicializa la lógica de la escena y alimenta la vista con datos. Llamado por GameManager al cargar la escena.</summary>
		public virtual void Initialize() { }

		#endregion
	}
}