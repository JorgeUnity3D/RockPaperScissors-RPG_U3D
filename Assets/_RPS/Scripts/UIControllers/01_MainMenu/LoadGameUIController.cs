using System.Collections.Generic;
using Kapibara.RPS.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Kapibara.RPS
{
	public class LoadGameUIController : BaseUIController
	{
		[Header("UI")]
		[Header("Load Game")]
		[SerializeField] private Transform _gameButtonsHolder;
		[SerializeField] private GameContextButton _gameContextButtonPrefab;
		[SerializeField] private TextMeshProUGUI _timestampText;
		[SerializeField] private TextMeshProUGUI _dateText;
		[SerializeField] private TextMeshProUGUI _gamenameText;
		[SerializeField] private TextMeshProUGUI _playernameText;
		[SerializeField] private Button _loadSelectedButton;
		[SerializeField] private Button _deleteSelectedButton;

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
			Debug.Log($"[LoadGameUIController] SetUp() -> ");			
			_backButton.AddListener(Back);
		}

        #endregion

        #region CONTROL

		public void Initialize()
		{
	        Debug.Log($"[LoadGameUIController] Initialize() -> ");			
			_timestampText.text = "";
			_dateText.text = "";
			_gamenameText.text = "";
			_playernameText.text = "";
			_loadSelectedButton.interactable = false;
			_deleteSelectedButton.interactable = false;
		}

		public void InstanceGames(List<GameContext> gameContexts)
		{
	        Debug.Log($"[LoadGameUIController] InstanceGames() -> ");			
			_gameButtonsHolder.DestroyChildren();
			for (int i = 0; i < gameContexts.Count; i++)
			{
				GameContextButton gameContextButton = Instantiate(_gameContextButtonPrefab, _gameButtonsHolder);
				gameContextButton.SetData(gameContexts[i], SelectGame);
			}
		}
		
		private void SelectGame(GameContext gameContext, Button button)
		{
	        Debug.Log($"[LoadGameUIController] SelectGame() -> ");	
			_timestampText.text = gameContext.Timestamp;
			_dateText.text = gameContext.Date;
			_gamenameText.text = gameContext.GameName;
			_playernameText.text = gameContext.Player.Name;
			_loadSelectedButton.interactable = true;
			_loadSelectedButton.AddListener(() =>
			{
				LoadSelectedGame(gameContext);
			}, true);
			_deleteSelectedButton.interactable = true;
			_deleteSelectedButton.AddListener(() =>
			{
				DeleteSelectedGame(gameContext, button);
			}, true);
		}

		private void LoadSelectedGame(GameContext gameContext)
		{
			Debug.Log($"[LoadGameUIController] LoadSelectedGame() -> Invoking: AppEvents.OnConfirmLoadGame({gameContext})");			
			AppEvents.OnConfirmLoadGame?.Invoke(gameContext);
		}

		private void DeleteSelectedGame(GameContext gameContext, Button button)
		{
	        Debug.Log($"[LoadGameUIController] DeleteSelectedGame() -> Invoking: AppEvents.OnConfirmDeleteGame({gameContext})");			
			AppEvents.OnConfirmDeleteGame?.Invoke(gameContext);
			Destroy(button.gameObject);
			if (_gameButtonsHolder.childCount == 0)
			{
				Back();
				return;
			}
			Initialize();
		}
		
		private void Back()
		{
	        Debug.Log($"[LoadGameUIController] Back() -> Invoking: AppEvents.OnBackToMainMenu");			
			AppEvents.OnBackToMainMenu?.Invoke();
		}

        #endregion

	}
}