using Kapibara.Util.Extensions;
using Kapibara.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Kapibara.RPS
{
    public class NewGameUIController : BaseUIController
    {
        [Header("UI")]
        [Header("New Game")]
        [SerializeField] private TMP_InputField _gamenameInput;
        [SerializeField] private Button _confirmNewGameButton;
        
        [Header("Back Game")]
        [SerializeField] private Button _backButton;

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