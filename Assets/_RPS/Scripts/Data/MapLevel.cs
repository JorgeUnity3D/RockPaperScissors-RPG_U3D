using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.Signals;
using Doozy.Runtime.UIManager;
using Doozy.Runtime.UIManager.Components;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS {
    public class MapLevel {
        [FoldoutGroup("LevelInfo")]
        [HorizontalGroup("LevelInfo/Row1", MaxWidth = 50f, Width = 0.1f, PaddingLeft = 0.001f, PaddingRight = 0.005f)]
        [LabelWidth(50f)]
        public int level;

        [FoldoutGroup("LevelInfo")]
        [HorizontalGroup("LevelInfo/Row1", MaxWidth = 200f, Width = 0.2f, PaddingLeft = 0.001f, PaddingRight = 0.005f)]
        [LabelWidth(75f)]
        public string levelName;

        [FoldoutGroup("LevelInfo")]
        [HorizontalGroup("LevelInfo/Row1", MaxWidth = 50f, Width = 0.1f, PaddingLeft = 0.001f, PaddingRight = 0.005f)]
        [LabelWidth(40f)]
        public int steps = 10;

        // [FoldoutGroup("LevelInfo")]
        // [HorizontalGroup("LevelInfo/Row1", MaxWidth = 50f, Width = 0.1f, PaddingLeft = 0.001f, PaddingRight = 0.005f)]
        // [LabelWidth(50f)]
        // public int reward = 10;

        // [FoldoutGroup("LevelInfo")]
        // [HorizontalGroup("LevelInfo/Row2", MaxWidth = 300f, Width = 0.1f, PaddingLeft = 0.001f, PaddingRight = 0.005f)]
        // [LabelWidth(100f)]
        // public UIButtonId.TravelButtons levelButtonID;

        [FoldoutGroup("LevelInfo")]
        [HorizontalGroup("LevelInfo/Row3", MaxWidth = 100F, Width = 0.5f, PaddingLeft = 0.001f, PaddingRight = 0.001f)]
        [VerticalGroup("LevelInfo/Row3/Column1")]
        [HideLabel]
        [PreviewField(Height = 100f)]
        public Sprite levelIcon;

        [FoldoutGroup("LevelInfo")]
        [HorizontalGroup("LevelInfo/Row3", MaxWidth = 100F, Width = 0.5f, PaddingLeft = 0.001f, PaddingRight = 0.001f)]
        [VerticalGroup("LevelInfo/Row3/Column1")]
        [HideLabel]
        [PreviewField(Height = 100f)]
        public Sprite levelPortrait;

        [FoldoutGroup("LevelInfo")]
        [HorizontalGroup("LevelInfo/Row3", MaxWidth = 500f, Width = 0.5f, PaddingLeft = 0.001f, PaddingRight = 0.001f)]
        [InlineEditor]
        [HideLabel]
        public List<EnemyDataObject> levelEnemies;

        [FoldoutGroup("LevelInfo")] [HorizontalGroup("LevelInfo/Row4", MaxWidth = 500f, Width = 0.5f, PaddingLeft = 0.001f, PaddingRight = 0.001f)]
        public bool isAvailable;
        
        [FoldoutGroup("LevelInfo")] [HorizontalGroup("LevelInfo/Row4", MaxWidth = 500f, Width = 0.5f, PaddingLeft = 0.001f, PaddingRight = 0.001f)]
        public bool isSpecialLevel;

        [HideInInspector] public UIButton button;
        

    }
}