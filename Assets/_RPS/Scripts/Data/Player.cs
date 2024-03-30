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
		[SerializeField] private NotificableField<string> _name;
		[SerializeField] private NotificableField<int> _level;
		[SerializeField] private NotificableField<int> _gold;
		//Health
		[SerializeField] private NotificableField<int> _currentHealth;
		[SerializeField] private NotificableField<int> _maxHealth;
		//Mentality
		[SerializeField] private NotificableField<int> _mentality;
		//Rock
		[SerializeField] private NotificableField<Attribute> _rock;
		[SerializeField] private NotificableField<int> _rockCost;
		//Paper
		[SerializeField] private NotificableField<Attribute> _paper;
		[SerializeField] private NotificableField<int> _paperCost;
		//Scissors
		[SerializeField] private NotificableField<Attribute> _scissor;
		[SerializeField] private NotificableField<int> _scissorCost;
		//Defense
		[SerializeField] private NotificableField<Attribute> _defense;
		[SerializeField] private NotificableField<int> _defenseCost;
		[SerializeField] private NotificableField<int> _thorns;
		//Energy
		[SerializeField] private NotificableField<int> _currentEnergy;
		[SerializeField] private NotificableField<int> _maxEnergy;
		[SerializeField] private NotificableField<int> _initialEnergy;
		[SerializeField] private NotificableField<int> _energyRecovery;
		//Crit & SuperPower
		[SerializeField] private NotificableField<int> _crit;
		[SerializeField] private NotificableField<int> _superpower;

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

		#endregion

		#region CONSTRUCTORS

		public Player(Player player) { }

		public Player(string playername)
		{
			_name = new NotificableField<string> { Value = playername };
			_level = new NotificableField<int> { Value = 1 };
			_gold = new NotificableField<int> { Value = 100 };
			_rock = new NotificableField<Attribute> { Value = new Attribute(3) };
			_rock.Value.AddModifier(new TrainingModifier(1));
			_rock.Value.AddModifier(new SkillTreeModifier(2));
			_paper = new NotificableField<Attribute> { Value = new Attribute(4) };
			_paper.Value.AddModifier(new TrainingModifier(3));
			_paper.Value.AddModifier(new SkillTreeModifier(4));
			_scissor = new NotificableField<Attribute> { Value = new Attribute(5) };
			_scissor.Value.AddModifier(new TrainingModifier(5));
			_scissor.Value.AddModifier(new SkillTreeModifier(6));
			_currentHealth = new NotificableField<int> { Value = 10 };
			_maxHealth = new NotificableField<int> { Value = 10 };
			//
			_mentality = new NotificableField<int> { Value = 1 };
			_rockCost = new NotificableField<int> { Value = 5 };
			_paperCost = new NotificableField<int> { Value = 5 };
			_scissorCost = new NotificableField<int> { Value = 5 };
			_defense = new NotificableField<Attribute> { Value = new Attribute(0) };
			_defenseCost = new NotificableField<int> { Value = 0 };
			_thorns = new NotificableField<int> { Value = 0 };
			_currentEnergy = new NotificableField<int> { Value = 0 };
			_maxEnergy = new NotificableField<int> { Value = 0 };
			_initialEnergy = new NotificableField<int> { Value = 0 };
			_energyRecovery = new NotificableField<int> { Value = 0 };
			_crit = new NotificableField<int> { Value = 0 };
			_superpower = new NotificableField<int> { Value = 0 };
		}

		[JsonConstructor]
		public Player(string name, int level, int currentGold, int currentHealth, int maxHealth, int mentality, Attribute rock, 
			int rockCost, Attribute paper, int paperCost, Attribute scissor, int scissorCost, Attribute defense, int defenseCost,
			int thorns, int currentEnergy, int maxEnergy, int initialEnergy, int energyRecovery, int crit, int superpower)
		{
			_name = new NotificableField<string> { Value = name };
			_level = new NotificableField<int> { Value = level };
			_gold = new NotificableField<int> { Value = currentGold };
			_currentHealth = new NotificableField<int> { Value = currentHealth };
			_maxHealth = new NotificableField<int> { Value = maxHealth };
			_mentality = new NotificableField<int> { Value = mentality };
			_rock = new NotificableField<Attribute> { Value = rock };
			_rockCost = new NotificableField<int> { Value = rockCost };
			_paper = new NotificableField<Attribute> { Value = paper };
			_paperCost = new NotificableField<int> { Value = paperCost };
			_scissor = new NotificableField<Attribute> { Value = scissor };
			_scissorCost = new NotificableField<int> { Value = scissorCost };
			_defense = new NotificableField<Attribute> { Value =  defense };
			_defenseCost = new NotificableField<int> { Value = defenseCost };
			_thorns = new NotificableField<int> { Value = thorns };
			_currentEnergy = new NotificableField<int> { Value = currentEnergy };
			_maxEnergy = new NotificableField<int> { Value = maxEnergy };
			_initialEnergy = new NotificableField<int> { Value = initialEnergy };
			_energyRecovery = new NotificableField<int> { Value = energyRecovery };
			_crit = new NotificableField<int> { Value = crit };
			_superpower = new NotificableField<int> { Value = superpower };
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