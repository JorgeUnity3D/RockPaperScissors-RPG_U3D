using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS {
    [Serializable]
    [CreateAssetMenu(fileName = "Item", menuName = "RPSRPG/Item")]
    public class ItemDataObject : SerializedScriptableObject {
        [HideReferenceObjectPicker] [HideLabel]
        public Item data;
    }
}