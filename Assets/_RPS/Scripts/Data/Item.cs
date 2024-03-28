using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.UIManager.Components;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS {
    public class Item {
        public string itemName;
        public ItemType itemType;
        public int cost;
        public bool targetIsPlayer;
        public Stats affectedStat;
        public int amount;
        [PreviewField] [HideLabel]
        public Sprite icon;

        public int level;
        public int currentExp;
        
        [HideInInspector] public UIButton button;
    }
}