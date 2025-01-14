using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS {
    public class Enemy : Character {

        [VerticalGroup("Stats")] [HorizontalGroup("Stats/Mentality", MaxWidth = 200f)] [LabelWidth(100f)]
        public int mentalityMod;

        [VerticalGroup("Stats")] [HorizontalGroup("Stats/Mentality", MaxWidth = 200f)] [LabelWidth(100f)]
        public int storedMentality;

        [VerticalGroup("Stats")] [HorizontalGroup("Stats/Rock", MaxWidth = 200f)] [LabelWidth(100f)]
        public int rockProb;

        [VerticalGroup("Stats")] [HorizontalGroup("Stats/Paper", MaxWidth = 200f)] [LabelWidth(100f)]
        public int paperProb;

        [VerticalGroup("Stats")] [HorizontalGroup("Stats/Scissor", MaxWidth = 200f)] [LabelWidth(100f)]
        public int scissorProb;

        [VerticalGroup("Stats")] [HorizontalGroup("Stats/Defense1", MaxWidth = 200f)] [LabelWidth(100f)]
        public int defenseProb;

        [VerticalGroup("Stats")] [HorizontalGroup("Stats/Energy2", MaxWidth = 200f)] [LabelWidth(100f)]
        public int energyProb;

        [VerticalGroup("Stats")] [HorizontalGroup("Stats/Gold", MaxWidth = 200f)] [LabelWidth(100f)]
        public int minGold;

        [VerticalGroup("Stats")] [HorizontalGroup("Stats/Gold", MaxWidth = 200f)] [LabelWidth(100f)]
        public int maxGold;

        
        #region TESTING
        public Enemy( /*string path, */ int randStone, int randPaper, int randScissor, int randDefense, int randMent) {
            // this.spritePath = path;
            //GENERAL STATS
            this.level = 0;
            this.currentHealth = 10;
            this.maxHealth = 10;
            this.mentality = 0 + randMent;
            this.rock = 3 + randStone;
            this.rockCost = 10;
            this.paper = 3 + randPaper;
            this.paperCost = 10;
            this.scissor = 3 + randScissor;
            this.scissorCost = 10;
            this.defense = 3 + randDefense;
            this.defenseCost = 10;
            this.thorns = 0;
            this.currentEnergy = 10;
            this.initialEnergy = 10;
            this.maxEnergy = 100;
            this.energyRecovery = 5;
            this.crit = 0;
            this.superpower = 0;
            //ENEMY STATS
            this.mentalityMod = 2;
            this.storedMentality = this.mentality;
            this.minGold = 5;
            this.maxGold = 20;
            SplitActionProbabilities();
            this.actionSkillLevel = new Dictionary<Actions, int>();
            this.actionSkillLevel.Add(Actions.ROCK, rock);
            this.actionSkillLevel.Add(Actions.PAPER, paper);
            this.actionSkillLevel.Add(Actions.SCISSOR, scissor);
            this.actionSkillLevel.Add(Actions.ENERGY, 0);
            this.actionSkillLevel.Add(Actions.DEFENSE, 0);
        }

        private void SplitActionProbabilities() {
            this.rockProb = 0;
            this.paperProb = 0;
            this.scissorProb = 0;
            this.defenseProb = 0;
            this.energyProb = 0;
            for (int i = 0; i < 100; i++) {
                Actions randomAction = RNGGenerator.RandomEnumValue<Actions>();
                switch (randomAction) {
                    case Actions.ROCK:
                        this.rockProb++;
                        break;
                    case Actions.PAPER:
                        this.paperProb++;
                        break;
                    case Actions.SCISSOR:
                        this.scissorProb++;
                        break;
                    case Actions.DEFENSE:
                        this.defenseProb++;
                        break;
                    case Actions.ENERGY:
                        this.energyProb++;
                        break;
                    default:
                        break;
                }
            }
        }

        public void IncreaseMentality() {
            this.storedMentality += this.mentalityMod;
            Debug.Log("[Enemy] ActionRoll() -> this.storedMentality: " + this.storedMentality.ToString());
        }

        public void ResetMentality() {
            this.storedMentality = this.mentality;
        }

        #endregion

        #region ROLLS

        public override int MentalityRollAgainst(Character other) {
            int mentalityRoll = (VariabilityRoll() + this.storedMentality) - ((Player) other).mentality;
            Debug.Log("[Enemy] MentalityRollAgainst() -> mentalityRoll: " + mentalityRoll.ToString());
            return mentalityRoll;
        }

        public void ActionRoll() {
            //Basic ponderated Action Selection
            List<int> ponderatedActions = new List<int>();
            Actions[] attackTypes = (Actions[]) Enum.GetValues(typeof(Actions));
            foreach (Actions attackType in attackTypes) {
                int attackProbability = 0;
                switch (attackType) {
                    case Actions.ROCK:
                        attackProbability = this.rockProb;    
                        break;
                    case Actions.PAPER:
                        attackProbability = this.paperProb;
                        break;
                    case Actions.SCISSOR:
                        attackProbability = this.scissorProb;
                        break;
                    case Actions.DEFENSE:
                        attackProbability = this.defenseProb;
                        break;
                    case Actions.ENERGY:
                        attackProbability = this.energyProb;
                        break;
                }

                for (int i = 0; i < attackProbability; i++) {
                    int attackCost = 0;
                    switch (attackType) {
                        case Actions.ROCK:
                            attackCost = this.rockCost;    
                            break;
                        case Actions.PAPER:
                            attackCost = this.paperCost;
                            break;
                        case Actions.SCISSOR:
                            attackCost = this.scissorCost;
                            break;
                        case Actions.DEFENSE:
                            attackCost = this.defenseCost;
                            break;
                    }

                    if (this.currentEnergy >= attackCost) {
                        ponderatedActions.Add((int) attackType);                        
                    }
                }
            }
            //todo: check de coste de energia, si no da, cambiar accion a energia
            //todo: añadir gambits
            Actions chosenAttackType = (Actions) ponderatedActions[UnityEngine.Random.Range(0, ponderatedActions.Count)];
            this.currentAction = chosenAttackType;
            this.thinkingAction = chosenAttackType;
            Debug.Log("[Enemy] ActionRoll() -> chosenAttackType: " + chosenAttackType.ToString());
            Debug.Log("[Enemy] ActionRoll() -> this.currentAction: " + this.currentAction.ToString());
            Debug.Log("[Enemy] ActionRoll() -> this.thinkingAction: " + this.thinkingAction.ToString());
        }

        //TODO: Programar tirada de acción contra la acción concreta del Player
        public void ActionRollAgainst(Player player) {
            CompareActionsAndSetMultiplier(player);
            if (currentMultiplier > 1) { //Si NPC está ganando
                //1. comportamiento base -> si puedo critico, hago critico ya seleccionado
                //2. gambit -> "usar el mejor stat", "usar el mejor multiplicador", etc
                //--
                //1. gambit incluyen todo tipo de comportamientos, con prioridad
                /*
                 * class Gambit : ScriptableObject
                 *   bool useStats;
                 *   bool useMultiplier;
                 *   bool useWorstOption;
                 *   bool useHealthFromPlayer; //si true
                 *      int playerMin = 5;
                 *
                 * GambitBestCrit
                 *   useStats = true;
                 *   useMultiplier = true;
                 *   ... = false;
                 */
                //TODO: Si puede hacer super ataque, que cambie al mejor super ataque posible, en función de los gambits
                if (currentEnergy == maxEnergy) { 
                    //TODO: programar super ataque
                    return;
                } else {
                    return;    
                }
            } else {
                switch (player.currentAction) {
                    case Actions.ROCK:
                        if (this.currentEnergy >= this.paperCost) {
                            this.currentAction = Actions.PAPER;
                        }
                        
                        break;
                    case Actions.PAPER:
                        if (this.currentEnergy >= this.scissorCost) {
                            this.currentAction = Actions.SCISSOR;
                        }

                        break;
                    case Actions.SCISSOR:
                        if (this.currentEnergy >= this.rockCost) {
                            this.currentAction = Actions.ROCK;
                        }

                        break;
                    case Actions.ENERGY:
                        if (this.currentAction == Actions.DEFENSE || this.currentAction == Actions.ENERGY) {
                            //Buscar acción con mayor valor
                            int greaterValue = 0;
                            foreach (KeyValuePair<Actions, int> keyValuePair in this.actionSkillLevel) {
                                if (keyValuePair.Value > greaterValue) {
                                    greaterValue = keyValuePair.Value;
                                }
                            }
                            List<Actions> availableActions = (List<Actions>) this.actionSkillLevel
                                .Where(pair => pair.Value == greaterValue).Select(pair => pair.Key);
                            List<int> deleteIndexes = new List<int>();
                            
                            for (int i = 0; i < availableActions.Count; i++) {
                                switch (availableActions[i]) {
                                    case Actions.ROCK:
                                        if (this.currentEnergy < this.rockCost) {
                                            deleteIndexes.Add(i);
                                        }

                                        break;
                                    case Actions.PAPER:
                                        if (this.currentEnergy < this.paperCost) {
                                            deleteIndexes.Add(i);
                                        }

                                        break;
                                    case Actions.SCISSOR:
                                        if (this.currentEnergy < this.scissorCost) {
                                            deleteIndexes.Add(i);
                                        }

                                        break;
                                }
                            }

                            for (int i = 0; i < deleteIndexes.Count; i++) {
                                availableActions.RemoveAt(deleteIndexes[i]);
                            }

                            if (availableActions.Count != 0) {
                                this.currentAction =
                                    availableActions[RNGGenerator.RandomBetween(0, availableActions.Count - 1)];
                            }
                        }

                        break;
                }
            }

            Debug.Log("[Enemy] ActionRollAgainst() -> ");
        }

        public void LanguageRoll() {
            this.currentLanguage = this.languages[RNGGenerator.RandomBetween(0, this.languages.Count-1)].data;
            Debug.Log("[Enemy] LanguageRoll() -> this.currentLanguage: " + this.currentLanguage.language.ToString());
        }

        public int RewardRoll() {
            return RNGGenerator.RandomBetween(this.minGold, this.maxGold);
        }

        #endregion

        public override string ToString() {
            return "currentEnemy(RPSM): (" + this.rock + "-" +
                   this.paper + "-" + this.scissor + "-" + this.mentality+")";
        }
    }
}