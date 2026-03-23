using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Kapibara.RPS
{
    /// <summary>
    /// Servicio de carga de escenas. Carga de forma asíncrona usando SceneManager y activa la escena al llegar al 90%.
    /// </summary>
    public class SceneService : ServiceSubscriber<SceneService>
    {
        #region CONTROL

        /// <summary>Carga la escena correspondiente al enum indicado de forma asíncrona.</summary>
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