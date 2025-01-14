using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Kapibara.RPS
{
	[DefaultExecutionOrder(-50)]
    public class SceneDebugger : MonoBehaviour 
	{
    
        #region UNITY_LIFECYCLE

        private void OnEnable()
        {
	        SceneManager.sceneLoaded += DebugScene;
	        //Subscribe();
        }

        private void OnDestroy()
        {
	        UnSubscribe();
        }

        #endregion
        
        #region SETUP

		protected void Subscribe()
		{
			SceneManager.sceneLoaded += DebugScene;
		}

		protected void UnSubscribe()
		{
			SceneManager.sceneLoaded -= DebugScene;
		}

        #endregion
        
        #region CONTROL

		private void DebugScene(Scene scene, LoadSceneMode loadSceneMode)
		{
			Debug.Log($"[SceneDebugger] DebugScene() -> \n ---SCENE {scene}---");
		}

        #endregion
    }
}