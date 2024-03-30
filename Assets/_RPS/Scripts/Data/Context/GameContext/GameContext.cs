using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Serialization;

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
		[SerializeField] private NList<TownData> _townViews;

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

		public List<TownData> TownViews
		{
			get => _townViews.Value;
			set => _townViews.Value = value;
		}

		#endregion

	    #region CONSTRUCTORS

		public GameContext() { }

		[JsonConstructor]
		public GameContext(string gameName, string timestamp, string date, Player player, List<TownData> townViews)
		{
			_gameName = new NString(gameName);
			_timestamp = new NString(timestamp);
			_date = new NString(date);
			_player = new NotificableField<Player> { Value = player };
			_townViews = new NList<TownData>() { Value = townViews };
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