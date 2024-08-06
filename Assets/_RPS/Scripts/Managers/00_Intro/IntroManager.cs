using System;
using System.Collections;
using System.Collections.Generic;
using Kapibara.RPS;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS
{

    public class IntroManager : BaseManager
    {
        [SerializeField, ReadOnly] private UIService _uiService;
        [SerializeField, ReadOnly] private SceneService _sceneService;
        [SerializeField, ReadOnly] private IntroUIController _introUIController;

	#region SETUP

        public override void SetUp()
        {
            Debug.Log($"[IntroManager] SetUp() -> ");
            _uiService = ServiceLocator.Instance.GetService<UIService>();
            _sceneService = ServiceLocator.Instance.GetService<SceneService>();
            _introUIController = _uiService.GetController<IntroUIController>();
        }

        protected override void Subscribe()
        {
            Debug.Log($"[IntroManager] Subscribe() -> ");
            AppEvents.OnIntroCompleted += LoadSceneAfterIntro;
        }

        protected override void UnSubscribe()
        {
            Debug.Log($"[IntroManager] UnSubscribe() -> ");
            AppEvents.OnIntroCompleted -= LoadSceneAfterIntro;
        }

    #endregion

    #region CONTROL

        public override void Initialize()
        {
            Debug.Log($"[IntroManager] Initialize() -> ");
            _introUIController.FadeInIntro();
        }

        private void LoadSceneAfterIntro()
        {
            Debug.Log($"[IntroManager] LoadSceneAfterIntro() -> ");
            _sceneService.LoadScene(GameScenes.MAIN_MENU);
        }

    #endregion
    }
}