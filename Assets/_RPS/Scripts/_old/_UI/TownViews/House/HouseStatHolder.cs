using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Kapibara.RPS {
    public class HouseStatHolder : MonoBehaviour {
        public Stats stat;
        public TextMeshProUGUI totalLevel;
        public TextMeshProUGUI baseLevel;
        public TextMeshProUGUI trainingLevel;
        public TextMeshProUGUI skilltreeLevel;
        public TextMeshProUGUI guildLevel;

        private GameObject baseLevelLocked;
        private GameObject trainingLevelLocked;
        private GameObject skilltreeLevelLocked;
        private GameObject guildLevelLocked;

        private void Awake() {
            baseLevelLocked = baseLevel.transform.GetChild(0).gameObject;
            trainingLevelLocked = trainingLevel.transform.GetChild(0).gameObject;
            skilltreeLevelLocked = skilltreeLevel.transform.GetChild(0).gameObject;
            guildLevelLocked = guildLevel.transform.GetChild(0).gameObject;

            baseLevelLocked.SetActive(false);
            trainingLevelLocked.SetActive(false);
            skilltreeLevelLocked.SetActive(false);
            guildLevelLocked.SetActive(false);
        }

        public void SetLevels(int baseLvl, int trainingLvl, int skilltreeLvl, int guildLvl) {
            totalLevel.text = (baseLvl + trainingLvl + skilltreeLvl + guildLvl).ToString();

            // baseLevelLocked.gameObject.SetActive(baseLvl == 0);
            // baseLevel.text = baseLvl == 0 ? "" : baseLvl.ToString();
            baseLevel.text = baseLvl.ToString();
            if (trainingLevelLocked != null) {
                trainingLevelLocked.SetActive(trainingLvl == 0);
                trainingLevel.text = trainingLvl == 0 ? "" : trainingLvl.ToString();
            }

            if (skilltreeLevelLocked != null) {
                skilltreeLevelLocked.SetActive(skilltreeLvl == 0);
                skilltreeLevel.text = skilltreeLvl == 0 ? "" : skilltreeLvl.ToString();
            }


            if (guildLevelLocked != null) {
                guildLevelLocked.SetActive(guildLvl == 0);
                guildLevel.text = guildLvl == 0 ? "" : guildLvl.ToString();
            }
        }
    }
}