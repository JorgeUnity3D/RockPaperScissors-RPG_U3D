using System;
using System.Globalization;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Kapibara.RPS
{
	public class GameManager : BaseManager
	{
		[SerializeField, ReadOnly] private static GameContext _gameContext;
		
        #region SETUP

		protected override void Subscribe()
		{
			Debug.Log($"[GameManager] Subscribe() -> ");
			SceneManager.sceneLoaded += InitializeScene;
			AppEvents.OnGameContextUpdated += UpdateSaveGame;
		}

		protected override void UnSubscribe()
		{
			Debug.Log($"[GameManager] UnSubscribe() -> ");
			SceneManager.sceneLoaded -= InitializeScene;
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
					//ServiceLocator.Instance.GetService<ManagerService>().GetManager<PlayerManager>().Initialize();
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

		private void UpdateSaveGame()
		{
			AppContext.GameContext.date = RPSTimestamp.ConvertTimestampToDateTime(RPSTimestamp.GetTimestamp()).ToString(CultureInfo.InvariantCulture);
			ServiceLocator.Instance.GetService<PersistenceService>().UpdateSaveGame(AppContext.GameContext);
		}
		
        #endregion
	}
}