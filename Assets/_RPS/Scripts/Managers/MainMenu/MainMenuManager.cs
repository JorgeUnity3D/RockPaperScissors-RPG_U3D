using System.Collections.Generic;
using System.Globalization;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS
{
	public class MainMenuManager : BaseManager
	{
		[SerializeField, ReadOnly] private UIService _uiService;
		[SerializeField, ReadOnly] private SceneService _sceneService;
		[SerializeField, ReadOnly] private PersistenceService _persistenceService;
		[SerializeField, ReadOnly] private MainMenuUIController _mainMenuUIController;
		[SerializeField, ReadOnly] private NewGameUIController _newGameUIController;
		[SerializeField, ReadOnly] private LoadGameUIController _loadGameUIController;

        #region SETUP

		public override void SetUp()
		{
			Debug.Log($"[MainMenuManager] SetUp() -> ");
			_uiService = ServiceLocator.Instance.GetService<UIService>();
			_sceneService = ServiceLocator.Instance.GetService<SceneService>();
			_persistenceService = ServiceLocator.Instance.GetService<PersistenceService>();
			_mainMenuUIController = _uiService.GetController<MainMenuUIController>();
			_newGameUIController = _uiService.GetController<NewGameUIController>();
			_loadGameUIController = _uiService.GetController<LoadGameUIController>();
		}

		protected override void Subscribe()
		{
			Debug.Log($"[MainMenuManager] Subscribe() -> ");
			AppEvents.OnContinueGame += ContinueGame;
			AppEvents.OnNewGameMenu += NewGameMenu;
			AppEvents.OnLoadGameMenu += LoadGameMenu;
			AppEvents.OnBackToMainMenu += Initialize;
			AppEvents.OnConfirmNewGame += ConfirmNewGame;
			AppEvents.OnConfirmLoadGame += LoadSelectedGame;
			AppEvents.OnConfirmDeleteGame += DeleteSelectedGame;
		}

		protected override void UnSubscribe()
		{
			Debug.Log($"[MainMenuManager] UnSubscribe() -> ");
			AppEvents.OnContinueGame -= ContinueGame;
			AppEvents.OnNewGameMenu -= NewGameMenu;
			AppEvents.OnLoadGameMenu -= LoadGameMenu;
			AppEvents.OnBackToMainMenu -= Initialize;
			AppEvents.OnConfirmNewGame -= ConfirmNewGame;
			AppEvents.OnConfirmLoadGame -= LoadSelectedGame;
			AppEvents.OnConfirmDeleteGame -= DeleteSelectedGame;
		}

        #endregion

        #region CONTROL

		public override void Initialize()
		{
			Debug.Log($"[MainMenuManager] Initialize() -> ");
			MainMenu();
		}
		
		private void MainMenu()
		{
			Debug.Log($"[MainMenuManager] MainMenu() -> ");
			_mainMenuUIController.ShowCanvas(0);
			_newGameUIController.HideCanvas(0);
			_loadGameUIController.HideCanvas(0);
			_persistenceService.LoadGameList((gameContexts) =>
			{
				_mainMenuUIController.EnableContinueButton(gameContexts.Count > 0);
			});
		}

		private void ContinueGame()
		{
			Debug.Log($"[MainMenuManager] ContinueGame() -> ");
			_persistenceService.LoadGameList((gameContexts) =>
			{
				LoadSelectedGame(gameContexts[0]);
			});
		}
		
		private void NewGameMenu()
		{
			Debug.Log($"[MainMenuManager] NewGameMenu() -> ");
			_mainMenuUIController.HideCanvas();
			_newGameUIController.ShowCanvas();
			_newGameUIController.Initialize();
		}

		private void LoadGameMenu()
		{
			Debug.Log($"[MainMenuManager] LoadGameMenu() -> ");
			_mainMenuUIController.HideCanvas();
			_loadGameUIController.ShowCanvas();
			_loadGameUIController.Initialize();
			_persistenceService.LoadGameList(_loadGameUIController.InstanceGames);
		}

		private void ConfirmNewGame(string playername)
		{
			Debug.Log($"[MainMenuManager] ConfirmNewGame() -> ");
			GameContext gameContext = new GameContext()
			{
				gameName = "Game_" + _persistenceService.GetGamesCount(),
				timestamp = RPSTimestamp.GetTimestamp(),
				date = RPSTimestamp.ConvertTimestampToDateTime(RPSTimestamp.GetTimestamp()).ToString(CultureInfo.InvariantCulture),
				player = new Player(playername),
				townViews = new List<TownView>()
				{
					new TownView(TownMenu.LIBRARY),
					new TownView(TownMenu.PAPER_TREE),
					new TownView(TownMenu.SCISSORS),
					new TownView(TownMenu.STABLES),
					new TownView(TownMenu.STONE_SMITHY),
					new TownView(TownMenu.THEATER),
					new TownView(TownMenu.TRAINING_HOUSE),
					new TownView(TownMenu.TRAVEL),
					new TownView(TownMenu.HOUSE, true, false, false, false)
				}
			};
			_persistenceService.SaveGame(gameContext);
			LoadSelectedGame(gameContext);
		}

		private void LoadSelectedGame(GameContext gameContext)
		{
			Debug.Log($"[MainMenuManager] LoadSelectedGame() -> ");
			AppContext.GameContext = gameContext;
			_sceneService.LoadScene(GameScenes.TOWN);
		}
		
		private void DeleteSelectedGame(GameContext gameContext)
		{
			Debug.Log($"[MainMenuManager] DeleteSelectedGame() -> ");
			_persistenceService.DeleteGame(gameContext.gameName);
		}

        #endregion

	}
}