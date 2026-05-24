using System;
using System.Collections.Generic;
using System.Globalization;
using Kapibara.Util.NotificableFields;
using Newtonsoft.Json;
using UnityEngine;

namespace Kapibara.RPS
{
	/// <summary>
	/// Modelo de datos serializable que representa una partida guardada completa.
	/// </summary>
	[Serializable]
	public class GameContext
	{
	    #region FIELDS

		//Generic Game Data
		[SerializeField] private NString _gameName;
		[SerializeField] private NString _creationTimestamp;
		[SerializeField] private NString _lastUpdateDate;
		[SerializeField] private int _version;
		//Player Data
		[SerializeField] private NotificableField<Player> _player;
		//Town Context
		[SerializeField] private TownContext _townContext;
		//Library Quests
		[SerializeField] private List<LibraryQuestProgress> _libraryQuests;
		//Credits
		private int    _creditsLeft          = -1;
		private string _creditTimerExpiresUnix = "0";
		//Map
		private List<int> _unlockedLevelIndexes;
		private List<int> _completedLevelIndexes;

		#endregion

	    #region PROPERTIES

		/// <summary>Nombre identificador de la partida guardada.</summary>
		public string GameName
		{
			get => _gameName.Value;
			set => _gameName.Value = value;
		}

		/// <summary>Marca de tiempo Unix de creación de la partida.</summary>
		public string CreationTimestamp
		{
			get => _creationTimestamp.Value;
			set => _creationTimestamp.Value = value;
		}

		/// <summary>Fecha legible de la última actualización.</summary>
		public string LastUpdateDate
		{
			get => _lastUpdateDate.Value;
			set => _lastUpdateDate.Value = value;
		}

		/// <summary>Versión del formato de guardado para migraciones futuras.</summary>
		public int Version
		{
			get => _version;
		}

		/// <summary>Datos del jugador asociados a esta partida.</summary>
		public Player Player
		{
			get => _player.Value;
			set => _player.Value = value;
		}

		/// <summary>Contexto de la ciudad con todos los edificios.</summary>
		public TownContext TownContext
		{
			get => _townContext;
		}

		/// <summary>Acceso directo a la lista de edificios del contexto de ciudad.</summary>
		public List<TownData> TownData
		{
			get => _townContext.TownData;
			set => _townContext.TownData = value;
		}

		/// <summary>Quests de la Biblioteca materializadas al activar el edificio. Vacío hasta que el NPC es rescatado.</summary>
		public List<LibraryQuestProgress> LibraryQuests => _libraryQuests;

		/// <summary>Niveles desbloqueados en runtime via victoria de boss. Complementa el flag IsAvailable del ScriptableObject.</summary>
		public List<int> UnlockedLevelIndexes => _unlockedLevelIndexes;

		/// <summary>Niveles cuyo boss ha sido derrotado al menos una vez. Fuente de verdad para MapLevel.IsCompleted.</summary>
		public List<int> CompletedLevelIndexes => _completedLevelIndexes;

		/// <summary>Créditos de viaje disponibles. Persiste en el fichero de guardado.</summary>
		public int CreditsLeft
		{
			get => _creditsLeft;
			set => _creditsLeft = value;
		}

		/// <summary>Unix timestamp (segundos) en que expira el próximo crédito. "0" = timer parado.</summary>
		public string CreditTimerExpiresUnix
		{
			get => _creditTimerExpiresUnix;
			set => _creditTimerExpiresUnix = value;
		}

		#endregion

	    #region CONSTRUCTORS

		public GameContext() { }

		public GameContext(string gameName, string playerName)
		{
			_gameName = new NString(gameName);
			_creationTimestamp = new NString(RPSTimestamp.GetTimestamp());
			_lastUpdateDate = new NString(RPSTimestamp.ConvertTimestampToDateTime(RPSTimestamp.GetTimestamp()).ToString(CultureInfo.InvariantCulture));
			_version = 1;
			_player = new NotificableField<Player> { Value = new Player(playerName) };
			_townContext = new TownContext(new List<TownData>()
			{
				new TownData(TownMenu.LIBRARY),
				new TownData(TownMenu.PAPER_TREE),
				new TownData(TownMenu.SCISSORS),
				new TownData(TownMenu.STABLES),
				new TownData(TownMenu.STONE_SMITHY),
				new TownData(TownMenu.THEATER),
				new TownData(TownMenu.TRAINING_HOUSE),
				new TownData(TownMenu.TRAVEL, true, false, false, false),
				new TownData(TownMenu.HOUSE, true, false, false, false)
			});
			_libraryQuests          = new List<LibraryQuestProgress>();
			_creditsLeft            = -1;
			_creditTimerExpiresUnix = "0";
			_unlockedLevelIndexes   = new List<int>();
			_completedLevelIndexes  = new List<int>();
		}

		// townContext: new saves. townData: backward compat with old saves that had the flat list.
		[JsonConstructor]
		public GameContext(string gameName, string creationTimestamp, string lastUpdateDate, Player player, TownContext townContext, List<TownData> townData, int version = 0, int creditsLeft = 5, string creditTimerExpiresUnix = "0", List<LibraryQuestProgress> libraryQuests = null, List<int> unlockedLevelIndexes = null, List<int> completedLevelIndexes = null)
		{
			_gameName = new NString(gameName);
			_creationTimestamp = new NString(creationTimestamp);
			_lastUpdateDate = new NString(lastUpdateDate);
			_player = new NotificableField<Player> { Value = player };
			_townContext = townContext ?? new TownContext(townData ?? new List<TownData>());
			_version = version;
			_creditsLeft            = creditsLeft;
			_creditTimerExpiresUnix = creditTimerExpiresUnix ?? "0";
			_libraryQuests          = libraryQuests ?? new List<LibraryQuestProgress>();
			_unlockedLevelIndexes   = unlockedLevelIndexes ?? new List<int>();
			_completedLevelIndexes  = completedLevelIndexes ?? new List<int>();
		}

		#endregion

	    #region TOSTRING

		public override string ToString()
		{
			return $"[GameContext] {GameName} --- {Player.Name}";
		}

		#endregion
	}
}