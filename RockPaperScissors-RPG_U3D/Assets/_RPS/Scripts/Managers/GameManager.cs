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
		[Header("DEBUG")]
		[SerializeField, ReadOnly] private PersistenceService _persistenceService;
		[SerializeField, ReadOnly] private SceneService _sceneService;
		[SerializeField, ReadOnly] private GameContext _gameContext;

		private bool _waitingForComic;

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
			AppEvents.OnStepFinished        += OnStepFinished;
			AppEvents.OnComicClosed         += OnComicClosed;
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
			AppEvents.OnStepFinished        -= OnStepFinished;
			AppEvents.OnComicClosed         -= OnComicClosed;
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
			string      gameName    = "Game_" + _persistenceService.GetGamesCount();
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
		}

		private void DeleteSelectedGame(GameContext gameContext)
		{
			Debug.Log($"[GameManager] DeleteSelectedGame() -> ");
			_persistenceService.DeleteGame(gameContext.GameName);
		}

		private void OnStepFinished(bool playerWins)
		{
			Debug.Log($"[GameManager] OnStepFinished() -> playerWins={playerWins}");

			CombatContext ctx = AppContext.CombatContext;

			if (ctx == null)
			{
				// Viene del botón Continue del result UI — ir a Town
				_sceneService.LoadScene(GameScenes.TOWN);
				return;
			}

			if (playerWins)
			{
				MapStep step = ctx.CurrentStep;

				if ((step.Type == MapStepType.COMBAT || step.Type == MapStepType.BOSS) && step.Enemy != null)
					ProcessLibraryKills(step.Enemy.Data.Id);

				if (step.Type == MapStepType.BOSS)
				{
					bool isFirstCompletion = !ctx.SelectedLevel.IsCompleted;
					ctx.SelectedLevel.SetCompleted();

					if (isFirstCompletion)
					{
						ComicStoryScrObj bossStory = ctx.SelectedLevel.BossStory;
						if (bossStory != null)
						{
							if (bossStory.StoryId >= 0)
								AppContext.Player.UnlockStory(bossStory.StoryId);

							ComicPlayerUIController comicPlayer = ServiceLocator.Instance.GetService<UIService>().GetController<ComicPlayerUIController>();
							if (comicPlayer != null)
							{
								Debug.Log($"[GameManager] Boss defeated (first time) → showing Historia (storyId {bossStory.StoryId})");
								_waitingForComic = true;
								comicPlayer.SetData(bossStory);
								return;
							}
							Debug.LogWarning("[GameManager] BossStory assigned but ComicPlayerUIController not found in scene.");
						}
					}
					else
					{
						Debug.Log("[GameManager] Boss defeated (level already completed) → skipping Historia.");
					}
				}

				if (step.Type == MapStepType.NPC_RESCUE)
				{
					TownMenu       targetBuilding = step.TargetBuilding;
					List<TownData> townDatas      = AppContext.GameContext?.TownData;
					TownData       townData        = townDatas?.Find(td => td.TownMenu == targetBuilding);
					if (townData != null)
					{
						townData.NpcUnlocked = true;
						Debug.Log($"[GameManager] NPC rescued → {targetBuilding} unlocked.");
					}
					else
					{
						Debug.LogWarning($"[GameManager] NPC rescue: TownData not found for {targetBuilding}.");
					}
				}

				int nextIndex = ctx.CurrentStepIndex + 1;
				if (nextIndex < ctx.StepCount)
				{
					ctx.CurrentStepIndex = nextIndex;
					Debug.Log($"[GameManager] OnStepFinished() -> advancing to step {nextIndex} ({ctx.CurrentStep.Type})");
					StepManager stepManager = ServiceLocator.Instance.GetService<ManagerService>().GetManager<StepManager>();
					if (stepManager != null)
						stepManager.Initialize();
					else
						Debug.LogError("[GameManager] OnStepFinished() -> StepManager not found.");
					return;
				}
			}

			ShowLevelResult(ctx, playerWins);
		}

		private void OnComicClosed()
		{
			if (!_waitingForComic) return;
			_waitingForComic = false;

			CombatContext ctx = AppContext.CombatContext;
			if (ctx == null) return;

			int nextIndex = ctx.CurrentStepIndex + 1;
			if (nextIndex < ctx.StepCount)
			{
				ctx.CurrentStepIndex = nextIndex;
				Debug.Log($"[GameManager] OnComicClosed() -> advancing to step {nextIndex} ({ctx.CurrentStep.Type})");
				StepManager stepManager = ServiceLocator.Instance.GetService<ManagerService>().GetManager<StepManager>();
				if (stepManager != null)
					stepManager.Initialize();
				else
					Debug.LogError("[GameManager] OnComicClosed() -> StepManager not found.");
			}
			else
			{
				ShowLevelResult(ctx, true);
			}
		}

		private void ShowLevelResult(CombatContext ctx, bool playerWins)
		{
			int totalGold = ctx.TotalGoldEarned;
			int totalExp  = ctx.TotalTrainingExp;
			AppContext.CombatContext = null;

			CombatResultUIController resultUI = ServiceLocator.Instance.GetService<UIService>().GetController<CombatResultUIController>();
			if (resultUI == null)
			{
				Debug.LogError("[GameManager] ShowLevelResult() -> CombatResultUIController not found.");
				_sceneService.LoadScene(GameScenes.TOWN);
				return;
			}

			Debug.Log($"[GameManager] ShowLevelResult() -> win:{playerWins}  gold:{totalGold}  exp:{totalExp}");
			resultUI.SetData(playerWins, totalGold, totalExp);
			resultUI.ShowCanvas();
		}

		private void ProcessLibraryKills(string enemyId)
		{
			List<LibraryQuestProgress> quests = AppContext.GameContext?.LibraryQuests;
			if (quests == null || quests.Count == 0) return;

			bool changed = false;
			foreach (LibraryQuestProgress quest in quests)
			{
				if (quest.EnemyId != enemyId || quest.IsCompleted) continue;
				if (quest.CurrentKills >= quest.TargetKills) continue;

				quest.CurrentKills++;
				changed = true;

				if (quest.CurrentKills >= quest.TargetKills)
				{
					quest.IsCompleted = true;
					ApplyLibraryReward(quest);
					AddLibraryExp(1);
				}
			}

			if (changed)
				AppEvents.OnGameContextUpdated?.Invoke();
		}

		private void ApplyLibraryReward(LibraryQuestProgress quest)
		{
			Player player = AppContext.Player;
			if (player == null) return;
			StatAttribute attribute = player.Attributes.Find(a => a.Stat == quest.RewardStat);
			if (attribute == null) return;
			LibraryModifier modifier = attribute.GetModifier<LibraryModifier>();
			if (modifier == null) return;
			modifier.Modifier += quest.RewardAmount;
			Debug.Log($"[GameManager] Library quest complete — +{quest.RewardAmount} {quest.RewardStat}");
		}

		private void AddLibraryExp(int amount)
		{
			TownData libraryData = AppContext.TownData?.Find(td => td.TownMenu == TownMenu.LIBRARY);
			if (libraryData == null) return;
			libraryData.Experience += amount;
			int maxLevel = GameConsts.TRAINING_EXP_PER_LEVEL.Count - 1;
			while (libraryData.Level < maxLevel &&
			       libraryData.Experience >= GameConsts.TRAINING_EXP_PER_LEVEL[libraryData.Level])
			{
				libraryData.Level++;
			}
			if (libraryData.Level >= maxLevel)
				libraryData.Experience = Mathf.Min(libraryData.Experience, GameConsts.TRAINING_EXP_PER_LEVEL[maxLevel]);
			AppEvents.OnBuildingExpUpdated?.Invoke(TownMenu.LIBRARY);
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