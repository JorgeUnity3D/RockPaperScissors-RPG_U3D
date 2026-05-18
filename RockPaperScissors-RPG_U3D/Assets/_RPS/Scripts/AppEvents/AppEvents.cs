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
		/// <summary>Se dispara cuando cambia el nivel del jugador.</summary>
		public static UnityAction<int> OnLevelUpdated;
		/// <summary>Se dispara cuando cambia el oro del jugador.</summary>
		public static UnityAction<int> OnGoldUpdated;

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
		/// <summary>Señal de pago del desbloqueo (uso futuro).</summary>
		public static UnityAction<TownData> OnPayUnlock;
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
		/// <summary>Solicita ganar un crédito (caballo). StablesManager lo dispara; CreditsTimeCounterManager lo consume.</summary>
		public static UnityAction OnEarnCredit;

		/// <summary>Se dispara al pulsar un botón de historia en TheaterUIController. El índice corresponde a la lista en TheaterScrObj.</summary>
		public static UnityAction<int> OnStorySelected;
		/// <summary>Se dispara al cerrar el lector de comic (ComicPlayerUIController). TheaterManager lo recibe para restaurar el estado del Theater.</summary>
		public static UnityAction OnComicClosed;

		/// <summary>Se dispara cuando el jugador pulsa un botón de acción en combate.</summary>
		public static UnityAction<Actions> OnCombatActionSelected;
		/// <summary>Se dispara al terminar una ronda de combate con el resultado (daño jugador, daño enemigo).</summary>
		public static UnityAction<int, int> OnCombatRoundResolved;
		/// <summary>Se dispara al terminar el combate. true = victoria, false = derrota.</summary>
		public static UnityAction<bool> OnCombatFinished;

		/// <summary>Se dispara cuando el jugador selecciona una acción RPS en un step de tesoro.</summary>
		public static UnityAction<Actions> OnTreasureActionSelected;
		/// <summary>Se dispara cuando el jugador pulsa "Siguiente" en el diálogo de NPC.</summary>
		public static UnityAction OnNPCDialogueNext;
		/// <summary>Se dispara cuando el jugador pulsa "Anterior" en el diálogo de NPC.</summary>
		public static UnityAction OnNPCDialoguePrev;
	}
}