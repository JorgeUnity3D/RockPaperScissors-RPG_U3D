using System.Collections.Generic;
using System.Globalization;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS
{
	public class MainMenuManager : BaseManager
	{
		[SerializeField, ReadOnly] private UIService _uiService;
		[SerializeField, ReadOnly] private PersistenceService _persistenceService;
		[SerializeField, ReadOnly] private MainMenuUIController _mainMenuUIController;
		[SerializeField, ReadOnly] private NewGameUIController _newGameUIController;
		[SerializeField, ReadOnly] private LoadGameUIController _loadGameUIController;

        #region SETUP

		public override void SetUp()
		{
			Debug.Log($"[MainMenuManager] SetUp() -> ");
			_uiService = ServiceLocator.Instance.GetService<UIService>();
			_persistenceService = ServiceLocator.Instance.GetService<PersistenceService>();
			_mainMenuUIController = _uiService.GetController<MainMenuUIController>();
			_newGameUIController = _uiService.GetController<NewGameUIController>();
			_loadGameUIController = _uiService.GetController<LoadGameUIController>();
		}

		protected override void Subscribe()
		{
			Debug.Log($"[MainMenuManager] Subscribe() -> ");
			AppEvents.OnNewGameMenu += NewGameMenu;
			AppEvents.OnLoadGameMenu += LoadGameMenu;
			AppEvents.OnBackToMainMenu += Initialize;
		}

		protected override void UnSubscribe()
		{
			Debug.Log($"[MainMenuManager] UnSubscribe() -> ");
			AppEvents.OnNewGameMenu -= NewGameMenu;
			AppEvents.OnLoadGameMenu -= LoadGameMenu;
			AppEvents.OnBackToMainMenu -= Initialize;
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

        #endregion
	}
}