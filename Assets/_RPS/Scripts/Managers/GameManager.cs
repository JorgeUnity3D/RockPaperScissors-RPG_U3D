using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Kapibara.RPS
{
	public class GameManager : BaseManager
	{
		[SerializeField, ReadOnly] private PersistenceService _persistenceService;
		[SerializeField, ReadOnly] private SceneService _sceneService;
		[SerializeField, ReadOnly] private GameContext _gameContext;

		[SerializeField] private bool _testContext = true;
		
        #region SETUP

		public override void SetUp()
		{
			_persistenceService = ServiceLocator.Instance.GetService<PersistenceService>();
			_sceneService = ServiceLocator.Instance.GetService<SceneService>();
			if (_testContext)
			{
				RunTest();
			}
		}

		private void RunTest()
		{
			_persistenceService.LoadGameList((gameContexts) =>
			{
				AppContext.GameContext = gameContexts[0];
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

		private void ConfirmNewGame(string playername)
		{
			Debug.Log($"[GameManager] ConfirmNewGame() -> ");
			string gameName = "Game_" + _persistenceService.GetGamesCount();
			string timestamp = RPSTimestamp.GetTimestamp();
			string date = RPSTimestamp.ConvertTimestampToDateTime(RPSTimestamp.GetTimestamp()).ToString(CultureInfo.InvariantCulture);
			Player player = new Player(playername);
			List<TownData> townViews = new List<TownData>()
			{
				new TownData(TownMenu.LIBRARY),
				new TownData(TownMenu.PAPER_TREE),
				new TownData(TownMenu.SCISSORS),
				new TownData(TownMenu.STABLES),
				new TownData(TownMenu.STONE_SMITHY),
				new TownData(TownMenu.THEATER),
				new TownData(TownMenu.TRAINING_HOUSE),
				new TownData(TownMenu.TRAVEL),
				new TownData(TownMenu.HOUSE, true, false, false, false, false)
			};
			GameContext gameContext = new GameContext(gameName, timestamp, date, player, townViews);
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