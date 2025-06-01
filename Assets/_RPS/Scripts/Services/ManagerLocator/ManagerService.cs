using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS
{
	[DefaultExecutionOrder(-9989)]
	public class ManagerService : ServiceSubscriber<ManagerService>
	{
		[SerializeField, ReadOnly] private List<BaseManager> _managers;
		[SerializeField, ReadOnly] private ManagerDictionary _sceneManagers;

		#region UNITY_LIFECYCLE

		protected override void Awake()
		{
			base.Awake();
			GetManagers();
		}
		
		#endregion

		#region CONTROL

		private void GetManagers()
		{
			Debug.Log($"[ManagerService] GetManagers() -> ");
			_managers = new List<BaseManager>(GetComponentsInChildren<BaseManager>());
			_sceneManagers = new ManagerDictionary();
			foreach (BaseManager sceneManager in _managers) {
				_sceneManagers.Add(sceneManager.GetType(), sceneManager);
			}
		}

		public T GetManager<T>() where T : BaseManager
		{
			Debug.Log($"[ManagerService] GetManager() -> T: {typeof(T).Name}");
			T manager = _sceneManagers.ContainsKey(typeof(T)) ? (T)_sceneManagers[typeof(T)] : null;
			if (manager == null) {
				foreach (KeyValuePair<Type, BaseManager> sceneManager in _sceneManagers) {
					if (typeof(T).IsAssignableFrom(sceneManager.Key)) {
						manager = (T)sceneManager.Value;
					}
				}
			}
			return manager;
		}

		public BaseManager GetManager(TownMenu menu)
		{
			return menu switch
			{
				//TownMenu.LIBRARY => GetManager<LibraryManager>(),
				TownMenu.PAPER_TREE => GetManager<PaperTreeManager>(),
				TownMenu.SCISSORS => GetManager<ScissorBonfireManager>(),
				//TownMenu.STABLES => GetManager<StablesManager>(),
				//TownMenu.STONE_SMITHY => GetManager<StoneSmithyManager>(),
				//TownMenu.THEATER => GetManager<TheaterManager>(),
				TownMenu.TRAINING_HOUSE => GetManager<TrainingHouseManager>(),
				//TownMenu.TRAVEL => GetManager<TravelManager>(),
				TownMenu.HOUSE => GetManager<HouseManager>(),
				_ => null
			};
		}

		#endregion

	}
}