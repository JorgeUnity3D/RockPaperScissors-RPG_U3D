using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.Signals;
using Doozy.Runtime.UIManager;
using Doozy.Runtime.UIManager.Components;
using Kapibara.RPS.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Kapibara.RPS.OLD {
    [RequireComponent(typeof(StoneSmithyManagerRefactored))]
    public class StoneSmithyUIController : UIController<StoneSmithyManagerRefactored> {
        #region FIELDS

        [FoldoutGroup("UI")] [FoldoutGroup("UI/Buttons")] [SerializeField]
        protected List<UIButton> buyItemButtons;

        #endregion

        #region UNITY_LIFECYCLE

        private void Start() {
            SetButtonsFunctionality();
        }

        #endregion

        #region UI CONTROLLER OVERRIDES

        protected override void LinkManager() { }

        protected override void InitializeUIButtons() {
            Debug.Log("[StoneSmithyUIController] InitializeUIButtons()");
            for (int i = 0; i < buyItemButtons.Count; i++) {
                Item item = Manager.Items[i];
                UIButton buyItemButton = buyItemButtons[i];
                item.button = buyItemButton;
                buyItemButton.GetComponent<StoneSmithyButton>().SetButtonData(item);
            }
        }

        #endregion

        #region UI METHODS

        private void SetButtonsFunctionality() {
            Debug.Log("[StoneSmithyUIController] SetButtonsFunctionality()");
            for (int i = 0; i < buyItemButtons.Count; i++) {
                UIButton buyItemButton = buyItemButtons[i];
                Item item = Manager.Items[i];
                buyItemButton.AddListener(() => { SetItemUI(item); });
            }
        }

        private void SetItemUI(Item item) {
            Debug.Log("[StoneSmithyUIController] SetItemUI() - " + item.itemName);
            for (int i = 0; i < buyItemButtons.Count; i++) {
                UIButton button = buyItemButtons[i];
                button.GetComponent<StoneSmithyButton>().SetSelectionOverlay(button == item.button);
            }
        }

        #endregion

        // private void DebugBuyPotionClicked() {
        //     Debug.Log("[StoneSmithyUIController] DebugBuyPotionClicked()");
        // }
        //
        // private void DebugBuyShurikenClicked() {
        //     Debug.Log("[StoneSmithyUIController] DebugBuyShurikenClicked()");
        // }
        //
        // private void DebugBuyTorchClicked() {
        //     Debug.Log("[StoneSmithyUIController] DebugBuyTorchClicked()");
        // }
    }
}