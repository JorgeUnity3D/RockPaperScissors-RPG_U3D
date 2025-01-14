using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kapibara.RPS {
    public class BaseManager_old : MonoBehaviour {
        protected BaseGameManager _gameManager;

        public BaseGameManager GameManager {
            get { return _gameManager; }
            set { _gameManager = value; }
        }

        //Game State Events    

        #region GAME STATES

        public virtual void OnInitializeGameEvent() { }
        public virtual void OnNewGameEvent() { }
        public virtual void OnLoadGameEvent() { }
        public virtual void OnCloseGameEvent() { }
        public virtual void OnLevelSelectedEvent(Level level) { }

        #endregion


        //Combate Phases
        public virtual void OnCombatPreparationEvent() { }
        public virtual void OnCombatStartsEvent() { }
        public virtual void OnCombatNextTurnEvent() { }
        public virtual void OnEnemyDiesEvent() { }
        public virtual void OnPlayerDiesEvent() { }


        //public virtual void OnOpenSettingsEvent() { }
        //public virtual void OnNewGameEvent() { }
        //public virtual void OnLoadGameEvent() { }

        //Information exchange events
        //public virtual void OnScoreUpdatedEvent(int updatedScore) { }
        //public virtual void OnLevelTimeUpdatedEvent(int updatedLevel) { }
        //public virtual void OnHighScoreUpdatedEvent(int updatedHighScore) { }
        //public virtual void OnCoinsUpdatedEvent(int updatedCoins) { }
        //public virtual void OnDifficultyUpdatedEvent(int updatedDifficulty) { }
        //public virtual void OnLevelSelectedEvent(int updatedLevel) { }
        //public virtual void OnGameResumedEvent(bool isPlaying) { }
        //public virtual void OnTimeUpdatedEvent(int time) { }
    }

    public class Level {
        public string sceneName;

        public Level(string sceneName) {
            this.sceneName = sceneName;
        }
    }
}