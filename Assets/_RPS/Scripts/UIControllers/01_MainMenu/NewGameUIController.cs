using Doozy.Runtime.UIManager.Components;
using Kapibara.RPS.Extensions;
using TMPro;
using UnityEngine;

namespace Kapibara.RPS
{
    public class NewGameUIController : BaseUIController
    {
        [Header("New Game")]
        [SerializeField] private TMP_InputField _gamenameInput;
        [SerializeField] private UIButton _confirmNewGameButton;
        
        [Header("Back Game")]
        [SerializeField] private UIButton _backButton;

        #region UNITY_LIFECYCLE

        private void Awake()
        {
            SetUp();
        }

        #endregion
        
        #region SETUP

        public override void SetUp()
        {
	        Debug.Log($"[NewGameUIController] SetUp() -> ");
            _confirmNewGameButton.AddListener(ConfirmNewGame);
            _gamenameInput.onValueChanged.AddListener(SetNewGameButtonState);
            _backButton.AddListener(Back);
        }
        
        public void Initialize()
        {
	        Debug.Log($"[NewGameUIController] Initialize() -> ");
	        _gamenameInput.text = "";
        }
        
        #endregion
        
        #region CONTROL
        
        private void SetNewGameButtonState(string text)
        {
			Debug.Log($"[NewGameUIController] SetNewGameButtonState() -> ");
            _confirmNewGameButton.interactable = !string.IsNullOrEmpty(_gamenameInput.text);
        }
        
        private void ConfirmNewGame()
        {
			Debug.Log($"[NewGameUIController] ConfirmNewGame() -> Invoking: AppEvents.OnConfirmNewGame({_gamenameInput.text})");
            AppEvents.OnConfirmNewGame?.Invoke(_gamenameInput.text);
        }
        
        private void Back()
        {
			Debug.Log($"[NewGameUIController] Back() -> Invoking: AppEvents.OnBackToMainMenu");
            AppEvents.OnBackToMainMenu?.Invoke();  
        }

        #endregion
        
    }
}