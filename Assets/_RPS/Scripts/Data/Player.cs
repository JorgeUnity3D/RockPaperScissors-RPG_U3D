using System;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS {
    
    [Serializable]
    public class Player : Character {
        [VerticalGroup("Stats")] [HorizontalGroup("Stats/Gold", MaxWidth = 200f)] [LabelWidth(100f)]
        public int currentGold;

        public Stats trainingStat;
        
        public Player(Player player) { }
        
        public Player(string playername)
        {
            name = playername;
            currentGold = 100;
        }
        
        [JsonConstructor]
        public Player(int currentGold, Stats trainingStat)
        {
            this.currentGold = currentGold;
            this.trainingStat = trainingStat;
        }

        // public override bool IsPlayer() {
        //     return true;
        // }

        #region ROLLS

        public override int MentalityRollAgainst(Character other) {
            var mentalityRoll = VariabilityRoll() + this.mentality - ((Enemy) other).storedMentality;
            Debug.Log("[Player] MentalityRollAgainst() -> mentalityRoll: " + mentalityRoll);
            return mentalityRoll;
        }

        #endregion

        #region STATS

        public void UpdateGold(int gold) {
            this.currentGold += this.isSuperAction ? gold * 2 : gold;
        }

        #endregion
    }
}