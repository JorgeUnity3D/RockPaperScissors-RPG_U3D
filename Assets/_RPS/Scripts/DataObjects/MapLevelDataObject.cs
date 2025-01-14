using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS {
    [Serializable]
    [CreateAssetMenu(fileName = "MapLevels", menuName = "RPSRPG/MapLevels")]
    public class MapLevelDataObject : SerializedScriptableObject {
        [HideReferenceObjectPicker] [HideLabel]
        public List<MapLevel> data;
    }
}