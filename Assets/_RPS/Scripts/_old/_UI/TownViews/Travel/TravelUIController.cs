using System.Collections.Generic;
using Doozy.Runtime.Signals;
using Doozy.Runtime.UIManager;
using Doozy.Runtime.UIManager.Components;
using Kapibara.RPS.Extensions;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Kapibara.RPS.OLD {
    public class TravelUIController : UIController<TravelManagerRefactored> {
        #region FIELDS

        [FoldoutGroup("UI")] [FoldoutGroup("UI/Buttons")] [ValidateInput("@buttons != null")] [ValidateInput("@buttons.Count != 0")] [SerializeField]
        protected List<UIButton> buttons;

        [FoldoutGroup("UI/Buttons")] [ValidateInput("@buttons != null")] [SerializeField]
        protected UIButton goToLevelButton;

        [FoldoutGroup("UI/General")] [ValidateInput("@levelPortraitImage != null")]
        public Image levelPortraitImage;

        [FoldoutGroup("UI/General")] [ValidateInput("@levelNameText != null")]
        public TextMeshProUGUI levelNameText;

        #endregion

        #region UNITY_LIFECYCLE

        private void Start() {
            InitializeUI();
            SetButtonsFunctionality();
        }

        #endregion

        #region UI CONTROLLER OVERRIDES

        protected override void OnOpenView() {
            
        }
        
        protected override void InitializeUIButtons() {
            Debug.Log("[TravelUIController] InitializeUIButtons()");
            for (int i = 0; i < buttons.Count; i++) {
                UIButton button = buttons[i];
                MapLevel mapLevel =
                    Manager.GetMapLevel(button.Id);
                if (mapLevel == null) continue;
                mapLevel.button = button;
            }
        }

        protected override void LinkManager() {
            //Manager.OnLevelSelected += SetUpLevelDetailUI;
            //Manager.OnLevelOpened += OpenLevelUI;
        }

        #endregion

        #region UI METHODS

        private void SetButtonsFunctionality() {
            for (int i = 0; i < Manager.MapLevels.Count; i++) {
                MapLevel mapLevel = Manager.MapLevels[i];
                mapLevel.button.AddListener(() => {
                        if (mapLevel.isAvailable) {
                            SetUpLevelDetailUI(mapLevel);    
                        } else {
                            //todo: shake or something
                            levelPortraitImage.sprite = null;
                            levelNameText.text = "";
                        }
                    },
                    true);
            }
        }

        private void InitializeUI() {
            Debug.Log("[TravelUIController] InitializeUI()");
            levelPortraitImage.sprite = null;
            levelNameText.text = "";
            for (int i = 0; i < Manager.MapLevels.Count; i++) {
                MapLevel mapLevel = Manager.MapLevels[i];
                mapLevel.button.interactable = mapLevel.isAvailable;
            }
        }

        private void SetUpLevelDetailUI(MapLevel currentMapLevel) {
            Debug.Log("[TravelUIController] SetUpLevelDetailUI() -> Manager.TargetMapLevel == null " +
                      (Manager.TargetMapLevel == null).ToString());
            if (currentMapLevel != null) {
                levelPortraitImage.sprite = currentMapLevel.levelPortrait;
                levelNameText.text = currentMapLevel.levelName;
                goToLevelButton.AddListener(() => {
                    if (ServiceLocator.Instance.GetService<CreditsManagerRefactored>().creditsLeft > 0) {
                        ServiceLocator.Instance.GetService<CreditsManagerRefactored>().UseCredit();    
                        //todo: launch level
                    } else {
                        //todo: shake or something
                        levelPortraitImage.sprite = null;
                        levelNameText.text = "";
                    }
                } , true);

            } else {
                levelPortraitImage.sprite = null;
                levelNameText.text = "";
            }
        }

        #endregion
    }
}