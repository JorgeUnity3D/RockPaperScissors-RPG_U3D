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
		[SerializeField, ReadOnly] private static PersistenceService _persistenceService;
		[SerializeField, ReadOnly] private static SceneService _sceneService;
		[SerializeField, ReadOnly] private static GameContext _gameContext;
	
        #region SETUP

		public override void SetUp()
		{
			_persistenceService = ServiceLocator.Instance.GetService<PersistenceService>();
			_sceneService = ServiceLocator.Instance.GetService<SceneService>();
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
			List<TownView> townViews = new List<TownView>()
			{
				new TownView(TownMenu.LIBRARY),
				new TownView(TownMenu.PAPER_TREE),
				new TownView(TownMenu.SCISSORS),
				new TownView(TownMenu.STABLES),
				new TownView(TownMenu.STONE_SMITHY),
				new TownView(TownMenu.THEATER),
				new TownView(TownMenu.TRAINING_HOUSE),
				new TownView(TownMenu.TRAVEL),
				new TownView(TownMenu.HOUSE, true, false, false, false, false)
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
			Debug.Log($"[GameManager] DeleteSelectedGame() -> ");
			AppContext.GameContext.Date = RPSTimestamp.ConvertTimestampToDateTime(RPSTimestamp.GetTimestamp()).ToString(CultureInfo.InvariantCulture);
			ServiceLocator.Instance.GetService<PersistenceService>().UpdateSaveGame(AppContext.GameContext);
		}

        #endregion
	}
}