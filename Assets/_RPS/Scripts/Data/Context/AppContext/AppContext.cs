using System.Collections.Generic;

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

        public static string TimeStamp => _gameContext.timestamp;
        public static Player Player => _gameContext.player;
        public static List<TownView> TownViews => _gameContext.townViews;
    }
}