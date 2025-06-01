using System;
using System.Collections.Generic;
using UnityEngine;

namespace Kapibara.RPS
{
	[DefaultExecutionOrder(-9989)]
    public class UIService : ServiceSubscriber<UIService> 
	{
        List<BaseUIController> _uiControllers;
        Dictionary<Type, BaseUIController> _uiControllersDictionary;
        
        #region UNITY_LIFECYCLE

        protected override void Awake()
        {
            base.Awake();
            Initialize();
        }

        #endregion
        
        #region SETUP

        private void Initialize()
        {
            GetControllers();
        }

        #endregion
        
        #region CONTROL

        private void GetControllers() 
        {
            _uiControllers = new List<BaseUIController>(GetComponentsInChildren<BaseUIController>());
            _uiControllersDictionary = new Dictionary<Type, BaseUIController>();
            foreach (BaseUIController manager in _uiControllers) {
                _uiControllersDictionary.Add(manager.GetType(), manager);
            }
        }

        public T GetController<T>() where T : BaseUIController 
        {
            T controller = _uiControllersDictionary.ContainsKey(typeof(T)) ? (T)_uiControllersDictionary[typeof(T)] : null;
            if (controller == null) {
                foreach (var manPair in _uiControllersDictionary) {
                    if (typeof(T).IsAssignableFrom(manPair.Key)) {
                        controller = (T)manPair.Value;
                    }
                }
            }
            return controller;
        }

		public BaseUIController GetController(TownMenu menu)
		{
			return menu switch
			{
				TownMenu.LIBRARY => GetController<LibraryUIController>(),
				TownMenu.PAPER_TREE => GetController<PaperTreeUIController>(),
				TownMenu.SCISSORS => GetController<ScissorsBonfireUIController>(),
				TownMenu.STABLES => GetController<StablesUIController>(),
				TownMenu.STONE_SMITHY => GetController<StoneSmithyUIController>(),
				TownMenu.THEATER => GetController<TheaterUIController>(),
				TownMenu.TRAINING_HOUSE => GetController<TrainingHouseUIController>(),
				TownMenu.TRAVEL => GetController<TravelUIController>(),
				TownMenu.HOUSE => GetController<HouseUIController>(),
				_ => null
			};
		}

		#endregion
	}
}