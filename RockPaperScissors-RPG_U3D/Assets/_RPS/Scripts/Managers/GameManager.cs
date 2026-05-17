using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Kapibara.RPS
{
	/// <summary>
	/// Manager global persistente que orquesta la carga de escenas, el guardado y la inicialización de managers por escena.
	/// </summary>
	public class GameManager : BaseManager
	{
		[SerializeField, ReadOnly] private PersistenceService _persistenceService;
		[SerializeField, ReadOnly] private SceneService _sceneService;
		[SerializeField, ReadOnly] private GameContext _gameContext;

        #region SETUP

		public override void SetUp()
		{
			UnityEngine.Random.InitState(System.Environment.TickCount ^ System.Guid.NewGuid().GetHashCode());
			_persistenceService = ServiceLocator.Instance.GetService<PersistenceService>();
			_sceneService = ServiceLocator.Instance.GetService<SceneService>();
		}

		protected override void Subscribe()
		{
			Debug.Log($"[GameManager] Subscribe() -> ");
			SceneManager.sceneLoaded        += InitializeScene;
			AppEvents.OnConfirmContinueGame += ContinueGame;
			AppEvents.OnConfirmNewGame      += ConfirmNewGame;
			AppEvents.OnConfirmLoadGame     += LoadSelectedGame;
			AppEvents.OnConfirmDeleteGame   += DeleteSelectedGame;
			AppEvents.OnCombatFinished      += OnCombatFinished;
			AppEvents.OnGameContextUpdated  += UpdateSaveGame;
		}

		protected override void UnSubscribe()
		{
			Debug.Log($"[GameManager] UnSubscribe() -> ");
			SceneManager.sceneLoaded -= InitializeScene;
			AppEvents.OnConfirmContinueGame -= ContinueGame;
			AppEvents.OnConfirmNewGame      -= ConfirmNewGame;
			AppEvents.OnConfirmLoadGame     -= LoadSelectedGame;
			AppEvents.OnConfirmDeleteGame   -= DeleteSelectedGame;
			AppEvents.OnCombatFinished      -= OnCombatFinished;
			AppEvents.OnGameContextUpdated  -= UpdateSaveGame;
		}

        #endregion

        #region CONTROL

		void InitializeScene(Scene scene, LoadSceneMode loadSceneMode)
		{
			Debug.Log($"[GameManager] InitializeScene() -> scene {scene.name}");
			ManagerService managerService = ServiceLocator.Instance.GetService<ManagerService>();
			switch (GameConsts.SceneEnums[scene.name])
			{
				case GameScenes.INTRO:
					IntroManager introManager = managerService.GetManager<IntroManager>();
					if (introManager == null) { Debug.LogError("[GameManager] InitializeScene() -> IntroManager not found."); break; }
					introManager.Initialize();
					break;
				case GameScenes.MAIN_MENU:
					MainMenuManager mainMenuManager = managerService.GetManager<MainMenuManager>();
					if (mainMenuManager == null) { Debug.LogError("[GameManager] InitializeScene() -> MainMenuManager not found."); break; }
					mainMenuManager.Initialize();
					break;
				case GameScenes.TOWN:
					TownManager townManager = managerService.GetManager<TownManager>();
					if (townManager == null) { Debug.LogError("[GameManager] InitializeScene() -> TownManager not found."); break; }
					townManager.Initialize();
					break;
				case GameScenes.COMBAT:
					StepManager stepManager = managerService.GetManager<StepManager>();
					if (stepManager == null) { Debug.LogError("[GameManager] InitializeScene() -> StepManager not found."); break; }
					stepManager.Initialize();
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
			AppEvents.OnGameContextUpdated -= UpdateSaveGame;
			string gameName = "Game_" + _persistenceService.GetGamesCount();
			GameContext gameContext = new GameContext(gameName, playerName);
			_persistenceService.SaveGame(gameContext);
			AppEvents.OnGameContextUpdated += UpdateSaveGame;
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
		}

		private void DeleteSelectedGame(GameContext gameContext)
		{
			Debug.Log($"[GameManager] DeleteSelectedGame() -> ");
			_persistenceService.DeleteGame(gameContext.GameName);
		}

		private void OnCombatFinished(bool playerWins)
		{
			Debug.Log($"[GameManager] OnCombatFinished() -> playerWins={playerWins}");

			CombatContext ctx = AppContext.CombatContext;
			if (playerWins && ctx != null)
			{
				int nextIndex = ctx.CurrentStepIndex + 1;
				if (nextIndex < ctx.SelectedLevel.StepCount)
				{
					ctx.CurrentStepIndex = nextIndex;
					Debug.Log($"[GameManager] OnCombatFinished() -> advancing to step {nextIndex} ({ctx.CurrentStep.Type})");
					_sceneService.LoadScene(GameScenes.COMBAT);
					return;
				}
			}

			AppContext.CombatContext = null;
			_sceneService.LoadScene(GameScenes.TOWN);
		}

		private void UpdateSaveGame()
		{
			Debug.Log($"[GameManager] UpdateSaveGame() -> ");
			if (AppContext.GameContext == null) return;
			ServiceLocator.Instance.GetService<PersistenceService>().UpdateSaveGame(AppContext.GameContext);
		}

        #endregion
	}
}