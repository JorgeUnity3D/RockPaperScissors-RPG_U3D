using System;
using System.Collections.Generic;
using Doozy.Runtime.Signals;
using Doozy.Runtime.UIManager;
using Doozy.Runtime.UIManager.Components;
using Kapibara.RPS.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS.OLD {
    [RequireComponent(typeof(StablesManagerRefactored))]
    public class StablesUIController : UIController<StablesManagerRefactored> {
        #region FIELDS

        [FoldoutGroup("UI")] [FoldoutGroup("UI/Buttons")] [ValidateInput("@watchAdButton != null")] [SerializeField]
        protected UIButton watchAdButton;

        [FoldoutGroup("UI/Buttons")] [ValidateInput("@buyGameButton != null")] [SerializeField]
        protected UIButton buyGameButton;

        #endregion

        #region UNITY_LIFECYCLE

        private void Start() {
            SetButtonsFunctionality();
        }

        #endregion

        #region UI CONTROLLER OVERRIDES

        protected override void InitializeUIButtons() {
            watchAdButton.interactable = !Manager.gamePurchased || !ServiceLocator.Instance.GetService<CreditsManagerRefactored>().CreditsAtMax;
            buyGameButton.interactable = !Manager.gamePurchased;
        }

        #endregion

        #region UI METHODS

        private void SetButtonsFunctionality() {
            watchAdButton.AddListener(Manager.WatchAd);
            buyGameButton.AddListener(Manager.BuyGame);
        }

        #endregion

   }
}