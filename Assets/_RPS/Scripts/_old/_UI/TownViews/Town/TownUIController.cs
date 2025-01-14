using System.Collections.Generic;
using Doozy.Runtime.UIManager.Components;
using Doozy.Runtime.UIManager.Containers;
using Kapibara.RPS.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Kapibara.RPS.OLD {
    public class TownUIController : UIController<TownManagerRefactored> {
        #region FIELDS

        [SerializeField] protected List<UIButton> buttons;
        [SerializeField] protected List<UIView> views;
        public UIView townGeneralView;
        public UIView timeCounterView;
        public UIView closeMenuView;
        public UIButton closeMenuButton;
        public UIView menuView;
        public UIView menuInfo;
        public Image menuNPCImage;
        public TextMeshProUGUI menuNPCMessage;
        public TextMeshProUGUI menuInfoName;
        public TextMeshProUGUI menuInfoLevelText;
        public Scrollbar menuInfoProgressScrollbar;
        public UIView unlockView;
        public Image unlockImage;
        public TextMeshProUGUI unlockPrice;
        public UIButton unlockButton;
        public UIButton cancelButton;

        #endregion

        #region UNITY_LIFECYCLE

        private void Start() {
            SetButtonsFunctionality();
        }

        #endregion

        #region UI CONTROLLER OVERRIDES

        protected override void LinkManager() {
            Manager.OnTargetTownViewUnlocked += TownViewUnlockedUI;
        }

        protected override void InitializeUIButtons() {
            Debug.Log("[TownUIController] InitializeUIButtons()");
            for (int i = 0; i < buttons.Count; i++) {
                UIButton button = buttons[i];
                TownView townView =
                    Manager.GetTownView(button.Id);
                if (townView == null) continue;
                // townView.button = button;
                // button.GetComponent<Image>().sprite =
                    // townView.isUnlocked ? townView.buildingIcon : townView.notBuiltIcon;
            }
        }

        protected override void InitializeUIViews() {
            Debug.Log("[TownUIController] InitializeUIViews()");
            for (int i = 0; i < buttons.Count; i++) {
                UIView view = views[i];
                TownView townView =
                    Manager.GetTownView(view.Id);
                if (townView == null) continue;
                // townView.view = view;
            }

            menuView.Hide();
        }

        #endregion

        #region UI METHODS

        private void SetButtonsFunctionality() {
            Debug.Log("[TownUIController] SetButtonsFunctionality()");
            for (int i = 0; i < Manager.TownViews.Count; i++) {
                TownView townView = Manager.TownViews[i];
                // townView.button.AddListener(() => { CheckTownMenuAvailability(townView); },
                //     true);
            }
        }

        private void CheckTownMenuAvailability(TownView targetTownView) {
            Debug.Log("[TownUIController] CheckTownMenuAvailability() -> targetTownView: " + targetTownView.name +
                      " - available: " + targetTownView.isUnlocked);
            if (targetTownView.isUnlocked) {
                SetTownMenuUI(targetTownView);
            } else {
                SetUnlockMenuUI(targetTownView);
            }
        }

        private void SetTownMenuUI(TownView targetTownView) {
            Debug.Log("[TownUIController] SetTownMenuUI() -> currentTownView: " + targetTownView.name);
            //Views
            townGeneralView.Hide();
            timeCounterView.Hide();
            closeMenuView.Show();
            menuView.Show();
            menuInfo.Show();
            //targetTownView.view.Show();
            //UI
            if (targetTownView.hasTimeCounter) {
                timeCounterView.Show();
            }

            menuNPCImage.transform.parent.gameObject.SetActive(targetTownView.npc);
            //menuNPCImage.sprite = targetTownView.npcIcon;
            menuNPCMessage.text = targetTownView.message;

            menuInfoName.text = targetTownView.name;

            menuInfoLevelText.transform.parent.gameObject.SetActive(targetTownView.hasLevel);
            menuInfoLevelText.text = targetTownView.level.ToString();
            menuInfoProgressScrollbar.transform.parent.gameObject.SetActive(targetTownView.hasLevel);
            //todo: levelBar
            //Buttons
            closeMenuButton.AddListener(() => { CloseMenuView(targetTownView); }, true);
        }

        private void SetUnlockMenuUI(TownView currentTownView) {
            //Signal.Send(StreamId.RPSViewStream.ShowUnlockUI);
            Debug.Log("[TownUIController] SetUnlockMenuUI() -> currentTownView: " + currentTownView.name);
            unlockView.Show();
            //Manager.TargetTownView = currentTownView;
            //unlockImage.sprite = currentTownView.buildingIcon;
            unlockPrice.text = currentTownView.cost.ToString();
            unlockButton.AddListener(() => {
                    Manager.UnlockTargetTownView(currentTownView);
                    unlockView.Hide();
                },
                true);
            cancelButton.AddListener(() => { unlockView.Hide(); },
                true);
        }

        private void TownViewUnlockedUI(bool isUnlocked, TownView currentTownView) {
            if (isUnlocked) {
                unlockView.Hide();
                //currentTownView.button.GetComponent<Image>().sprite = currentTownView.buildingIcon;
                SetTownMenuUI(currentTownView);
            } else {
                //todo: not enough money view or shake effect
            }
        }

        public void ShowTimeCounter() {
            timeCounterView.Show();
        }

        private void CloseMenuView(TownView currentTownView) {
            //Views
            townGeneralView.Show();
            timeCounterView.Show();
            closeMenuView.Hide();
            //currentTownView.view.Hide();
            menuView.Hide();
            menuInfo.Hide();
        }

        #endregion
    }
}