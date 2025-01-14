using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kapibara.RPS {
    public class EnemiesManagerOld : BaseManager_old {
        private Dictionary<string, List<Enemy>> _allEnemies;
        private string _currentLevel;

        public List<Enemy> CurrentEnemies {
            get { return _allEnemies[_currentLevel]; }
        }

        #region GAME STATES

        public override void OnInitializeGameEvent() {
            //TODO: READ ENEMIES
            // FillLevelEnemies();
        }

        public override void OnLevelSelectedEvent(Level level) {
            _currentLevel = level.sceneName;
        }

        #endregion

        #region RANDOM ENEMIES

        // public void FillLevelEnemies() {
        //     int maxEnemiesPerZone = 2;
        //     int maxLevels = 7;        
        //     _allEnemies = new Dictionary<string, List<Enemy>>();
        //     List<Enemy> enemies;
        //     for (int i = 0; i < maxLevels; i++) {
        //         int levelIndex = i;
        //         enemies = new List<Enemy>();
        //         for (int j = 0; j < maxEnemiesPerZone; j++) {
        //             string randEne = j % maxEnemiesPerZone == 0 ? "a" : "b";
        //             string path = "enemy_" + randEne + "_" + levelIndex.ToString();
        //             enemies.Add(new Enemy(path, RNGGenerator.RandomBetween(0,2), RNGGenerator.RandomBetween(0, 2), RNGGenerator.RandomBetween(0, 2), RNGGenerator.RandomBetween(0, 2), RNGGenerator.RandomBetween(8, 10)));
        //         }
        //         _allEnemies.Add("Level" + levelIndex.ToString(), enemies);
        //     }
        // }

        #endregion
    }
}