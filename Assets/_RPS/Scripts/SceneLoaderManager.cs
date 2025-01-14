using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Kapibara.RPS {
    

    public class SceneLoaderManager : MonoBehaviour {
        public Slider progressSlider;
        public string[] sceneNames;
        public float gameLoadTime = 0.75f;
        public string currentSceneToLoad;

        #region UNITY_LIFECYCLE

        //Singleton
        private static SceneLoaderManager _instance;

        [HideInInspector]
        public static SceneLoaderManager Instance {
            get { return _instance; }
        }

        private void Awake() {
            if (_instance != null && _instance != this) {
                _instance.progressSlider = this.progressSlider;
                Destroy(this.gameObject);
            } else {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        private void Start() {
            SceneManager.sceneLoaded += StartLoadingGameScene;
        }

        #endregion

        #region SCENE LOADING

        public void PrepareToLoadScene(GameScenes gameScene) {
            string sceneName = sceneNames[(int) gameScene];
            Debug.Log(sceneName);
        }

        private void StartLoadingGameScene(Scene scene, LoadSceneMode loadSceneMode) {
            if (scene.name != currentSceneToLoad) {
                StartCoroutine(CRLoadGameScene());
            }
        }

        IEnumerator CRLoadGameScene() {
            yield return null;
            progressSlider.value = 0.01f;
            AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(currentSceneToLoad);
            loadingOperation.allowSceneActivation = false;
            float elapsedTime = 0f;
            while (elapsedTime < gameLoadTime && !loadingOperation.isDone) {
                progressSlider.value = elapsedTime / gameLoadTime;
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            //AnalyticsManager.Instance.LogGameOpenEvent();
            loadingOperation.allowSceneActivation = true;
        }

        #endregion
    }
}