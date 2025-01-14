using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kapibara.RPS {
    public class BaseGameManager : MonoBehaviour {
        //Managers references
        private List<BaseManager_old> _managers;
        private Dictionary<Type, BaseManager_old> _managersDictionary;

        public Player Player {
            get { return GetManager<PlayerManagerOld>().player; }
        }

        public virtual void Start() {
            GetManagers();
            //LinkManagersEvents();
            InitializeGame();
        }

        #region GAME FLOW

        public void InitializeGame() {
            foreach (BaseManager_old manager in _managers) {
                manager.OnInitializeGameEvent();
            }
        }

        public void StartGame() {
            foreach (BaseManager_old manager in _managers) {
                manager.OnNewGameEvent();
            }
        }

        public void LoadGame() {
            foreach (BaseManager_old manager in _managers) {
                manager.OnLoadGameEvent();
            }
        }

        public void LevelSelected(Level level) {
            foreach (BaseManager_old manager in _managers) {
                manager.OnLevelSelectedEvent(level);
            }
        }

        public void CloseGame() {
            foreach (BaseManager_old manager in _managers) {
                manager.OnCloseGameEvent();
            }
        }

        #endregion

        #region COMBAT FLOW

        public void CombatPreparationEvent() {
            foreach (BaseManager_old manager in _managers) {
                manager.OnCombatPreparationEvent();
            }
        }

        public void CombatStartsEvent() {
            foreach (BaseManager_old manager in _managers) {
                manager.OnCombatStartsEvent();
            }
        }

        public void CombatNextTurnEvent() {
            foreach (BaseManager_old manager in _managers) {
                manager.OnCombatNextTurnEvent();
            }
        }

        public void EnemyDiesEvent() {
            foreach (BaseManager_old manager in _managers) {
                manager.OnEnemyDiesEvent();
            }
        }

        public void PlayerDiesEvent() {
            foreach (BaseManager_old manager in _managers) {
                manager.OnPlayerDiesEvent();
            }
        }

        #endregion

        public void GetManagers() {
            _managers = new List<BaseManager_old>(GetComponentsInChildren<BaseManager_old>());
            _managersDictionary = new Dictionary<Type, BaseManager_old>();
            foreach (BaseManager_old manager in _managers) {
                _managersDictionary.Add(manager.GetType(), manager);
                manager.GameManager = this;
            }
        }

        public T GetManager<T>() where T : BaseManager_old {
            return _managersDictionary.ContainsKey(typeof(T)) ? (T) _managersDictionary[typeof(T)] : null;
        }
    }
}