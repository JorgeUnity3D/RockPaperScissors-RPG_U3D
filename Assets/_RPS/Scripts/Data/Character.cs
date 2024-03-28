using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Color = System.Drawing.Color;

namespace Kapibara.RPS {
    
    [Serializable]
    public abstract class Character {
        
        [VerticalGroup("Stats")] [HorizontalGroup("Stats/ID", MaxWidth = 200f)] [VerticalGroup("Stats/ID/Data")] [LabelWidth(100f)] 
        public string name;
        [VerticalGroup("Stats")] [HorizontalGroup("Stats/ID", MaxWidth = 200f)] [VerticalGroup("Stats/ID/Data")] [LabelWidth(100f)]
        public int level;

        [VerticalGroup("Stats")] [HorizontalGroup("Stats/ID", MarginLeft = 50f, MaxWidth = 50f)] [HideLabel] [PreviewField(Height = 80f)]
        public Sprite portrait;

        [VerticalGroup("Stats")] [HorizontalGroup("Stats/Health", MaxWidth = 200f)] [LabelWidth(100f)] 
        public int currentHealth;

        [VerticalGroup("Stats")] [HorizontalGroup("Stats/Health", MaxWidth = 200f)] [LabelWidth(100f)] 
        public int maxHealth;

        [VerticalGroup("Stats")] [HorizontalGroup("Stats/Mentality", MaxWidth = 200f)] [LabelWidth(100f)]
        public int mentality;

        [VerticalGroup("Stats")] [HorizontalGroup("Stats/Rock", MaxWidth = 200f)] [LabelWidth(100f)]
        public int rock;

        [VerticalGroup("Stats")] [HorizontalGroup("Stats/Rock", MaxWidth = 200f)] [LabelWidth(100f)]
        public int rockCost;

        [VerticalGroup("Stats")] [HorizontalGroup("Stats/Paper", MaxWidth = 200f)] [LabelWidth(100f)]
        public int paper;

        [VerticalGroup("Stats")] [HorizontalGroup("Stats/Paper", MaxWidth = 200f)] [LabelWidth(100f)]
        public int paperCost;

        [VerticalGroup("Stats")] [HorizontalGroup("Stats/Scissor", MaxWidth = 200f)] [LabelWidth(100f)]
        public int scissor;

        [VerticalGroup("Stats")] [HorizontalGroup("Stats/Scissor", MaxWidth = 200f)] [LabelWidth(100f)]
        public int scissorCost;

        [VerticalGroup("Stats")] [HorizontalGroup("Stats/Defense1", MaxWidth = 200f)] [LabelWidth(100f)]
        public int defense;

        [VerticalGroup("Stats")] [HorizontalGroup("Stats/Defense1", MaxWidth = 200f)] [LabelWidth(100f)]
        public int defenseCost;

        [VerticalGroup("Stats")] [HorizontalGroup("Stats/Defense2", MaxWidth = 200f)] [LabelWidth(100f)]
        public int thorns;

        [VerticalGroup("Stats")] [HorizontalGroup("Stats/Energy1", MaxWidth = 200f)] [LabelWidth(100f)]
        public int currentEnergy;

        [VerticalGroup("Stats")] [HorizontalGroup("Stats/Energy1", MaxWidth = 200f)] [LabelWidth(100f)]
        public int maxEnergy;

        [VerticalGroup("Stats")] [HorizontalGroup("Stats/Energy2", MaxWidth = 200f)] [LabelWidth(100f)]
        public int initialEnergy;

        [VerticalGroup("Stats")] [HorizontalGroup("Stats/Energy2", MaxWidth = 200f)] [LabelWidth(100f)]
        public int energyRecovery;

        [VerticalGroup("Stats")] [HorizontalGroup("Stats/Crit", MaxWidth = 200f)] [LabelWidth(100f)]
        public int crit;

        [VerticalGroup("Stats")] [HorizontalGroup("Stats/Crit", MaxWidth = 200f)] [LabelWidth(100f)]
        public int superpower;

        

        
        [VerticalGroup("Stats")]
        [HorizontalGroup("Stats/Language1", MaxWidth = 400f)]
        [LabelWidth(150f)]
        [InlineEditor()]
        public LanguageDataObject commonLanguage;
        [VerticalGroup("Stats")]
        [HorizontalGroup("Stats/Language2", MaxWidth = 400f)]
        [LabelWidth(150f)]
        [InlineEditor()]
        [HideIf("IsPlayer")]
        public List<LanguageDataObject> languages;

        // public virtual bool IsPlayer() {
        //     //return GetType() == typeof(Player);
        //     return false;
        // }
        
