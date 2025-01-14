using Doozy.Runtime.Signals;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Kapibara.RPS.OLD {
    public class PlayerUIController : UIController<PlayerManagerRefactored> {
        [FoldoutGroup("UI")] [ValidateInput("@coinsText")] [SerializeField]
        private TextMeshProUGUI coinsText;
       
        protected override void LinkManager() {
            Manager.OnCoinsUpdated += UpdateCoinsUI;
        }

        private void UpdateCoinsUI(int coins) {
            coinsText.text = coins.ToString();
        }
    }
}