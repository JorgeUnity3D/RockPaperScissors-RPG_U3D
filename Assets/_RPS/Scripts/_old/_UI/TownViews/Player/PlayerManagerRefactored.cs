using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Kapibara.RPS {
    public class PlayerManagerRefactored : BaseManagerRefactored {
        [FoldoutGroup("DATA")] [SerializeField] [InlineEditor] [ValidateInput("@playerDataObject != null")]
        private PlayerDataObject playerDataObject;

        [FoldoutGroup("DEBUG")] 
        private Player _player;


        [HideInInspector] public UnityAction<int> OnCoinsUpdated;
        [HideInInspector] public UnityAction<int> OnHealthUpdated;
        [HideInInspector] public UnityAction<int> OnEnergyhUpdated;

        [HideInInspector] public Player Player {
            get { return _player; }
        }

        public override void InitializeData() {
            _player = Instantiate(playerDataObject).data;
        }

        private void Start() {
            UpdateCoins(0);
        }

        public void UpdateCoins(int amount) {
            int deltaCoins = _player.currentGold + amount;
            _player.currentGold = (deltaCoins) < 0 ? 0 : deltaCoins;
            OnCoinsUpdated?.Invoke(_player.currentGold);
        }

        public void UpdateHealth(int amount, bool isDamage = true) {
            if (isDamage) {
                _player.ReceiveDamage(amount);
            } else {
                _player.HealDamage(amount);
            }

            OnHealthUpdated?.Invoke(_player.currentHealth);
        }

        public void UpdateEnergy(int amount) { }
    }
}