using System.Collections.Generic;
using Doozy.Runtime.Signals;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS {
    [RequireComponent(typeof(StoneSmithyUIController))]
    public class StoneSmithyManagerRefactored : BaseManagerRefactored {
        [FoldoutGroup("DATA")] [SerializeField] [InlineEditor]
        private List<ItemDataObject> itemDataObjects;

        [FoldoutGroup("DEBUG")] [SerializeField]
        private List<Item> _items;

        public List<Item> Items {
            get {
                return _items;
            }
        }

        public override void InitializeData() {
            _items = new List<Item>();
            for (int i = 0; i < itemDataObjects.Count; i++) {
                _items.Add(Instantiate(itemDataObjects[i]).data);
            }
        }

        public Item GetItem(ItemType itemTypeType) {
            return _items.Find((i) => i.itemType == itemTypeType);
        }

        
    }
}