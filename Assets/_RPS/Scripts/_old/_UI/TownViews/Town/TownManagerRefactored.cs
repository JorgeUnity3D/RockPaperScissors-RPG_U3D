using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.Signals;
using Doozy.Runtime.UIManager;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Kapibara.RPS {
    [RequireComponent(typeof(TownUIController))]
    public class TownManagerRefactored : BaseManagerRefactored {
        [FoldoutGroup("DATA")] [SerializeField] [InlineEditor]
        private TownViewDataObject townViewObject;

        [FoldoutGroup("DEBUG")] private List<TownView> _townViews;

        [HideInInspector] public UnityAction<bool, TownView> OnTargetTownViewUnlocked;

        public List<TownView> TownViews {
            get { return _townViews; }
        }

        public TownView GetTownView(UIButtonId buttonID) {
            return _townViews.Find((tw) => tw.townMenu.ToString() == buttonID.Name);
        }

        public TownView GetTownView(UIViewId viewID) {
            return _townViews.Find((tw) => tw.townMenu.ToString() == viewID.Name);
        }

        public override void InitializeData() {
//            _townViews = new List<TownView>(Instantiate(townViewObject).Data);
        }

        public void UnlockTargetTownView(TownView targetTownView) {
            Debug.Log("[TownManager] UnlockTargetTownView() -> targetTownView: " + targetTownView.name);
            if (ServiceLocator.Instance.GetService<PlayerManagerRefactored>().Player.currentGold >= targetTownView.cost) {
                targetTownView.isUnlocked = true;
                ServiceLocator.Instance.GetService<PlayerManagerRefactored>().UpdateCoins(-targetTownView.cost);
                OnTargetTownViewUnlocked?.Invoke(true, targetTownView);
            } else {
                OnTargetTownViewUnlocked?.Invoke(false, targetTownView);
            }
        }
    }
}