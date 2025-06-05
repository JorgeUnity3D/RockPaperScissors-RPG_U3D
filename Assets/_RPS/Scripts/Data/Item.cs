using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

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
        
        [HideInInspector] public Button button;
    }
}