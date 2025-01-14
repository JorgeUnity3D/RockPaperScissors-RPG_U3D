using System.Collections.Generic;
using Doozy.Runtime.Signals;
using Doozy.Runtime.UIManager;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Kapibara.RPS {
    public class TravelManagerRefactored : BaseManagerRefactored {
        [FoldoutGroup(RPSEditorConst.DATA)] [SerializeField] [InlineEditor]
        private MapLevelDataObject mapLevelDataObject;

        private List<MapLevel> _mapLevels;
        private MapLevel _targetMapLevel;

        [HideInInspector] public UnityAction<MapLevel> OnLevelSelected;
        [HideInInspector] public UnityAction<MapLevel> OnLevelOpened;

        public List<MapLevel> MapLevels {
            get { return _mapLevels; }
        }

        public MapLevel GetMapLevel(int level) {
            return _mapLevels.Find((ml) => ml.level == level);
        }

        public MapLevel GetMapLevel(UIButtonId buttonID) {
            return _mapLevels.Find((ml) => ml.levelButtonID.ToString() == buttonID.Name);
        }

        public MapLevel TargetMapLevel {
            get { return _targetMapLevel; }
        }


        public override void InitializeData() {
            _mapLevels = new List<MapLevel>(Instantiate(mapLevelDataObject).data);
        }

        public void SelectLevel(UIButtonId buttonID) {
            Debug.Log("[TravelManager] SelectLevel() -> streamID: " + buttonID.ToString());
            var currentMapLevel = GetMapLevel(buttonID);
            if (currentMapLevel == null) return;
            _targetMapLevel = _targetMapLevel == currentMapLevel ? null : currentMapLevel;
            OnLevelSelected?.Invoke(_targetMapLevel);
        }
    }
}