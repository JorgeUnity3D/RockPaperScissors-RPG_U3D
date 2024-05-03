using UnityEngine;
using UnityEngine.Events;
using Object = System.Object;

namespace Kapibara.RPS
{
	public static class AppEvents
	{
		//Player
		public static UnityAction OnGameContextUpdated;
		public static UnityAction<int> OnLevelUpdated;
		public static UnityAction<int> OnGoldUpdated;

		//00_Intro
		public static UnityAction OnIntroCompleted;

		//01_MainMenu
		public static UnityAction OnConfirmContinueGame;
		public static UnityAction OnNewGameMenu;
		public static UnityAction OnLoadGameMenu;
		public static UnityAction OnBackToMainMenu;
		public static UnityAction<string> OnConfirmNewGame;
		public static UnityAction<GameContext> OnConfirmLoadGame;
		public static UnityAction<GameContext> OnConfirmDeleteGame;

		//02_Town
		//Town Menus
		public static UnityAction<TownMenu> OnOpenTownMenu;
		public static UnityAction OnBackFromTownMenu;
		//Town Unlock Menus
		public static UnityAction<TownData> OnConfirmUnlock;
		public static UnityAction<TownData> OnPayUnlock;
		public static UnityAction OnCancelUnlock;
		//Training House
		public static UnityAction<TrainingHouseModifier> OnTrainingSelected;
		public static UnityAction<TrainingHouseModifier> OnTrainingUnlocked;
		public static UnityAction OnTrainingExpUpdated;
		public static UnityAction OnTrainingLevelUpdated;
		//Scissors Bonfire
		public static UnityAction OnConfirmLevelUp;
	}
}