using System;
using Doozy.Runtime.UIManager.Components;
using Kapibara.RPS.Extensions;
using UnityEngine;

namespace Kapibara.RPS
{
    public class MainMenuUIController : BaseUIController
    {
        [Header("Main Menu")]
        [SerializeField] private UIButton _continueGameButton;
        [SerializeField] private UIButton _newGameButton;
        [SerializeField] private UIButton _loadGameButton;
        [SerializeField] private UIButton _optionsButton;

        #region UNITY_LIFECYCLE

        private void Awake()
        {
            SetUp();
        }

        #endregion

        #region SETUP

        public override void SetUp()
        {
	        Debug.Log($"[MainMenuUIController] SetUp() -> ");
	        _continueGameButton.AddListener(ContinueGame);
            _newGameButton.AddListener(NewGameMenu);
            _loadGameButton.AddListener(LoadGameMenu);
        }
        
        #endregion

        #region CONTROL

	    private void ContinueGame()
	    {
		    Debug.Log($"[MainMenuUIController] ContinueGame() -> Invoking: AppEvents.OnContinueGame");
		    AppEvents.OnConfirmContinueGame?.Invoke();
	    }
	    
        void NewGameMenu()
        {
            Debug.Log($"[MainMenuUIController] NewGameMenu() -> Invoking: AppEvents.OnNewGameMenu");
            AppEvents.OnNewGameMenu?.Invoke();
        }

        void LoadGameMenu()
        {
            Debug.Log($"[MainMenuUIController] LoadGameMenu() -> Invoking: AppEvents.OnLoadGameMenu");
            AppEvents.OnLoadGameMenu?.Invoke();
        }
        
        public void EnableContinueButton(bool isEnabled)
        {
	        Debug.Log($"[MainMenuUIController] EnableContinueButton() -> isEnabled {isEnabled}");
	        _continueGameButton.gameObject.SetActive(isEnabled);
        }

        #endregion
	    
    }
}