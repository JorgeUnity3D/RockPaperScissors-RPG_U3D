using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kapibara.RPS {
    public class PlayerManagerOld : BaseManager_old {
        // public Player playerDataObject;
        [HideInInspector] public Player player;

        // //Singleton
        // private static PlayerManager _instance;
        // [HideInInspector] public static PlayerManager Instance { get { return _instance; } }
        // private void Awake() {
        //     if (_instance != null && _instance != this) { Destroy(this.gameObject); } else { _instance = this; }
        //     DontDestroyOnLoad(this.gameObject);
        // }

        private void Start() {
            // player = Instantiate(playerDataObject);
            Debug.Log(player.currentGold);
            player.UpdateGold(10);
            Debug.Log(player.currentGold);
        }
    }
}