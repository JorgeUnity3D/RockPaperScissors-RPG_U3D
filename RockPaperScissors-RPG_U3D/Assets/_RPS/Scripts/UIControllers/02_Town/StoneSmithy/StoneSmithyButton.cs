using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Kapibara.RPS {
    public class StoneSmithyButton : MonoBehaviour {
        [SerializeField] private Image _icon;
        [SerializeField] private Image _selectionOverlay;
        [SerializeField] private TextMeshProUGUI _levelText;

        public void SetButtonData(Item item) {
            SetImage(item.icon);
            SetLevel(item.level);
            SetSelectionOverlay(false);
        }

        public void SetImage(Sprite newSprite) {
            _icon.sprite = newSprite != null ? newSprite : _icon.sprite;
        }

        public void SetLevel(int level) {
            _levelText.text = "Lvl. " + level;
        }

        public void SetSelectionOverlay(bool isSelected) {
            _selectionOverlay.enabled = isSelected;
        }
    }
}
