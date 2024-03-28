using Doozy.Runtime.Signals;
using Doozy.Runtime.UIManager;
using Doozy.Runtime.UIManager.Components;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS {
    public class TrainingStat {
        [HorizontalGroup("Data", MaxWidth = 200)] [EnumPaging] [HideLabel]
        public Stats stat;

        [HorizontalGroup("Data", MaxWidth = 50)] [PreviewField] [HideLabel]
        public Sprite icon;

        // [HorizontalGroup("UI", MaxWidth = 200)] [EnumPaging] [HideLabel]
        // public UIButtonId.TrainingHouseButtons buttonID;

        [HorizontalGroup("Training", MaxWidth = 50)] [LabelWidth(120)]
        public bool isTraining;

        [HorizontalGroup("Values", MaxWidth = 50)] [LabelWidth(120)]
        public int currentLevel;

        [HorizontalGroup("Values", MaxWidth = 50)] [LabelWidth(120)]
        public int currentExperience;

        // [HorizontalGroup("Values", MaxWidth = 50)] [LabelWidth(120)]
        // public int currentExperience;
        [HorizontalGroup("Values", MaxWidth = 50)] [LabelWidth(120)]
        public int currentBonus;

        [HorizontalGroup("Unlock", MaxWidth = 50)] [LabelWidth(120)]
        public bool isAvailable;

        [HorizontalGroup("Unlock", MaxWidth = 50, MarginLeft = 26)] [LabelWidth(120)]
        public int cost;


        [HideInInspector] public UIButton button;
    }

    public class StatLevel {
        public int level;
        public int levelExp;
        public int levelBonus;
    }
}