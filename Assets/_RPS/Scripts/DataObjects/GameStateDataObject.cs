using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS {
    [Serializable]
    [CreateAssetMenu(fileName = "GameState", menuName = "RPSRPG/GameState")]
    public class GameStateDataObject : SerializedScriptableObject {
    
        [FoldoutGroup(RPSEditorConst.DATA)] [SerializeField]
        public int gameStateID;

        [FoldoutGroup(RPSEditorConst.DATA)] [SerializeField]
        public bool isNewGame;
        
        [FoldoutGroup(RPSEditorConst.DATA)] [SerializeField] [InlineEditor]
        private PlayerDataObject _playerDataObject;

        [FoldoutGroup(RPSEditorConst.DATA)] [SerializeField] [InlineEditor]
        private TownViewDataObject _townViewDataObject;

        [FoldoutGroup(RPSEditorConst.DEBUG)] [ReadOnly]
        public GameState data;

        #region PROPERTIES
        public Sprite DefaultPlayerSprite {
            get {
                return _playerDataObject.data.portrait;
            }
        }
        public Sprite CurrentPlayerSprite {
            get {
                return data.playerState.portrait;
            }
        }
        public string DefaultPlayerName {
            get {
                return _playerDataObject.data.name;
            }
        }
        public string CurrentPlayerName {
            get {
                return data.playerState.name;
            }
        }
        public int DefaultPlayerGold {
            get {
                return _playerDataObject.data.currentGold;
            }
        }
        public int CurrentPlayerGold {
            get {
                return data.playerState.currentGold;
            }
        }
        #endregion

        public void InitializeGameState() {
//            data.InitializeGameState(_playerDataObject.data, _townViewDataObject.Data);
        }
    }
}