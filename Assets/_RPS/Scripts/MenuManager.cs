using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Kapibara.RPS {
    public class MenuManager : MonoBehaviour {
        public Button startButton;
        public Button optionsButton;
        public Button creditsButton;
        public Button exitButton;

        private void Start() {
            AssignButtonsFunctionality();
        }

        private void AssignButtonsFunctionality() {
            startButton.onClick.AddListener(() => SceneLoaderManager.Instance.PrepareToLoadScene(GameScenes.TOWN));
            //optionsButton.onClick.AddListener(UIManager.Instance.ShowOptions);
            //creditsButton.onClick.AddListener(UIManager.Instance.ShowCredits);
            //exitButton.onClick.AddListener();
        }
    }
}