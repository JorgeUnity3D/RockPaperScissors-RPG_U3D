using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.Signals;
using Doozy.Runtime.UIManager;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Kapibara.RPS {
//    [RequireComponent(typeof(TrainingHouseUIController))]
    public class TrainingHouseManagerRefactored : BaseManagerRefactored {
        [FoldoutGroup("DATA")] [SerializeField] [InlineEditor]
        private TrainingStatsDataObject trainingStatsDataObject;

        [FoldoutGroup("DEBUG")] [SerializeField]
        private List<TrainingStat> _trainingStats;

        public List<TrainingStat> TrainingStats {
            get { return _trainingStats; }
        }

        [HideInInspector] public UnityAction OnTargetStatSelected;
        [HideInInspector] public UnityAction<bool, TrainingStat> OnTargetStatUnlocked;

        public TrainingStat GetStat(UIButtonId buttonID) {
            return _trainingStats.Find((s) => s.buttonID.ToString() == buttonID.Name);
        }

        public override void InitializeData() {
            _trainingStats = new List<TrainingStat>(Instantiate(trainingStatsDataObject).data);
        }

        public void SetTrainingStat(TrainingStat targetTrainingStat) {
            Debug.Log("[TrainingHouseManager] SetTrainingStat() -> targetStat:" + targetTrainingStat.stat);
            for (int i = 0; i < _trainingStats.Count; i++) {
                TrainingStat trainingStat = _trainingStats[i];
                trainingStat.isTraining = trainingStat.stat == targetTrainingStat.stat;
            }

            OnTargetStatSelected?.Invoke();
        }

        public void UnlockTargetStat(TrainingStat targetTrainingStat) {
            Debug.Log("[TrainingHouseManager] UnlockTargetStat() -> targetStat: " + targetTrainingStat.stat);
            if (ServiceLocator.Instance.GetService<PlayerManagerRefactored>().Player.currentGold >= targetTrainingStat.cost) {
                targetTrainingStat.isAvailable = true;
                ServiceLocator.Instance.GetService<PlayerManagerRefactored>().UpdateCoins(-targetTrainingStat.cost);
                OnTargetStatUnlocked?.Invoke(true, targetTrainingStat);
            } else {
                OnTargetStatUnlocked?.Invoke(false, targetTrainingStat);
            }
        }
    }
}