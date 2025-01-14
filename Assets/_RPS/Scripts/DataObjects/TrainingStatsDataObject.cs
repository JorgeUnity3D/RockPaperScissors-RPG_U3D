using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS {
    [CreateAssetMenu(fileName = "Stats", menuName = "RPSRPG/Stats")]
    public class TrainingStatsDataObject : SerializedScriptableObject {
        [HideReferenceObjectPicker] [HideLabel]
        public List<TrainingStat> data;

    }
}