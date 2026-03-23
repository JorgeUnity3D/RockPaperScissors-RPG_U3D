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
		[SerializeField] private NString _timestamp;
		[SerializeField] private NString _date;
		[SerializeField] private int _version;
		//Player Data
		[SerializeField] private NotificableField<Player> _player;
		//Town Context
		[SerializeField] private TownContext _townContext;

		#endregion

	    #region PROPERTIES

		/// <summary>Nombre identificador de la partida guardada.</summary>
		public string GameName
		{
			get => _gameName.Value;
			set => _gameName.Value = value;
		}

		/// <summary>Marca de tiempo Unix de la última modificación.</summary>
		public string Timestamp
		{
			get => _timestamp.Value;
			set => _timestamp.Value = value;
		}

		/// <summary>Fecha legible de la última modificación.</summary>
		public string Date
		{
			get => _date.Value;
			set => _date.Value = value;
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

		#endregion

	    #region CONSTRUCTORS

		public GameContext() { }

		public GameContext(string gameName, string playerName)
		{
			_gameName = new NString(gameName);
			_timestamp = new NString(RPSTimestamp.GetTimestamp());
			_date = new NString(RPSTimestamp.ConvertTimestampToDateTime(RPSTimestamp.GetTimestamp()).ToString(CultureInfo.InvariantCulture));
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
				new TownData(TownMenu.TRAVEL, true, false, false, false, false),
				new TownData(TownMenu.HOUSE, true, false, false, false, false)
			});
		}

		// townContext: new saves. townData: backward compat with old saves that had the flat list.
		[JsonConstructor]
		public GameContext(string gameName, string timestamp, string date, Player player, TownContext townContext, List<TownData> townData, int version = 0)
		{
			_gameName = new NString(gameName);
			_timestamp = new NString(timestamp);
			_date = new NString(date);
			_player = new NotificableField<Player> { Value = player };
			_townContext = townContext ?? new TownContext(townData ?? new List<TownData>());
			_version = version;
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