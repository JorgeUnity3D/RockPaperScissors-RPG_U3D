﻿using System.Collections.Generic;

namespace Kapibara.RPS
{
    public static class AppContext
    {
        private static GameContext _gameContext;
        public static GameContext GameContext
        {
	        get
	        {
		        return _gameContext;
	        }
            set
            {
                _gameContext = value;
            }
        }

        public static string TimeStamp => _gameContext.Timestamp;
        public static Player Player => _gameContext.Player;
        public static List<TownData> TownViews => _gameContext.TownViews;
    }
}