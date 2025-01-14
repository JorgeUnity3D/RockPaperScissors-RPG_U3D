using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Kapibara.RPS {
    public class MapUIManagerOld : BaseManager_old {
        [Header("Map Canvas")] public GameObject mapCanvas;
        [Header("Level Buttons")] public List<Button> levelButtons;


        #region GAME STATES

        public override void OnInitializeGameEvent() {
            AssignButtonsFunctionality();
        }

        public override void OnLevelSelectedEvent(Level level) {
            mapCanvas.SetActive(false);
        }

        #endregion

        public void AssignButtonsFunctionality() {
            for (int i = 0; i < levelButtons.Count; i++) {
                int levelIndex = i;
                levelButtons[i].onClick.AddListener(() => StartLevel(new Level("Level" + levelIndex.ToString())));
            }
        }


        public void StartLevel(Level level) {
            GameManager.LevelSelected(level);
        }
    }
}