using UnityEngine;
using UnityEngine.Events;
using Object = System.Object;

namespace Kapibara.RPS
{
    public static class AppEvents
	{
		//Player
		public static UnityAction OnGameContextUpdated;
		
        //00_Intro
        public static UnityAction OnIntroCompleted;
        
        //01_MainMenu
        public static UnityAction OnContinueGame;
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
        public static UnityAction<TownView> OnConfirmUnlock;
        public static UnityAction<TownView> OnPayUnlock;
        public static UnityAction OnCancelUnlock;
        

	}
}