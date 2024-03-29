using System;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS
{

	[Serializable]
	public class Player
	{
		#region FIELDS

		private NotificableField<string> _name;
		private NotificableField<int> _level;
		private NotificableField<int> _gold;
		private NotificableField<int> _currentHealth;
		private NotificableField<int> _maxHealth;
		private NotificableField<int> _mentality;
		private NotificableField<int> _rock;
		private NotificableField<int> _rockCost;
		private NotificableField<int> _paper;
		private NotificableField<int> _paperCost;
		private NotificableField<int> _scissor;
		private NotificableField<int> _scissorCost;
		private NotificableField<int> _defense;
		private NotificableField<int> _defenseCost;
		private NotificableField<int> _thorns;
		private NotificableField<int> _currentEnergy;
		private NotificableField<int> _maxEnergy;
		private NotificableField<int> _initialEnergy;
		private NotificableField<int> _energyRecovery;
		private NotificableField<int> _crit;
		private NotificableField<int> _superpower;

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

		public int Rock
		{
			get => _rock.Value;
			set => _rock.Value = value;
		}

		public int RockCost
		{
			get => _rockCost.Value;
			set => _rockCost.Value = value;
		}

		public int Paper
		{
			get => _paper.Value;
			set => _paper.Value = value;
		}

		public int PaperCost
		{
			get => _paperCost.Value;
			set => _paperCost.Value = value;
		}

		public int Scissor
		{
			get => _scissor.Value;
			set => _scissor.Value = value;
		}

		public int ScissorCost
		{
			get => _scissorCost.Value;
			set => _scissorCost.Value = value;
		}

		public int Defense
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

		public Player(Player player) { }

		public Player(string playername)
		{
			_name = new NotificableField<string> { Value = playername };
			_level = new NotificableField<int> { Value = 1 };
			_gold = new NotificableField<int> { Value = 100 };
			_rock = new NotificableField<int> { Value = 3 };
			_paper = new NotificableField<int> { Value = 3 };
			_scissor = new NotificableField<int> { Value = 3 };
			_currentHealth = new NotificableField<int> { Value = 10 };
			_maxHealth = new NotificableField<int> { Value = 10 };
			//
			_mentality = new NotificableField<int> { Value = 1 };
			_rockCost = new NotificableField<int> { Value = 5 };
			_paperCost = new NotificableField<int> { Value = 5 };
			_scissorCost = new NotificableField<int> { Value = 5 };
			_defense = new NotificableField<int> { Value = 0 };
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
		public Player(string name, int level, int currentGold, int currentHealth, int maxHealth, int mentality, int rock, int rockCost, int paper,
			int paperCost, int scissor, int scissorCost, int defense, int defenseCost, int thorns, int currentEnergy, int maxEnergy,
			int initialEnergy, int energyRecovery, int crit, int superpower)
		{
			_name = new NotificableField<string> { Value = name };
			_level = new NotificableField<int> { Value = level };
			_gold = new NotificableField<int> { Value = currentGold };
			_currentHealth = new NotificableField<int> { Value = currentHealth };
			_maxHealth = new NotificableField<int> { Value = maxHealth };
			_mentality = new NotificableField<int> { Value = mentality };
			_rock = new NotificableField<int> { Value = rock };
			_rockCost = new NotificableField<int> { Value = rockCost };
			_paper = new NotificableField<int> { Value = paper };
			_paperCost = new NotificableField<int> { Value = paperCost };
			_scissor = new NotificableField<int> { Value = scissor };
			_scissorCost = new NotificableField<int> { Value = scissorCost };
			_defense = new NotificableField<int> { Value = defense };
			_defenseCost = new NotificableField<int> { Value = defenseCost };
			_thorns = new NotificableField<int> { Value = thorns };
			_currentEnergy = new NotificableField<int> { Value = currentEnergy };
			_maxEnergy = new NotificableField<int> { Value = maxEnergy };
			_initialEnergy = new NotificableField<int> { Value = initialEnergy };
			_energyRecovery = new NotificableField<int> { Value = energyRecovery };
			_crit = new NotificableField<int> { Value = crit };
			_superpower = new NotificableField<int> { Value = superpower };
		}
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