        public bool IsPlayer {
            //return GetType() == typeof(Player);
            get {
                return GetType() == typeof(Player);
            } 
        }

        [HideInInspector] public Language currentLanguage;
        //
        [HideInInspector] public Actions currentAction;
        [HideInInspector] public Actions thinkingAction;

        public bool HasEnoughEnergy(Actions action) {
            return this.currentEnergy >= ActionCost(action);
        }
        [HideInInspector]
        public int ActionCost(Actions action) {
            int cost = 0;
            switch (action) {
                case Actions.ROCK:
                    cost = rockCost;
                    break;
                case Actions.PAPER:
                    cost = paperCost;
                    break;
                case Actions.SCISSOR:
                    cost = scissorCost;
                    break;
                case Actions.DEFENSE:
                    cost = defenseCost;
                    break;
                case Actions.ENERGY:
                    cost = -energyRecovery;
                    break;
            }

            return cost;
        }

        [HideInInspector] public bool isSuperAction;
        [HideInInspector] public int currentDamageRoll;
        [HideInInspector] public float currentMultiplier;
        [HideInInspector] public float currentCritBonus;
        [HideInInspector] public int currentTotalDamage;
        [HideInInspector] public int currentThornsRoll;
        [HideInInspector] public bool isTrainingSuccess;
        protected Dictionary<Actions, int> actionSkillLevel;

        #region ROLLS

        public virtual void ResetRolls() {
            this.currentAction = Actions.NONE;
            this.isSuperAction = false;
            this.currentDamageRoll = 0;
            this.currentMultiplier = 0;
            this.currentCritBonus = 0;
            this.currentTotalDamage = 0;
            this.currentThornsRoll = 0;
            this.isTrainingSuccess = false;
        }

        public abstract int MentalityRollAgainst(Character other);

        public virtual int VariabilityRoll() {
            //1D(level * 0.16 + 4)
            int max = Mathf.FloorToInt(this.level * 0.16f + 4);
            int variability = RNGGenerator.Roll1D(max);
            return variability;
        }

        public virtual int TotalDamage(Character other) {
            CompareActionsAndSetMultiplier(other);
            Debug.Log("[TotalDamage][CompareActionsAndSetMultiplier]" + this.currentMultiplier);
            DamageRoll();
            Debug.Log("[TotalDamage][DamageRoll]" + this.currentDamageRoll);
            int modifiedDamage = (int) (this.currentDamageRoll * this.currentMultiplier);
            Debug.Log("[TotalDamage][modifiedDamage]" + modifiedDamage);
            CritRoll();
            Debug.Log("[TotalDamage][CritRoll]" + this.currentCritBonus);
            int totalDamage = (int) (modifiedDamage + this.currentCritBonus);
            Debug.Log("[TotalDamage][totalDamage]" + totalDamage);
            this.currentTotalDamage = totalDamage;
            return totalDamage;
        }

        public virtual void DamageRoll() {
            int damage = 0;
            if (this.currentAction != Actions.ENERGY) {
                int variability = VariabilityRoll();
                int attribute = 0;
                switch (this.currentAction) {
                    case Actions.ROCK:
                        attribute = this.rock;
                        break;
                    case Actions.PAPER:
                        attribute = this.paper;
                        break;
                    case Actions.SCISSOR:
                        attribute = this.scissor;
                        break;
                    case Actions.DEFENSE:
                        attribute = this.defense;
                        break;
                }

                damage = variability + attribute;
            }

            this.currentDamageRoll = damage;
        }

        public virtual void CritRoll() {
            float critBonus = 0;
            int dice1d100Roll = RNGGenerator.Roll1D(100);
            if (dice1d100Roll <= this.crit) {
                switch (this.currentAction) {
                    case Actions.ROCK:
                        critBonus = this.rock * 0.5f;
                        break;
                    case Actions.PAPER:
                        critBonus = this.paper * 0.5f;
                        break;
                    case Actions.SCISSOR:
                        critBonus = this.scissor * 0.5f;
                        break;
                    case Actions.DEFENSE:
                        critBonus = this.defense * 0.5f;
                        break;
                    case Actions.ENERGY:
                        critBonus = this.energyRecovery * 0.5f;
                        break;
                }
            }

            this.currentCritBonus = critBonus;
        }

        public virtual void ThornsRoll() {
            int thornRoll = 0;
            thornRoll += (int) (this.currentMultiplier * this.thorns) + this.thorns + (VariabilityRoll() - 10);
            if (this.currentCritBonus > 0) {
                thornRoll += (int) (this.thorns * 0.5f);
            }

            this.currentThornsRoll = thornRoll;
        }
        //public virtual bool CheckThorns(Character other) {
        //    if (other.currentAction != Actions.DEFENSE && other.currentAction != Actions.ENERGY) {
        //        if (this.currentThornsRoll > 0) {
        //            return true;
        //        } else {
        //            return false;
        //        }
        //    } else {
        //        return false;
        //    }
        //}

