using Doozy.Runtime.Signals;
using UnityEngine;

namespace Kapibara.RPS {
    public class StablesManagerRefactored : BaseManagerRefactored {

        public bool gamePurchased = false;
        
        public void WatchAd() {
            ServiceLocator.Instance.GetService<CreditsManagerRefactored>().EarnCredit();
        }

        public void BuyGame() {
            if (!gamePurchased) {
                gamePurchased = true;
            }
        }
    }
}