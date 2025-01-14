using System;
using System.Collections.Generic;
using System.Linq;
using Doozy.Runtime.UIManager.Components;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS {
    
    public class UIController<K> : SerializedMonoBehaviour where K : BaseManagerRefactored {
        
        [FoldoutGroup("DEBUG")] 
        protected GameManager GameManager {
            get { return managerRefactored.GameManager; }
        }
        
        [FoldoutGroup("DEBUG")] 
        protected K Manager {
            get { return (K) managerRefactored; }
        }
        [FoldoutGroup("DEBUG")]
        protected BaseManagerRefactored managerRefactored;
        
        protected virtual void Awake() {
            // managerRefactored = GetComponent<BaseManagerRefactored>();
            // Manager.InitializeData();
            // LinkManager();
            // InitializeUIButtons();
            // InitializeUIViews();
        }

        protected virtual void OnOpenView() { }
        protected virtual void InitializeUIButtons() { }
        protected virtual void InitializeUIViews() { }
        protected virtual void LinkManager() { }
    }
}