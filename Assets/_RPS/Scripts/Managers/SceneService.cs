using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Kapibara.RPS
{
    public class SceneService : ServiceSubscriber<SceneService>
    {
        #region CONTROL

        public void LoadScene(GameScenes targetScene)
        {
            Debug.Log($"[SceneService] LoadScene() -> targetScene: {targetScene}");
            StartCoroutine(CRLoadSceneAsync(GameConsts.SceneNames[targetScene]));
        }
        
        private IEnumerator CRLoadSceneAsync(string targetSceneName)
        {
            Debug.Log($"[SceneService] CRLoadSceneAsync() -> targetSceneName: {targetSceneName}");
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(targetSceneName);
            asyncLoad.allowSceneActivation = false;

            while (!asyncLoad.isDone)
            {
                yield return null;
                if (asyncLoad.progress >= 0.9f)
                {
                    asyncLoad.allowSceneActivation = true;
                }
            }
        }
        
        #endregion
    }
}