using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Kapibara.RPS {
    public class CombatManagerOld : BaseManager_old {
        public TextMeshProUGUI logText;

        public float timeBetweenSteps = 1f;

        private CombatUIManagerOld _combatUIManagerOld;
        private CoroutineQueue _combatQueue;
        private Player _player;
        public Enemy _currentEnemy;
        private int currentIndex;

        #region GAME STATES

        public override void OnInitializeGameEvent() {
            _combatUIManagerOld = ServiceLocator.Instance.GetService<CombatUIManagerOld>();
            _combatQueue = new CoroutineQueue(this);
        }

        #endregion

        #region COMBAT PHASES OVERRIDES

        public override void OnCombatPreparationEvent() {
            logText.text += "\n[CombatManager][OnCombatPreparationEvent]";
            _player = GameManager.Player;
            List<Enemy> enemies = ServiceLocator.Instance.GetService<EnemiesManagerOld>().CurrentEnemies;
            _currentEnemy = _currentEnemy == null ? enemies[0] : enemies[enemies.IndexOf(_currentEnemy) + 1];
            _combatUIManagerOld.SetEnemyImage(_currentEnemy);
            _combatUIManagerOld.SetHealthBar(_player);
            _combatUIManagerOld.SetEnergyBar(_player);
            _combatUIManagerOld.SetHealthBar(_currentEnemy);
            _combatUIManagerOld.SetEnergyBar(_currentEnemy);
            logText.text += "\n[CombatManager][PlayerHealth][" + _player.currentHealth + "] --- vs --- [EnemyHealth][" +
                            _currentEnemy.currentHealth + "]";
            EnqueueCombatPhases();
            GameManager.CombatStartsEvent();
        }

        public override void OnCombatStartsEvent() {
            logText.text = "\n[CombatManager][OnCombatStartsEvent]";
            _combatQueue.StartLoop();
        }

        public override void OnCombatNextTurnEvent() {
            logText.text = "\n[CombatManager][OnCombatNextTurnEvent]";
            _player.ResetRolls();
            _currentEnemy.ResetRolls();
            _combatUIManagerOld.SetThoughtIcon(_player);
            _combatUIManagerOld.SetThoughtIcon(_currentEnemy);
            _combatUIManagerOld.ShowActionIcon(_player, false);
            _combatUIManagerOld.ShowActionIcon(_currentEnemy, false);
            _combatQueue.StopLoop();
            EnqueueCombatPhases();
            _combatQueue.StartLoop();
        }

        private void EnqueueCombatPhases() {
            _combatQueue.EnqueueAction(CombatPhase1());
            _combatQueue.EnqueueWait(timeBetweenSteps);
            _combatQueue.EnqueueAction(CombatPhase2());
            _combatQueue.EnqueueWait(timeBetweenSteps);
            _combatQueue.EnqueueAction(CombatPhase3());
            _combatQueue.EnqueueWait(timeBetweenSteps);
            _combatQueue.EnqueueAction(CombatPhase4());
            _combatQueue.EnqueueWait(timeBetweenSteps);
        }

        //Combat Phases
        IEnumerator CombatPhase1() {
            _combatUIManagerOld.EnableActionButtons(false);
            //logText.text = "\n[CombatManager][CombatPhase1]";
            Debug.Log("[CombatManager][CombatPhase1]");
            //1. NPC Selects Action
            _currentEnemy.LanguageRoll();
            _currentEnemy.ActionRoll();
            Debug.Log("[CombatManager][CombatPhase1][EnemyActionRoll][" + _currentEnemy.currentAction + "] -- [" +
                      _currentEnemy.currentLanguage + "]");
            //2. PJ Mentality Roll
            int playerMentalityRoll = _player.MentalityRollAgainst(_currentEnemy);
            Debug.Log("[CombatManager][CombatPhase1][PlayerMentalityRoll][" + playerMentalityRoll + "]");
            //2. PJ VS NPC Mentality Check        
            if (playerMentalityRoll > 0) {
                _currentEnemy.LanguageRoll();
                _currentEnemy.IncreaseMentality();
                Debug.Log("[CombatManager][CombatPhase1][EnemyLanguageRoll][" + _currentEnemy.currentLanguage + "]");
            }

            yield return null;
            _combatUIManagerOld.SetThoughtIcon(_currentEnemy, playerMentalityRoll <= 0);
            //logText.text = "\n[CombatManager][EnemyRolls]["+_currentEnemy.currentAction+ "] -- ["+ _currentEnemy.currentLanguage + "]";
            yield return new WaitForSeconds(timeBetweenSteps);
        }

        IEnumerator CombatPhase2() {
            //logText.text = "\n[CombatManager][CombatPhase2]";
            Debug.Log("[CombatManager][CombatPhase2]");
            //3. PJ Selects Action
            _combatUIManagerOld.EnableActionButtons(true);
            while (_player.currentAction == Actions.NONE) {
                //logText.text = "\n[CombatManager][CombatPhase2][Waiting Input]";
                Debug.Log("[CombatManager][CombatPhase2][Waiting Input]");
                yield return null;
            }

            yield return null;
            Debug.Log("[CombatManager][CombatPhase1][PlayerActionSelection][" + _player.currentAction + "]");
            _combatUIManagerOld.SetThoughtIcon(_player);
            yield return new WaitForSeconds(1f);
            //4. NPC Mentality Roll
            int enemyMentalityRoll = _currentEnemy.MentalityRollAgainst(_player);
            Debug.Log("[CombatManager][CombatPhase2][enemyMentalityRoll][" + enemyMentalityRoll + "]");
            //4. NPC VS PJ Mentality Check
            if (enemyMentalityRoll > 0) {
                _currentEnemy.ActionRollAgainst(_player);
                _currentEnemy.ResetMentality();
                Debug.Log("[CombatManager][CombatPhase2][ChangedActionRollTo][" + _currentEnemy.currentAction + "]");
            }

            yield return null;
            _combatUIManagerOld.SetThoughtIcon(_currentEnemy);
            yield return new WaitForSeconds(timeBetweenSteps);
        }

        IEnumerator CombatPhase3() {
            //logText.text = "[CombatManager][CombatPhase3]";
            Debug.Log("[CombatManager][CombatPhase3]");
            //1. Pay Action costs
            //_player.PayActionEnergyCost();
            //_currentEnemy.PayActionEnergyCost();
            //2. Show Action Icons
            yield return null;
            _combatUIManagerOld.SetActionIcon(_player);
            _combatUIManagerOld.SetActionIcon(_currentEnemy);
            //_combatUIManager.ShowActionIcon(_player, true);
            //_combatUIManager.ShowActionIcon(_currentEnemy, true);
            yield return new WaitForSeconds(timeBetweenSteps);

            //3. ResultCompare
            Debug.Log("[CombatManager][CombatPhase4][Player TotalDamageRolls]");
            _player.TotalDamage(_currentEnemy);
            Debug.Log("[CombatManager][CombatPhase4][Enemy TotalDamageRolls]");
            _currentEnemy.TotalDamage(_player);
            _combatUIManagerOld.SetDamageText(_player);
            _combatUIManagerOld.SetDamageText(_currentEnemy);
            int combatResult = _player.currentTotalDamage - _currentEnemy.currentTotalDamage;
            _combatUIManagerOld.ScaleActionHolder(combatResult);
            //4. Apply Damages and Thorns
            if (combatResult > 0) { //PJ Wins
                if (_player.currentAction != Actions.DEFENSE && _player.currentAction != Actions.ENERGY) {
                    _currentEnemy.ReceiveDamage(combatResult);
                }
            } else if (combatResult < 0) { //NPC Wins
                if (_currentEnemy.currentAction != Actions.DEFENSE && _currentEnemy.currentAction != Actions.ENERGY) {
                    _player.ReceiveDamage(combatResult);
                }
            }

            if (_player.currentAction == Actions.DEFENSE && (_currentEnemy.currentAction == Actions.ROCK ||
                                                             _currentEnemy.currentAction == Actions.PAPER ||
                                                             _currentEnemy.currentAction == Actions.SCISSOR)) {
                _player.ThornsRoll();
                _currentEnemy.ReceiveDamage(_player.currentThornsRoll);
            }

            if (_currentEnemy.currentAction == Actions.DEFENSE && (_player.currentAction == Actions.ROCK ||
                                                                   _player.currentAction == Actions.PAPER ||
                                                                   _player.currentAction == Actions.SCISSOR)) {
                _currentEnemy.ThornsRoll();
                _player.ReceiveDamage(_currentEnemy.currentThornsRoll);
            }

            if (_player.currentAction == Actions.ENERGY) {
                _player.RecoverEnergy();
            }

            if (_currentEnemy.currentAction == Actions.ENERGY) {
                _currentEnemy.RecoverEnergy();
            }

            //5. Update Bars
            yield return null;
            _combatUIManagerOld.SetHealthBar(_player);
            _combatUIManagerOld.SetHealthBar(_currentEnemy);
            _combatUIManagerOld.SetEnergyBar(_player);
            _combatUIManagerOld.SetEnergyBar(_currentEnemy);
            _player.CheckTraining();
            yield return new WaitForSeconds(timeBetweenSteps);
        }

        IEnumerator CombatPhase4() {
            logText.text = "[CombatManager][CombatPhase4]";
            if (_currentEnemy.currentHealth > 0 && _player.currentHealth > 0) {
                //GameManager.EnemyDiesEvent();
                GameManager.CombatNextTurnEvent();
                yield return null;
            } else if (_currentEnemy.currentHealth <= 0) {
                _player.UpdateGold(_currentEnemy.RewardRoll());
                GameManager.CombatPreparationEvent();
                yield return null;
            } else if (_player.currentHealth <= 0) {
                GameManager.PlayerDiesEvent();
                yield return null;
            }

            yield return null;
        }

        #endregion
    }
}