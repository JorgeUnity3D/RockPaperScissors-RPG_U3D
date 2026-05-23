using UnityEngine;
using UnityEngine.Events;
using Object = System.Object;

namespace Kapibara.RPS
{
	/// <summary>
	/// Bus de eventos global estático. Los managers suscriben y las vistas invocan. Usar siempre con ?.Invoke().
	/// </summary>
	public static class AppEvents
	{
		/// <summary>Dispara el guardado de la partida a disco. Los managers lo invocan explícitamente tras operaciones que modifican el estado persistente.</summary>
		public static UnityAction OnGameContextUpdated;

		/// <summary>Se dispara al finalizar la animación de introducción.</summary>
		public static UnityAction OnIntroCompleted;

		/// <summary>Se dispara para continuar la última partida guardada.</summary>
		public static UnityAction OnConfirmContinueGame;
		/// <summary>Solicita abrir la pantalla de nueva partida.</summary>
		public static UnityAction OnNewGameMenu;
		/// <summary>Solicita abrir la pantalla de carga de partida.</summary>
		public static UnityAction OnLoadGameMenu;
		/// <summary>Solicita volver al menú principal desde cualquier submenú.</summary>
		public static UnityAction OnBackToMainMenu;
		/// <summary>Confirma la creación de una nueva partida con el nombre de jugador dado.</summary>
		public static UnityAction<string> OnConfirmNewGame;
		/// <summary>Confirma la carga de una partida guardada.</summary>
		public static UnityAction<GameContext> OnConfirmLoadGame;
		/// <summary>Confirma el borrado de una partida guardada.</summary>
		public static UnityAction<GameContext> OnConfirmDeleteGame;

		/// <summary>Solicita abrir el menú de un edificio de la ciudad.</summary>
		public static UnityAction<TownMenu> OnOpenTownMenu;
		/// <summary>Solicita volver al mapa de ciudad desde el interior de un edificio.</summary>
		public static UnityAction OnBackFromTownMenu;

		/// <summary>Confirma el desbloqueo de un edificio tras pagar su coste.</summary>
		public static UnityAction<TownData> OnConfirmUnlock;
		/// <summary>Cancela el proceso de desbloqueo de un edificio.</summary>
		public static UnityAction OnCancelUnlock;

		/// <summary>Notifica que el jugador ha seleccionado una estadística para entrenar.</summary>
		public static UnityAction<TrainingHouseModifier> OnTrainingSelected;
		/// <summary>Confirma el desbloqueo del entrenamiento de una estadística.</summary>
		public static UnityAction<TrainingHouseModifier> OnTrainingUnlocked;
		/// <summary>Se dispara al ganar experiencia de entrenamiento (uso futuro).</summary>
		public static UnityAction OnTrainingExpUpdated;
		/// <summary>Se dispara al subir de nivel un entrenamiento (uso futuro).</summary>
		public static UnityAction OnTrainingLevelUpdated;

		/// <summary>Confirma la subida de nivel del jugador en la Hoguera de las Tijeras.</summary>
		public static UnityAction OnConfirmLevelUp;

		/// <summary>Se dispara periódicamente con el tiempo restante del contador de créditos.</summary>
		public static UnityAction<float> OnTimeUpdated;
		/// <summary>Se dispara cuando cambia la cantidad de créditos disponibles.</summary>
		public static UnityAction<int> OnCreditsUpdated;
		/// <summary>El jugador pulsa "Ver anuncio" en Stables. StablesManager lo consume para ganar 1 crédito.</summary>
		public static UnityAction OnWatchAd;
		/// <summary>El jugador pulsa "Comprar juego" en Stables. StablesManager lo consume (IAP placeholder).</summary>
		public static UnityAction OnBuyGame;
		/// <summary>Solicita ganar un crédito (caballo). StablesManager lo dispara; CreditsTimeCounterManager lo consume.</summary>
		public static UnityAction OnEarnCredit;
		/// <summary>El jugador solicita mejorar el item de ataque en StoneSmithy.</summary>
		public static UnityAction OnUpgradeAttack;
		/// <summary>El jugador solicita mejorar el item de curación en StoneSmithy.</summary>
		public static UnityAction OnUpgradeHeal;
		/// <summary>El jugador solicita mejorar el item de energía en StoneSmithy.</summary>
		public static UnityAction OnUpgradeEnergy;
		/// <summary>El jugador solicita viajar a un nivel. CreditsTimeCounterManager valida y descuenta el crédito.</summary>
		public static UnityAction<MapLevel> OnTravelRequested;
		/// <summary>Crédito validado y descontado; TravelManager inicia la escena de combate con el nivel dado.</summary>
		public static UnityAction<MapLevel> OnTravelConfirmed;

		/// <summary>Se dispara al pulsar un botón de historia en TheaterUIController. El índice corresponde a la lista en TheaterScrObj.</summary>
		public static UnityAction<int> OnStorySelected;
		/// <summary>Se dispara al cerrar el lector de comic (ComicPlayerUIController). TheaterManager lo recibe para restaurar el estado del Theater.</summary>
		public static UnityAction OnComicClosed;

		/// <summary>Se dispara al pulsar el botón de ajustes durante el combate. PauseMenuUIController lo consume.</summary>
		public static UnityAction OnSettingsRequested;
		/// <summary>El jugador pulsa Options en el menú de pausa. PauseManager lo consume.</summary>
		public static UnityAction OnOptionsRequested;
		/// <summary>El jugador pulsa Exit en el menú de pausa. PauseManager lo consume para abandonar el nivel.</summary>
		public static UnityAction OnCombatExited;

		/// <summary>Se dispara cuando el jugador pulsa un botón de acción en combate.</summary>
		public static UnityAction<Actions> OnCombatActionSelected;
		/// <summary>Se dispara al terminar un step. true = victoria/completado, false = derrota.</summary>
		public static UnityAction<bool> OnStepFinished;
		/// <summary>Se dispara cuando el jugador recoge el premio de la Caja Sorpresa.</summary>
		public static UnityAction OnSurpriseBoxCollected;

		/// <summary>Se dispara cuando el jugador usa un consumible de la mochila durante el combate.</summary>
		public static UnityAction<ItemType> OnBackpackItemUsed;

		/// <summary>Se dispara cuando el jugador selecciona una acción RPS en un step de tesoro.</summary>
		public static UnityAction<Actions> OnTreasureActionSelected;
		/// <summary>Se dispara cuando el jugador pulsa "Siguiente" en el diálogo de NPC.</summary>
		public static UnityAction OnNPCDialogueNext;
		/// <summary>Se dispara cuando el jugador pulsa "Anterior" en el diálogo de NPC.</summary>
		public static UnityAction OnNPCDialoguePrev;
	}
}