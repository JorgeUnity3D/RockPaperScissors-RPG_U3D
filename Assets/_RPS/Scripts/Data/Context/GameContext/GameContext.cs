using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using UnityEngine;

namespace Kapibara.RPS
{
	[Serializable]
	public class GameContext
	{
	    #region FIELDS

		//Generic Game Data
		[SerializeField] private NString _gameName;
		[SerializeField] private NString _timestamp;
		[SerializeField] private NString _date;
		//Player Data
		[SerializeField] private NotificableField<Player> _player;
		//Town Data
		[SerializeField] private NList<TownData> _townData;

		#endregion

	    #region PROPERTIES

		public string GameName
		{
			get => _gameName.Value;
			set => _gameName.Value = value;
		}

		public string Timestamp
		{
			get => _timestamp.Value;
			set => _timestamp.Value = value;
		}

		public string Date
		{
			get => _date.Value;
			set => _date.Value = value;
		}

		public Player Player
		{
			get => _player.Value;
			set => _player.Value = value;
		}

		public List<TownData> TownData
		{
			get => _townData.Value;
			set => _townData.Value = value;
		}

		#endregion

	    #region CONSTRUCTORS

		public GameContext() { }

		public GameContext(string gameName, string playerName)
		{
			_gameName = new NString(gameName);
			_timestamp = new NString(RPSTimestamp.GetTimestamp());
			_date = new NString(RPSTimestamp.ConvertTimestampToDateTime(RPSTimestamp.GetTimestamp()).ToString(CultureInfo.InvariantCulture));
			_player = new NotificableField<Player> { Value = new Player(playerName) };
			_townData = new NList<TownData>()
			{
				Value = new List<TownData>()
				{
					new TownData(TownMenu.LIBRARY),
					new TownData(TownMenu.PAPER_TREE),
					new TownData(TownMenu.SCISSORS),
					new TownData(TownMenu.STABLES),
					new TownData(TownMenu.STONE_SMITHY),
					new TownData(TownMenu.THEATER),
					new TownData(TownMenu.TRAINING_HOUSE),
					new TownData(TownMenu.TRAVEL),
					new TownData(TownMenu.HOUSE, true, false, false, false, false)
				}
			};
		}

		[JsonConstructor]
		public GameContext(string gameName, string timestamp, string date, Player player, List<TownData> townViews)
		{
			_gameName = new NString(gameName);
			_timestamp = new NString(timestamp);
			_date = new NString(date);
			_player = new NotificableField<Player> { Value = player };
			_townData = new NList<TownData>() { Value = townViews };
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