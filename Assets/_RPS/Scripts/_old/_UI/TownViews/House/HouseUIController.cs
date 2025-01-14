using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.UIManager.Components;
using Doozy.Runtime.UIManager.Containers;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Kapibara.RPS.OLD {
    public class HouseUIController : UIController<HouseManagerRefactored> {
        #region FIELDS

        [FoldoutGroup("UI")] [FoldoutGroup("UI/HouseStats")] [ValidateInput("@houseStats != null")] [ValidateInput("@houseStats.Count != 0")] [SerializeField]
        protected List<HouseStatHolder> houseStats;

        #endregion

        #region UNITY_LIFECYCLE

        private void Start() {
            SetStatsValuesUI();
        }

        #endregion

        #region UI CONTROLLER OVERRIDES

        protected override void LinkManager() { }

        protected override void InitializeUIButtons() { }

        #endregion

        #region UI METHODS

        private void SetStatsValuesUI() {
            for (int i = 0; i < houseStats.Count; i++) {
                Stats currentStat = houseStats[i].stat;
                int playerStat = Manager.PlayerStat(currentStat);
                int trainingStat = Manager.TrainingStat(currentStat).currentBonus;
                int skillStat = 0;
                int guildStat = 0;

                houseStats[i].SetLevels(playerStat, trainingStat, skillStat, guildStat);
            }
        }

        #endregion
    }
}