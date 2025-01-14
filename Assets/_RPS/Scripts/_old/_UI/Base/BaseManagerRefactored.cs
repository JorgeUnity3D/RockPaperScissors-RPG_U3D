using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Kapibara.RPS {
    public class BaseManagerRefactored : SerializedMonoBehaviour {
        private GameManager _gameManager;
        public GameManager GameManager { get; set; }

        

        public virtual void InitializeData() {
            
        }
    }
}