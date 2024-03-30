using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS
{

	[Serializable]
	public class Player
	{
		#region FIELDS

		//General
		[SerializeField] private NString _name;
		[SerializeField] private NInt _level;
		[SerializeField] private NInt _gold;
		//Health
		[SerializeField] private NInt _currentHealth;
		[SerializeField] private NInt _maxHealth;
		//Mentality
		[SerializeField] private NInt _mentality;
		//Rock
		[SerializeField] private NAttribute _rock;
		[SerializeField] private NInt _rockCost;
		//Paper
		[SerializeField] private NAttribute _paper;
		[SerializeField] private NInt _paperCost;
		//Scissors
		[SerializeField] private NAttribute _scissor;
		[SerializeField] private NInt _scissorCost;
		//Defense
		[SerializeField] private NAttribute _defense;
		[SerializeField] private NInt _defenseCost;
		[SerializeField] private NInt _thorns;
		//Energy
		[SerializeField] private NInt _currentEnergy;
		[SerializeField] private NInt _maxEnergy;
		[SerializeField] private NInt _initialEnergy;
		[SerializeField] private NInt _energyRecovery;
		//Crit & SuperPower
		[SerializeField] private NInt _crit;
		[SerializeField] private NInt _superpower;

		#endregion

		#region PROPERTIES

		public string Name
		{
			get => _name.Value;
			set => _name.Value = value;
		}
		
		public int Level
		{
			get => _level.Value;
			set => _level.Value = value;
		}

		public int Gold
		{
			get => _gold.Value;
			set => _gold.Value = value;
		}

		public int CurrentHealth
		{
			get => _currentHealth.Value;
			set => _currentHealth.Value = value;
		}

		public int MaxHealth
		{
			get => _maxHealth.Value;
			set => _maxHealth.Value = value;
		}

		public int Mentality
		{
			get => _mentality.Value;
			set => _mentality.Value = value;
		}

		public Attribute Rock
		{
			get => _rock.Value;
			set => _rock.Value = value;
		}
		
		public int RockCost
		{
			get => _rockCost.Value;
			set => _rockCost.Value = value;
		}

		public Attribute Paper
		{
			get => _paper.Value;
			set => _paper.Value = value;
		}

		public int PaperCost
		{
			get => _paperCost.Value;
			set => _paperCost.Value = value;
		}

		public Attribute Scissor
		{
			get => _scissor.Value;
			set => _scissor.Value = value;
		}

		public int ScissorCost
		{
			get => _scissorCost.Value;
			set => _scissorCost.Value = value;
		}

		public Attribute Defense
		{
			get => _defense.Value;
			set => _defense.Value = value;
		}

		public int DefenseCost
		{
			get => _defenseCost.Value;
			set => _defenseCost.Value = value;
		}

		public int Thorns
		{
			get => _thorns.Value;
			set => _thorns.Value = value;
		}

		public int CurrentEnergy
		{
			get => _currentEnergy.Value;
			set => _currentEnergy.Value = value;
		}

		public int MaxEnergy
		{
			get => _maxEnergy.Value;
			set => _maxEnergy.Value = value;
		}

		public int InitialEnergy
		{
			get => _initialEnergy.Value;
			set => _initialEnergy.Value = value;
		}

		public int EnergyRecovery
		{
			get => _energyRecovery.Value;
			set => _energyRecovery.Value = value;
		}

		public int Crit
		{
			get => _crit.Value;
			set => _crit.Value = value;
		}

		public int Superpower
		{
			get => _superpower.Value;
			set => _superpower.Value = value;
		}

		public List<Attribute> Attributes
		{
			get
			{
				return new List<Attribute>
				{
					_rock.Value,
					_paper.Value,
					_scissor.Value,
					_defense.Value
				};
			}
		}
		
		#endregion

		#region CONSTRUCTORS

		public Player(Player player) { }

		public Player(string playername)
		{
			_name = new NString(playername);
			_level = new NInt(1);
			_gold = new NInt(100);
			_rock = new NAttribute(3);
			_rock.Value.AddModifier(new TrainingModifier(1));
			_rock.Value.AddModifier(new SkillTreeModifier(2));
			_paper = new NAttribute(4);
			_paper.Value.AddModifier(new TrainingModifier(3));
			_paper.Value.AddModifier(new SkillTreeModifier(4));
			_scissor = new NAttribute(5);
			_scissor.Value.AddModifier(new TrainingModifier(5));
			_scissor.Value.AddModifier(new SkillTreeModifier(6));
			_currentHealth = new NInt(10);
			_maxHealth = new NInt(1 );
			//
			_mentality = new NInt(1);
			_rockCost = new NInt(5);
			_paperCost = new NInt(5);
			_scissorCost = new NInt(5);
			_defense = new NAttribute(0);
			_defenseCost = new NInt(0);
			_thorns = new NInt(0);
			_currentEnergy = new NInt(0);
			_maxEnergy = new NInt(0);
			_initialEnergy = new NInt(0);
			_energyRecovery = new NInt(0);
			_crit = new NInt(0);
			_superpower = new NInt(0);
		}

		[JsonConstructor]
		public Player(string name, int level, int currentGold, int currentHealth, int maxHealth, int mentality, Attribute rock, 
			int rockCost, Attribute paper, int paperCost, Attribute scissor, int scissorCost, Attribute defense, int defenseCost,
			int thorns, int currentEnergy, int maxEnergy, int initialEnergy, int energyRecovery, int crit, int superpower)
		{
			_name = new NString(name);
			_level = new NInt(level);
			_gold = new NInt(currentGold);
			_currentHealth = new NInt(currentHealth);
			_maxHealth = new NInt(maxHealth);
			_mentality = new NInt(mentality);
			_rock = new NAttribute(rock);
			_rockCost = new NInt(rockCost);
			_paper = new NAttribute(paper);
			_paperCost = new NInt(paperCost);
			_scissor = new NAttribute(scissor);
			_scissorCost = new NInt(scissorCost);
			_defense = new NAttribute( defense);
			_defenseCost = new NInt(defenseCost);
			_thorns = new NInt(thorns);
			_currentEnergy = new NInt(currentEnergy);
			_maxEnergy = new NInt(maxEnergy);
			_initialEnergy = new NInt(initialEnergy);
			_energyRecovery = new NInt(energyRecovery);
			_crit = new NInt(crit);
			_superpower = new NInt(superpower);
		}

		#endregion
	}

	[Serializable]
	public class PlayerOld : Character
	{
		[VerticalGroup("Stats")] [HorizontalGroup("Stats/Gold", MaxWidth = 200f)] [LabelWidth(100f)]
		public int currentGold;

		public Stats trainingStat;

		public PlayerOld(PlayerOld player) { }

		public PlayerOld(string playername)
		{
			name = playername;
			level = 1;
			currentGold = 100;
			rock = 3;
			paper = 3;
			scissor = 3;
		}

		[JsonConstructor]
		public PlayerOld(int currentGold, Stats trainingStat)
		{
			this.currentGold = currentGold;
			this.trainingStat = trainingStat;
		}

        #region ROLLS

		public override int MentalityRollAgainst(Character other)
		{
			var mentalityRoll = VariabilityRoll() + this.mentality - ((Enemy)other).storedMentality;
			Debug.Log("[Player] MentalityRollAgainst() -> mentalityRoll: " + mentalityRoll);
			return mentalityRoll;
		}

        #endregion

        #region STATS

		public void UpdateGold(int gold)
		{
			this.currentGold += this.isSuperAction ? gold * 2 : gold;
		}

        #endregion
	}
}