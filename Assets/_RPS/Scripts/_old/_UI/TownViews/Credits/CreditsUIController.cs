using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.UIManager.Components;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Kapibara.RPS {
    public class CreditsUIController : UIController<CreditsManagerRefactored> {
        #region FIELDS

        [FoldoutGroup("UI")] [FoldoutGroup("UI/General")] [ValidateInput("@credits != null")] [ValidateInput("@credits.Count != 0")] [SerializeField]
        protected List<Image> credits;

        [FoldoutGroup("UI/General")] [ValidateInput("@unusedCredit != null")] [SerializeField]
        protected Sprite unusedCredit;

        [FoldoutGroup("UI/General")] [ValidateInput("@usedCredit != null")] [SerializeField]
        protected Sprite usedCredit;

        [FoldoutGroup("UI/General")] [ValidateInput("@timeCounterText != null")] [SerializeField]
        protected TextMeshProUGUI timeCounterText;

        #endregion

        #region UNITY_LIFECYCLE

        #endregion

        #region UI CONTROLLER OVERRIDES

        protected override void LinkManager() {
            Manager.OnCreditsUpdated += SetCreditsUI;
            Manager.OnTimeUpdated += SetTimeCounterUI;
        }

        #endregion

        #region UI METHODS

        private void SetCreditsUI(int creditsLeft) {
            Debug.Log("[CreditsUIController] SetCreditsUI() -> creditsLeft: " + creditsLeft);
            for (int i = 0; i < credits.Count; i++) {
                credits[i].sprite = creditsLeft == 0 ? usedCredit : i < creditsLeft ? unusedCredit : usedCredit;
            }

            if (!Manager.CreditsAtMax) {
                Manager.StartTimeCounter();
            }

            timeCounterText.transform.parent.gameObject.SetActive(!Manager.CreditsAtMax);
        }

        private void SetTimeCounterUI(float timeToDisplay) {
            //Debug.Log("[CreditsUIController] SetTimeCounterUI() -> timeToDisplay: " + timeToDisplay);
            TimeSpan t = TimeSpan.FromSeconds(timeToDisplay);
            timeCounterText.text = string.Format("{0:00}:{1:00}:{2:00}", t.Hours, t.Minutes, t.Seconds);
        }

        #endregion
    }
}