using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


namespace Kapibara.RPS {
    public class TrainingButton : MonoBehaviour {
        public Image icon;
        public List<Image> levels;
        public Image selectionOverlay;

        public void SetButtonData(TrainingStat trainingStat) {
            SetImage(trainingStat.isAvailable ? trainingStat.icon : null);
            SetLevel(trainingStat.currentLevel);
            SetSelectionOverlay(trainingStat.isTraining);
        }

        public void SetImage(Sprite newSprite) {
            icon.sprite = newSprite != null ? newSprite : icon.sprite;
        }

        public void SetLevel(int level) {
            for (int i = 0; i < levels.Count; i++) {
                levels[i].color = i < level ? Color.green : Color.white;
            }
        }

        public void SetSelectionOverlay(bool isSelected) {
            selectionOverlay.enabled = isSelected;
        }
    }
}