        public void CompareActionsAndSetMultiplier(Character other) {
            float multiplier = 1f;
            if (other.currentAction != this.currentAction && other.currentAction != Actions.DEFENSE) {
                //accion enemigo diferente a accion jugador Y enemigo no elige defensa
                switch (this.currentAction) {
                    case Actions.ROCK:
                        if (other.currentAction == Actions.SCISSOR || other.currentAction == Actions.ENERGY) {
                            //Jugador gana O enemigo elige Energia                            
                            multiplier = 1.2f;
                        } else if (other.currentAction == Actions.PAPER) { //Jugador pierde
                            multiplier = 0.8f;
                        }

                        break;
                    case Actions.PAPER:
                        if (other.currentAction == Actions.ROCK || other.currentAction == Actions.ENERGY) {
                            multiplier = 1.2f;
                        } else if (other.currentAction == Actions.SCISSOR) {
                            multiplier = 0.8f;
                        }

                        break;
                    case Actions.SCISSOR:
                        if (other.currentAction == Actions.PAPER || other.currentAction == Actions.ENERGY) {
                            multiplier = 1.2f;
                        } else if (other.currentAction == Actions.ROCK) {
                            multiplier = 0.8f;
                        }

                        break;
                    case Actions.DEFENSE:
                        if (other.currentAction != Actions.ENERGY) { //Si enemigo elige ataque Y jugador elige defensa
                            multiplier = 1.5f;
                        }

                        break;
                    case Actions.ENERGY:
                        if (other.currentAction != Actions.ENERGY) { //Si enemigo elige ataque Y jugador elige energia
                            multiplier = 0.8f;
                        }

                        break;
                }
            }

            if (this.isSuperAction && this.currentAction != Actions.ENERGY) {
                //Si jugador super accion Y no elige energia
                //Si enemigo super accion Y accion enemigo diferente a accion jugador Y no elige defensa Y no elige energia
                if (other.isSuperAction && other.currentAction != this.currentAction &&
                    other.currentAction != Actions.DEFENSE && other.currentAction != Actions.ENERGY) {
                    switch (this.currentAction) {
                        case Actions.ROCK:
                            if (other.currentAction == Actions.SCISSOR) {
                                multiplier = 2.2f;
                            } else if (other.currentAction == Actions.PAPER) {
                                multiplier = 1.8f;
                            }

                            break;
                        case Actions.PAPER:
                            if (other.currentAction == Actions.ROCK) {
                                multiplier = 2.2f;
                            } else if (other.currentAction == Actions.SCISSOR) {
                                multiplier = 1.8f;
                            }

                            break;
                        case Actions.SCISSOR:
                            if (other.currentAction == Actions.PAPER) {
                                multiplier = 2.2f;
                            } else if (other.currentAction == Actions.ROCK) {
                                multiplier = 1.8f;
                            }

                            break;
                        case Actions.DEFENSE:
                            multiplier = 3f;
                            break;
                    }
                } else {
                    multiplier = 2f;
                }
            }

            this.currentMultiplier = multiplier;
        }

        #endregion

        #region STATS

        public void HealDamage(int damage) {
            this.currentHealth += damage;
            if (this.currentHealth > this.maxHealth) {
                this.currentHealth = this.maxHealth;
            }
        }

        public void ReceiveDamage(int damage) {
            if (damage > 0) {
                this.currentHealth -= damage;
                if (this.currentHealth < 0) {
                    this.currentHealth = 0;
                }
            }
        }

        internal void PayActionEnergyCost() {
            switch (currentAction) {
                case Actions.ROCK:
                    this.currentEnergy -= rockCost;
                    break;
                case Actions.PAPER:
                    this.currentEnergy -= paperCost;
                    break;
                case Actions.SCISSOR:
                    this.currentEnergy -= scissorCost;
                    break;
                case Actions.DEFENSE:
                    this.currentEnergy -= defenseCost;
                    break;
            }
        }

        public virtual void RecoverEnergy() {
            this.currentEnergy += this.energyRecovery;
        }

        public void CheckTraining() {
            int experience = 0;
            if (this.isTrainingSuccess) {
                if (this.currentMultiplier == 1.2f) {
                    experience++;
                } else if (this.currentMultiplier >= 2f) {
                    experience += 2;
                }

                if (currentCritBonus > 0) {
                    experience++;
                }
            }
            //TODO: AddExperience
        }

        #endregion
    }
}