using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS {
    public class GameState {

        public int gameStateID;
        public Player playerState;
        public List<TownView> townViewsState;
        public int lastSaveTimestamp;

        public void InitializeGameState(Player player, List<TownView> townViews) {
            playerState = new Player(player);
            townViewsState = new List<TownView>(townViews);
        }
    }
}