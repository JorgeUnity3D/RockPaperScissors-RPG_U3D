using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS {
    [Serializable]
    [CreateAssetMenu(fileName = "Enemy", menuName = "RPSRPG/Enemy")]
    public class EnemyDataObject : SerializedScriptableObject {
        [HideReferenceObjectPicker] [HideLabel]
        public Enemy data;
    }
}