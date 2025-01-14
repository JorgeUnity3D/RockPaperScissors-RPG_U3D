using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kapibara.RPS {
    [RequireComponent(typeof(HouseUIController))]
    public class HouseManagerRefactored : BaseManagerRefactored
    {

        Player _player;

        void Awake()
        {
            _player = ServiceLocator.Instance.GetService<PlayerManagerRefactored>().Player;
        }

        public int PlayerStat(Stats currentStat) {
            int playerStat = 0;
            switch (currentStat) {
                case Stats.HEALTH :
                    playerStat = _player.maxHealth;
                    break;
                case Stats.MENTALITY :
                    playerStat = _player.mentality;
                    break;
                case Stats.ROCK :
                    playerStat = _player.rock;
                    break;
                case Stats.PAPER :
                    playerStat = _player.paper;
                    break;
                case Stats.SCISSOR :
                    playerStat = _player.scissor;
                    break;
                case Stats.DEFENSE :
                    playerStat = _player.defense;
                    break;
                case Stats.THORNS :
                    playerStat = _player.thorns;
                    break;
                case Stats.ENERGY_BASE :
                    playerStat = _player.maxEnergy;
                    break;
                case Stats.ENERGY_RECOVERY :
                    playerStat = _player.energyRecovery;
                    break;
                case Stats.CRIT :
                    playerStat = _player.crit;
                    break;
                case Stats.SUPERPOWER :
                    playerStat = _player.superpower;
                    break;
            }
            return playerStat;
        }
        
        public List<TrainingStat> TrainingStats {
            get {
                return ServiceLocator.Instance.GetService<TrainingHouseManagerRefactored>().TrainingStats;
            }
        }

        public TrainingStat TrainingStat(Stats currentStat) {
            return TrainingStats.Find((ts) => ts.stat == currentStat);
        }
        
    }
}