using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Kapibara.RPS
{
	public class GameManager : BaseManager
	{
		[SerializeField, ReadOnly] private PersistenceService _persistenceService;
		[SerializeField, ReadOnly] private SceneService _sceneService;
		[SerializeField, ReadOnly] private GameContext _gameContext;

		[SerializeField] private bool _runTest = true;
		[SerializeField] private bool _runCleanTest = true;

        #region SETUP

		public override void SetUp()
		{
			_persistenceService = ServiceLocator.Instance.GetService<PersistenceService>();
			_sceneService = ServiceLocator.Instance.GetService<SceneService>();
			if (_runTest)
			{
				RunTest();
			}
		}

		private void RunTest()
		{
			_persistenceService.LoadGameList((gameContexts) =>
			{
				if (_runCleanTest)
				{
					AppContext.GameContext = new GameContext("Game_Test", "Player_Test");
				}
				else
				{
					if (gameContexts.Exists(gc => gc.GameName == "Game_Test"))
					{
						AppContext.GameContext = gameContexts.Find(gc => gc.GameName == "Game_Test");
					}
					else
					{
						AppContext.GameContext = new GameContext("Game_Test", "Player_Test");
					}
				}
				_gameContext = AppContext.GameContext;
				AppEvents.OnGameContextUpdated += UpdateSaveGame;
			});
		}

		protected override void Subscribe()
		{
			Debug.Log($"[GameManager] Subscribe() -> ");
			SceneManager.sceneLoaded += InitializeScene;
			AppEvents.OnConfirmContinueGame += ContinueGame;
			AppEvents.OnConfirmNewGame += ConfirmNewGame;
			AppEvents.OnConfirmLoadGame += LoadSelectedGame;
			AppEvents.OnConfirmDeleteGame += DeleteSelectedGame;
		}

		protected override void UnSubscribe()
		{
			Debug.Log($"[GameManager] UnSubscribe() -> ");
			SceneManager.sceneLoaded -= InitializeScene;
			AppEvents.OnConfirmContinueGame -= ContinueGame;
			AppEvents.OnConfirmNewGame -= ConfirmNewGame;
			AppEvents.OnConfirmLoadGame -= LoadSelectedGame;
			AppEvents.OnConfirmDeleteGame -= DeleteSelectedGame;
			AppEvents.OnGameContextUpdated -= UpdateSaveGame;
		}

        #endregion

        #region CONTROL

		void InitializeScene(Scene scene, LoadSceneMode loadSceneMode)
		{
			Debug.Log($"[GameManager] InitializeScene() -> scene {scene.name}");
			switch (GameConsts.SceneEnums[scene.name])
			{
				case GameScenes.INTRO:
					ServiceLocator.Instance.GetService<ManagerService>().GetManager<IntroManager>().Initialize();
					break;
				case GameScenes.MAIN_MENU:
					ServiceLocator.Instance.GetService<ManagerService>().GetManager<MainMenuManager>().Initialize();
					break;
				case GameScenes.TOWN:
					ServiceLocator.Instance.GetService<ManagerService>().GetManager<TownManager>().Initialize();
					break;
				case GameScenes.MAP:
					break;
				case GameScenes.COMBAT:
					break;
				case GameScenes.LOAD:
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private void ConfirmNewGame(string playerName)
		{
			Debug.Log($"[GameManager] ConfirmNewGame() -> ");
			string gameName = "Game_" + _persistenceService.GetGamesCount();
			GameContext gameContext = new GameContext(gameName, playerName);
			_persistenceService.SaveGame(gameContext);
			LoadSelectedGame(gameContext);
		}

		private void ContinueGame()
		{
			Debug.Log($"[GameManager] ContinueGame() -> ");
			_persistenceService.LoadGameList((gameContexts) =>
			{
				LoadSelectedGame(gameContexts[0]);
			});
		}

		private void LoadSelectedGame(GameContext gameContext)
		{
			Debug.Log($"[GameManager] LoadSelectedGame() -> ");
			AppContext.GameContext = gameContext;
			_gameContext = AppContext.GameContext;
			_sceneService.LoadScene(GameScenes.TOWN);
			AppEvents.OnGameContextUpdated += UpdateSaveGame;
		}

		private void DeleteSelectedGame(GameContext gameContext)
		{
			Debug.Log($"[GameManager] DeleteSelectedGame() -> ");
			_persistenceService.DeleteGame(gameContext.GameName);
		}

		private void UpdateSaveGame()
		{
			Debug.Log($"[GameManager] UpdateSaveGame() -> ");
			ServiceLocator.Instance.GetService<PersistenceService>().UpdateSaveGame(AppContext.GameContext);
		}

        #endregion
	}
}