using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Kapibara.RPS {
    public class StoneSmithyButton : MonoBehaviour {
        public Image icon;
        public Image selectionOverlay;
        public TextMeshProUGUI levelText;


        public void SetButtonData(Item item) {
            SetImage(item.icon);
            SetLevel(item.level);
            SetSelectionOverlay(false);
        }

        public void SetImage(Sprite newSprite) {
            icon.sprite = newSprite != null ? newSprite : icon.sprite;
        }

        public void SetLevel(int level) {
            levelText.text = "Lvl. " + level;
        }

        public void SetSelectionOverlay(bool isSelected) {
            selectionOverlay.enabled = isSelected;
        }
    }
}