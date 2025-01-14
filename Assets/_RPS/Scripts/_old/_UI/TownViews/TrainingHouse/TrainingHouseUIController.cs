using System.Collections.Generic;
using System.Linq;
using Doozy.Runtime.Signals;
using Doozy.Runtime.UIManager;
using Doozy.Runtime.UIManager.Components;
using Doozy.Runtime.UIManager.Containers;
using Kapibara.RPS.Extensions;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Kapibara.RPS.OLD {
    public class TrainingHouseUIController : UIController<TrainingHouseManagerRefactored> {
        #region FIELDS

        [FoldoutGroup("UI")] [FoldoutGroup("UI/Buttons")] [ValidateInput("@buttons != null")] [ValidateInput("@buttons.Count != 0")] [SerializeField]
        protected List<UIButton> buttons;

        [FoldoutGroup("UI/Information")] [ValidateInput("@statInfoView != null")]
        public UIView statInfoView;

        [FoldoutGroup("UI/Information")] [ValidateInput("@infoName != null")]
        public TextMeshProUGUI infoName;

        [FoldoutGroup("UI/Information")] [ValidateInput("@infoIcon != null")]
        public Image infoIcon;

        [FoldoutGroup("UI/Information")] [ValidateInput("@infoBonusText != null")]
        public TextMeshProUGUI infoBonusText;

        [FoldoutGroup("UI/Information")] [ValidateInput("@trainingStatButton != null")]
        public UIButton trainingStatButton;

        [FoldoutGroup("UI/Information")] [ValidateInput("@infoLevelText != null")]
        public TextMeshProUGUI infoLevelText;

        [FoldoutGroup("UI/Information")] [ValidateInput("@infoExpText != null")]
        public TextMeshProUGUI infoExpText;

        [FoldoutGroup("UI/Information")] [ValidateInput("@infoExpScrollbar != null")]
        public Scrollbar infoExpScrollbar;

        [FoldoutGroup("UI/Unlock")] [ValidateInput("@unlockStatView != null")]
        public UIView unlockStatView;

        [FoldoutGroup("UI/Unlock")] [ValidateInput("@unlockName != null")]
        public TextMeshProUGUI unlockName;

        [FoldoutGroup("UI/Unlock")] [ValidateInput("@unlockIcon != null")]
        public Image unlockIcon;

        [FoldoutGroup("UI/Unlock")] [ValidateInput("@unlockPriceText != null")]
        public TextMeshProUGUI unlockPriceText;

        [FoldoutGroup("UI/Unlock")] [ValidateInput("@unlockPriceText != null")]
        public UIButton unlockStatButton;

        #endregion

        #region UNITY_LIFECYCLE

        private void Start() {
            SetButtonsFunctionality();
        }

        #endregion

        #region UI CONTROLLER OVERRIDES

        protected override void LinkManager() {
            Manager.OnTargetStatUnlocked += StatUnlockedUI;
            Manager.OnTargetStatSelected += StatSelectedUI;
        }

        protected override void InitializeUIButtons() {
            Debug.Log("[TrainingHouseUIController] InitializeButtons()");
            for (int i = 0; i < buttons.Count; i++) {
                UIButton button = buttons[i];
                TrainingStat trainingStat = Manager.GetStat(button.Id);
                if (trainingStat != null) {
                    trainingStat.button = button;
                    button.GetComponent<TrainingButton>().SetButtonData(trainingStat);
                    // button.GetComponent<TrainingButton>().SetImage(trainingStat.isAvailable ? trainingStat.icon : null);
                    // button.GetComponent<TrainingButton>().SetLevel(trainingStat.currentLevel);
                    // button.GetComponent<TrainingButton>().SetSelectionOverlay(trainingStat.isTraining);
                }
            }
        }

        #endregion

        #region UI METHODS

        private void SetButtonsFunctionality() {
            for (int i = 0; i < Manager.TrainingStats.Count; i++) {
                TrainingStat trainingStat = Manager.TrainingStats[i];
                trainingStat.button.AddListener(() => { CheckStatAvailability(trainingStat); },
                    true);
            }
        }


        private void CheckStatAvailability(TrainingStat currentTrainingStat) {
            Debug.Log("[TrainingHouseUIController] CheckStatAvailability() -> currentStat: " + currentTrainingStat.stat +
                      " - isUnlocked: " + currentTrainingStat.isAvailable);
            if (currentTrainingStat.isAvailable) {
                SetStatUI(currentTrainingStat);
            } else {
                SetUnlockStatUI(currentTrainingStat);
            }
        }

        private void SetStatUI(TrainingStat currentTrainingStat) {
            //Views
            unlockStatView.Hide();
            statInfoView.Show();
            //UI
            infoName.text = currentTrainingStat.stat.ToString();
            infoIcon.sprite = currentTrainingStat.icon;
            infoLevelText.text = currentTrainingStat.currentLevel.ToString("D2");
            infoBonusText.text = currentTrainingStat.currentBonus.ToString("D2");
            infoExpText.text = "Exp: " + currentTrainingStat.currentExperience.ToString() + "/10";
            infoExpScrollbar.size = ((float) currentTrainingStat.currentExperience) / 10f;
            //Buttons
            trainingStatButton.AddListener(() => { Manager.SetTrainingStat(currentTrainingStat); },
                true);
        }


        private void SetUnlockStatUI(TrainingStat currentTrainingStat) {
            //Views
            unlockStatView.Show();
            statInfoView.Hide();
            //UI
            unlockName.text = currentTrainingStat.stat.ToString();
            unlockIcon.sprite = currentTrainingStat.icon;
            unlockPriceText.text = currentTrainingStat.cost.ToString();
            //Buttons
            unlockStatButton.AddListener(() => { Manager.UnlockTargetStat(currentTrainingStat); },
                true);
        }

        private void StatUnlockedUI(bool isUnlocked, TrainingStat currentTrainingStat) {
            if (isUnlocked) {
                unlockStatView.Hide();
                currentTrainingStat.button.GetComponent<TrainingButton>().SetImage(currentTrainingStat.icon);
                currentTrainingStat.button.GetComponent<TrainingButton>().SetLevel(currentTrainingStat.currentLevel);
                SetStatUI(currentTrainingStat);
            } else {
                //todo: not enough money view or shake effect
            }
        }

        private void StatSelectedUI() {
            for (int i = 0; i < Manager.TrainingStats.Count; i++) {
                TrainingButton button = Manager.TrainingStats[i].button.GetComponent<TrainingButton>();
                button.SetSelectionOverlay(Manager.TrainingStats[i].isTraining);
            }
        }
        #endregion
    }
}