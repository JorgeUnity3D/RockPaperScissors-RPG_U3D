using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Kapibara.RPS {
    public class CombatManagerRefactored : BaseManagerRefactored {
        
        [FoldoutGroup("SETUP")]
        public float timeBetweenSteps = 2f;
        
        [FoldoutGroup("DEBUG")] [SerializeField] [InlineEditor]
        private Player _player;
        [FoldoutGroup("DEBUG")]
        private List<Enemy> _enemies;
        [FoldoutGroup("DEBUG")] [SerializeField] [InlineEditor]
        private Enemy _currentEnemy;
        [FoldoutGroup("DEBUG")]
        private MapLevel _currentLevel;
        [FoldoutGroup("DEBUG")]
        private bool _debugCombat = false;
        [FoldoutGroup("DEBUG")]
        private bool _toNextStep = false;
        
        private CoroutineQueue _combatQueue;

        [HideInInspector] public UnityAction<Player> OnCombatSetUp;
        //Combat Phase 0 events
        [HideInInspector] public UnityAction OnCombatPhase0;
        //Combat Phase 1 events
        [HideInInspector] public UnityAction OnCombatPhase1Begins;
        [HideInInspector] public UnityAction<Enemy, bool> OnCombatPhase1Ends;
        //Combat Phase 2 events
        [HideInInspector] public UnityAction<Player> OnCombatPhase2Begins;
        [HideInInspector] public UnityAction<Player> OnCombatPhase2PlayerSelects;
        [HideInInspector] public UnityAction<Player> OnCombatPhase2Ends;
        //Combat Phase 3 events
        [HideInInspector] public UnityAction OnCombatPhase3Begins;
        [HideInInspector] public UnityAction<Player, Enemy> OnCombatPhase3Ends;
        //Combat Phase 4 events
        [HideInInspector] public UnityAction<Player, Enemy> OnCombatPhase4Begins;
        [HideInInspector] public UnityAction<Player, Enemy, int> OnCombatPhase4DamageCalculated;
        [HideInInspector] public UnityAction<Player, Enemy> OnCombatPhase4Ends;
        //Combat Phase 5 events
        [HideInInspector] public UnityAction<Player, Enemy> OnCombatPhase5Begins;
        [HideInInspector] public UnityAction<Player, Enemy> OnCombatPhase5Ends;
        //Update events
        [HideInInspector] public UnityAction<Player> OnPlayerUpdated;
        [HideInInspector] public UnityAction<Enemy> OnEnemyUpdated;


        private void Start() {
            ServiceLocator.Instance.GetService<TravelManagerRefactored>().OnLevelOpened += SetUpCombat;
        }

        public void SetUpCombat(MapLevel currentMapLevel) {
            Debug.Log("[CombatManager] SetUpCombat() -> currentMapLevel " + currentMapLevel.levelName);
            _combatQueue = new CoroutineQueue(this);
            GetCombatData(currentMapLevel);
            GetEnemy();
            EnqueueCombatPhases();
            OnCombatSetUp?.Invoke(_player);
        }

        private void GetCombatData(MapLevel currentMapLevel) {
            Debug.Log("[CombatManager] GetCombatData() -> currentMapLevel " + currentMapLevel.levelName);
            _currentLevel = currentMapLevel;
            _player = ServiceLocator.Instance.GetService<PlayerManagerRefactored>().Player;
            _enemies = new List<Enemy>();
            foreach (var levelEnemy in currentMapLevel.levelEnemies) {
                _enemies.Add(Instantiate(levelEnemy).data);
            }
        }

        public void StartCombat(bool debugCombat) {
            Debug.Log("[CombatManager] StartCombat() -> ");
            _debugCombat = debugCombat;
            _combatQueue.StartLoop();
        }

        public void PauseCombat() {
            // Timekeeper.instance.Clock("Combat").paused = !Timekeeper.instance.Clock("Combat").paused;
            // Debug.Log("[CombatManager] PauseCombat() -> currently " + Timekeeper.instance.Clock("Combat").paused);
        }

        public void StopCombat() {
            Debug.Log("[CombatManager] StopCombat() -> ");
            _combatQueue.StopLoop();
            _combatQueue = null;
            _currentEnemy = null;
        }

        private void GetEnemy() {
            _currentEnemy = _enemies[0];
            Debug.Log("[CombatManager] GetEnemy() -> " + _currentEnemy.ToString());
            OnEnemyUpdated?.Invoke(_currentEnemy);
        }

        private void KillEnemy() {
            _enemies.RemoveAt(0);
        }

        private void EnqueueCombatPhases(bool newCombat = true) {
            if (newCombat) {
                //Show Characters
                _combatQueue.EnqueueAction(CombatPhase0());
            }

            //UI Set Up + NPC Action Roll + PJ VS NPC Mentality Roll 
            _combatQueue.EnqueueAction(CombatPhase1());
            //PJ Selects Action
            _combatQueue.EnqueueAction(CombatPhase2());
            //NPC VS PJ Mentality Roll + Disable Thought Icons + Show Action Icons
            _combatQueue.EnqueueAction(CombatPhase3());
            //Show Damage Icons + Energy Bars + Change Action Holder Size + Apply Damage
            _combatQueue.EnqueueAction(CombatPhase4());
            //Check Combat Turn Results
            _combatQueue.EnqueueAction(CombatPhase5());
        }

        IEnumerator CombatPhase0() {
            GetEnemy();
            OnCombatPhase0?.Invoke();
            if (_debugCombat) {
                _toNextStep = false;
                Debug.Log("[CombatManager] CombatPhase0() -> Waiting NEXT STEP input");
                while (!_toNextStep) {
                    yield return null;
                }
            }
        }

        IEnumerator CombatPhase1() {
            //0. Combat Phase 1 Begins
            Debug.Log("[CombatManager] CombatPhase1() -> CombatPhase1 Begins");
            OnCombatPhase1Begins?.Invoke();

            //1. NPC Selects Action
            Debug.Log("[CombatManager] CombatPhase1() -> NPC Selects Action");
            _currentEnemy.ActionRoll();
            _currentEnemy.LanguageRoll();

            //2. PJ Mentality Roll
            Debug.Log("[CombatManager] CombatPhase1() -> PJ Mentality Roll");
            bool playerWinsMentalityRoll = _player.MentalityRollAgainst(_currentEnemy) > 0;

            //3. PJ VS NPC Mentality Check
            Debug.Log("[CombatManager] CombatPhase1() -> PJ VS NPC Mentality Check");
            if (playerWinsMentalityRoll) {
                Debug.Log("[CombatManager] CombatPhase1() -> Player Wins Mentality Check");
                //_currentEnemy.LanguageRoll();
                //TODO: mover a cuando se calculen los modificadores de daño
                //_currentEnemy.IncreaseMentality();
            } else {
                Debug.Log("[CombatManager] CombatPhase1() -> Player Loses Mentality Check");
            }

            //4. Combat Phase 1 Ends
            Debug.Log("[CombatManager] CombatPhase1() -> Finish Phase 1");
            OnCombatPhase1Ends?.Invoke(_currentEnemy, playerWinsMentalityRoll);
            if (_debugCombat) {
                _toNextStep = false;
                Debug.Log("[CombatManager] CombatPhase1() -> Waiting NEXT STEP input");
                while (!_toNextStep) {
                    yield return null;
                }
            }
        }

        IEnumerator CombatPhase2() {
            //4. Combat Phase 2 Begins
            OnCombatPhase2Begins?.Invoke(_player);
            Debug.Log("[CombatManager] CombatPhase2() -> PJ Selects Action");

            //5. PJ Selects Action
            Debug.Log("[CombatManager] CombatPhase2() -> Waiting Input...");
            while (_player.currentAction == Actions.NONE) {
                yield return null;
            }

            //6. Combat Phase 2 Ends
            Debug.Log("[CombatManager] CombatPhase2() -> Finish Phase 2");
            OnCombatPhase2Ends?.Invoke(_player);
            if (_debugCombat) {
                _toNextStep = false;
                Debug.Log("[CombatManager] CombatPhase2() -> Waiting NEXT STEP input");
                while (!_toNextStep) {
                    yield return null;
                }
            }
        }

        IEnumerator CombatPhase3() {
            //7. Combat Phase 3 Begins
            Debug.Log("[CombatManager] CombatPhase3() -> CombatPhase3 Begins");
            OnCombatPhase3Begins?.Invoke();

            //8. NPC Mentality Roll
            Debug.Log("[CombatManager] CombatPhase3() -> NPC Mentality Roll");
            bool enemyWinsMentalityRoll = _currentEnemy.MentalityRollAgainst(_player) > 0;

            //9. NPC VS PJ Mentality Check
            Debug.Log("[CombatManager] CombatPhase3() -> NPC VS PJ Mentality Check");
            if (enemyWinsMentalityRoll) {
                _currentEnemy.ActionRollAgainst(_player);
                //TODO: mover a cuando se calculen los modificadores de daño
                //_currentEnemy.ResetMentality();
                Debug.Log("[CombatManager] CombatPhase3() -> Enemy Wins Mentality Check");
            } else {
                Debug.Log("[CombatManager] CombatPhase3() -> Enemy Loses Mentality Check");
            }

            //10. Combat Phase 3 Ends
            OnCombatPhase3Ends?.Invoke(_player, _currentEnemy);
            if (_debugCombat) {
                _toNextStep = false;
                Debug.Log("[CombatManager] CombatPhase3() -> Waiting NEXT STEP input");
                while (!_toNextStep) {
                    yield return null;
                }
            }
        }

        IEnumerator CombatPhase4() {
            //11. Combat Phase 4 Begins
            Debug.Log("[CombatManager] CombatPhase4() -> CombatPhase4 Begins");
            OnCombatPhase4Begins?.Invoke(_player, _currentEnemy);

            //12. Pay Action costs
            Debug.Log("[CombatManager] CombatPhase4() -> Pay Action costs");
            _player.PayActionEnergyCost();
            _currentEnemy.PayActionEnergyCost();

            //13. ResultCompare
            Debug.Log("[CombatManager] CombatPhase4() -> Damage Results Compare");
            _player.TotalDamage(_currentEnemy);
            _currentEnemy.TotalDamage(_player);
            int combatResult = _player.currentTotalDamage - _currentEnemy.currentTotalDamage;
            Debug.Log("[CombatManager] CombatPhase4() -> _player.currentTotalDamage: " + _player.currentTotalDamage);
            Debug.Log("[CombatManager] CombatPhase4() -> _currentEnemy.currentTotalDamage: " + _currentEnemy.currentTotalDamage);
            Debug.Log("[CombatManager] CombatPhase4() -> combatResult(player-enemy): " + combatResult);
            OnCombatPhase4DamageCalculated?.Invoke(_player, _currentEnemy, combatResult);

            //14. Apply Damages and Thorns
            if (combatResult > 0) { //PJ Wins
                Debug.Log("[CombatManager] CombatPhase4() -> Enemy receives damage!");
                _currentEnemy.ReceiveDamage(Mathf.Abs(combatResult));
                // if (_player.currentAction != Actions.DEFENSE && _player.currentAction != Actions.ENERGY) {
                //     _currentEnemy.ReceiveDamage(combatResult);
                // }
            } else if (combatResult < 0) { //NPC Wins
                Debug.Log("[CombatManager] CombatPhase4() -> Player receives damage!");
                _player.ReceiveDamage(Mathf.Abs(combatResult));
                // if (_currentEnemy.currentAction != Actions.DEFENSE && _currentEnemy.currentAction != Actions.ENERGY) {
                //     _player.ReceiveDamage(combatResult);
                // }
            }

            if (_player.currentAction == Actions.DEFENSE && (_currentEnemy.currentAction == Actions.ROCK ||
                                                             _currentEnemy.currentAction == Actions.PAPER ||
                                                             _currentEnemy.currentAction == Actions.SCISSOR)) {
                Debug.Log("[CombatManager] CombatPhase4() -> Player is defending, applying THORNS!");
                _player.ThornsRoll();
                _currentEnemy.ReceiveDamage(_player.currentThornsRoll);
            }

            if (_currentEnemy.currentAction == Actions.DEFENSE && (_player.currentAction == Actions.ROCK ||
                                                                   _player.currentAction == Actions.PAPER ||
                                                                   _player.currentAction == Actions.SCISSOR)) {
                Debug.Log("[CombatManager] CombatPhase4() -> Enemy is defending, applying THORNS!");
                _currentEnemy.ThornsRoll();
                _player.ReceiveDamage(_currentEnemy.currentThornsRoll);
            }

            if (_player.currentAction == Actions.ENERGY) {
                Debug.Log("[CombatManager] CombatPhase4() -> Player is recovering energy!");
                _player.RecoverEnergy();
            }

            if (_currentEnemy.currentAction == Actions.ENERGY) {
                Debug.Log("[CombatManager] CombatPhase4() -> Enemy is recovering energy!");
                _currentEnemy.RecoverEnergy();
            }

            //15. Combat Phase 4 Ends
            Debug.Log("[CombatManager] CombatPhase4() -> CombatPhase4 Ends");
            OnCombatPhase4Ends?.Invoke(_player, _currentEnemy);
            //_player.CheckTraining();
            
            if (_debugCombat) {
                _toNextStep = false;
                Debug.Log("[CombatManager] CombatPhase4() -> Waiting NEXT STEP input");
                while (!_toNextStep) {
                    yield return null;
                }
            }
        }

        IEnumerator CombatPhase5() {
            //16. Combat Phase 5 Begins
            Debug.Log("[CombatManager] CombatPhase5() -> CombatPhase5 Begins");
            OnCombatPhase5Begins?.Invoke(_player, _currentEnemy);
            _player.currentAction = Actions.NONE;
            if (_currentEnemy.currentHealth > 0 && _player.currentHealth > 0) {
                Debug.Log("[CombatManager] CombatPhase5() -> Player and Enemy still alive, next round!");
                EnqueueCombatPhases(false);
            } else if (_currentEnemy.currentHealth <= 0) {
                Debug.Log("[CombatManager] CombatPhase5() -> Player stills alive and Enemy died, reward and next enemy!");
                _player.UpdateGold(_currentEnemy.RewardRoll());
                KillEnemy();
                GetEnemy();
                EnqueueCombatPhases();
            } else if (_player.currentHealth <= 0) {
                Debug.Log("[CombatManager] CombatPhase5() -> Player died, game over!");
                StopCombat();
                // GameManager.PlayerDiesEvent();
            }
            OnCombatPhase5Ends?.Invoke(_player, _currentEnemy);
            
            if (_debugCombat) {
                _toNextStep = false;
                Debug.Log("[CombatManager] CombatPhase5() -> Waiting NEXT STEP input");
                while (!_toNextStep) {
                    yield return null;
                }
            }
        }

        public void SetPlayerAction(Actions newAction) {
            if (_player.thinkingAction == Actions.NONE || _player.thinkingAction != newAction) {
                if (_player.HasEnoughEnergy(newAction)) {
                    _player.thinkingAction = newAction;
                } else {
                    _player.thinkingAction = Actions.NONE;
                }
            } else {
                _player.thinkingAction = Actions.NONE;
                _player.currentAction = newAction;
            }

            OnCombatPhase2PlayerSelects?.Invoke(_player);
        }


        public void NextStep() {
            Debug.Log("[CombatManager] NextStep() -> ");
            _toNextStep = true;
        }
    }
}