using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Kapibara.RPS
{
    [Serializable]
    public class GameContext
    {
        //Generic Game Data
        public string gameName;
        public string timestamp;
        public string date;
        
        //Player Data
        public Player player;

        //Town Data
        public List<TownView> townViews;

        //Constructors
        public GameContext() { }

        [JsonConstructor]
        public GameContext(string gameName, string timestamp, string date, Player player, List<TownView> townViews)
        {
            this.gameName = gameName;
            this.timestamp = timestamp;
            this.date = date;
            this.player = player;
            this.townViews = townViews;
        }
        
        
        
        //ToString
        public override string ToString()
        {
	        return $"[GameContext] {gameName} --- {player.name}";
        }
    }
}