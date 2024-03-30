using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS {
    [Serializable]
    [CreateAssetMenu(fileName = "Player", menuName = "RPSRPG/Player")]
    public class PlayerDataObject : SerializedScriptableObject {
        [HideReferenceObjectPicker] [HideLabel]
        public Player data;
    }
}