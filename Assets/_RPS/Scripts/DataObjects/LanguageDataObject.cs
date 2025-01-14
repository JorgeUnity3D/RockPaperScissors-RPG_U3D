using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS {
    [Serializable]
    [CreateAssetMenu(fileName = "Language", menuName = "RPSRPG/Language")]
    public class LanguageDataObject : SerializedScriptableObject {
        [HideReferenceObjectPicker] [SerializeField] [HideLabel]
        public Language data;
    }
}