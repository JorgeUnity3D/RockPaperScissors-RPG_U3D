using System;
using System.Collections.Generic;
using Kapibara.Util.NotificableFields;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

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
		[SerializeField, JsonIgnore] private NInt _currentHealth;
		[SerializeField] private NAttribute _maxHealth;
		//Mentality
		[SerializeField] private NAttribute _mentality;
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
		[SerializeField] private NAttribute _thorns;
		//Energy
		[SerializeField] private NInt _currentEnergy;
		[SerializeField] private NAttribute _baseEnergy;
		[SerializeField] private NInt _initialEnergy;
		[SerializeField] private NAttribute _energyRecovery;
		//Crit & SuperPower
		[SerializeField] private NAttribute _crit;
		[SerializeField] private NAttribute _superpower;

		//Helpers
		[JsonIgnore] private Dictionary<Stats, NAttribute> _statAttributes;

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

		[JsonIgnore]
		public int CurrentHealth
		{
			get => _currentHealth.Value;
			set => _currentHealth.Value = value;
		}

		public StatAttribute MaxHealth
		{
			get => _maxHealth.Value;
			set => _maxHealth.Value = value;
		}

		public StatAttribute Mentality
		{
			get => _mentality.Value;
			set => _mentality.Value = value;
		}

		public StatAttribute Rock
		{
			get => _rock.Value;
			set => _rock.Value = value;
		}

		public int RockCost
		{
			get => _rockCost.Value;
			set => _rockCost.Value = value;
		}

		public StatAttribute Paper
		{
			get => _paper.Value;
			set => _paper.Value = value;
		}

		public int PaperCost
		{
			get => _paperCost.Value;
			set => _paperCost.Value = value;
		}

		public StatAttribute Scissor
		{
			get => _scissor.Value;
			set => _scissor.Value = value;
		}

		public int ScissorCost
		{
			get => _scissorCost.Value;
			set => _scissorCost.Value = value;
		}

		public StatAttribute Defense
		{
			get => _defense.Value;
			set => _defense.Value = value;
		}

		public int DefenseCost
		{
			get => _defenseCost.Value;
			set => _defenseCost.Value = value;
		}

		public StatAttribute Thorns
		{
			get => _thorns.Value;
			set => _thorns.Value = value;
		}

		[JsonIgnore]
		public int CurrentEnergy
		{
			get => _currentEnergy.Value;
			set => _currentEnergy.Value = value;
		}

		public StatAttribute BaseEnergy
		{
			get => _baseEnergy.Value;
			set => _baseEnergy.Value = value;
		}

		public int InitialEnergy
		{
			get => _initialEnergy.Value;
			set => _initialEnergy.Value = value;
		}

		public StatAttribute EnergyRecovery
		{
			get => _energyRecovery.Value;
			set => _energyRecovery.Value = value;
		}

		public StatAttribute Crit
		{
			get => _crit.Value;
			set => _crit.Value = value;
		}

		public StatAttribute Superpower
		{
			get => _superpower.Value;
			set => _superpower.Value = value;
		}

		[JsonIgnore]
		public List<StatAttribute> Attributes
		{
			get
			{
				return new List<StatAttribute>
				{
					_maxHealth.Value,
					_mentality.Value,
					_rock.Value,
					_paper.Value,
					_scissor.Value,
					_defense.Value,
					_thorns.Value,
					_baseEnergy.Value,
					_energyRecovery.Value,
					_crit.Value,
					_superpower.Value
				};
			}
		}

		//indexer operator this[] overload
		/*
			dictList = lista.CreateDictionary(x => x.name, x => x);
			dicList<string, object>
			lista<object>
		*/
		public StatAttribute this[Stats stat]
		{
			get
			{
				return _statAttributes[stat].Value;
			}
		}

		#endregion

		#region CONSTRUCTORS

		public Player(Player player) { }

		public Player(string playername)
		{
			_name = new NString(playername);
			_level = new NInt(1);
			_gold = new NInt(1000);
			//Health
			_currentHealth = new NInt(10);
			_maxHealth = new NAttribute(Stats.HEALTH, 10);
			_maxHealth.Value.AddModifier(new ScissorBonfireModifier(Stats.HEALTH));
			//Mentality
			_mentality = new NAttribute(Stats.MENTALITY, 1);
			//Rock
			_rock = new NAttribute(Stats.ROCK, 3);
			_rock.Value.AddModifier(new ScissorBonfireModifier(Stats.ROCK));
			_rockCost = new NInt(5);
			//Paper
			_paper = new NAttribute(Stats.PAPER, 4);
			_paper.Value.AddModifier(new ScissorBonfireModifier(Stats.PAPER));
			_paperCost = new NInt(5);
			//Scissor
			_scissor = new NAttribute(Stats.SCISSOR, 5);
			_scissor.Value.AddModifier(new ScissorBonfireModifier(Stats.SCISSOR));
			_scissorCost = new NInt(5);
			//Defense
			_defense = new NAttribute(Stats.DEFENSE, 0);
			_defense.Value.AddModifier(new ScissorBonfireModifier(Stats.DEFENSE));
			_defenseCost = new NInt(0);
			//Thorns
			_thorns = new NAttribute(Stats.THORNS, 0);
			_thorns.Value.AddModifier(new ScissorBonfireModifier(Stats.THORNS));
			//Energy & Recovery
			_currentEnergy = new NInt(0);
			_baseEnergy = new NAttribute(Stats.ENERGY_BASE, 0);
			_baseEnergy.Value.AddModifier(new ScissorBonfireModifier(Stats.ENERGY_BASE));
			_initialEnergy = new NInt(0);
			_energyRecovery = new NAttribute(Stats.ENERGY_RECOVERY, 0);
			//Crit & SuperPower
			_crit = new NAttribute(Stats.CRIT, 0);
			_crit.Value.AddModifier(new ScissorBonfireModifier(Stats.CRIT));
			_superpower = new NAttribute(Stats.SUPERPOWER, 0);
			_superpower.Value.AddModifier(new ScissorBonfireModifier(Stats.SUPERPOWER));

			//Helpers
			_statAttributes = new Dictionary<Stats, NAttribute>
			{
				{ Stats.HEALTH, _maxHealth },
				{ Stats.MENTALITY, _mentality },
				{ Stats.ROCK, _rock },
				{ Stats.PAPER, _paper },
				{ Stats.SCISSOR, _scissor },
				{ Stats.DEFENSE, _defense },
				{ Stats.THORNS, _thorns },
				{ Stats.ENERGY_BASE, _baseEnergy },
				{ Stats.ENERGY_RECOVERY, _energyRecovery },
				{ Stats.CRIT, _crit },
				{ Stats.SUPERPOWER, _superpower }
			};
		}

		[JsonConstructor]
		public Player(string name, int level, int currentGold, int currentHealth, StatAttribute maxHealth, StatAttribute mentality, StatAttribute rock,
			int rockCost, StatAttribute paper, int paperCost, StatAttribute scissor, int scissorCost, StatAttribute defense, int defenseCost, StatAttribute thorns,
			int currentEnergy, StatAttribute baseEnergy, int initialEnergy, StatAttribute energyRecovery, StatAttribute crit, StatAttribute superpower)
		{
			_name = new NString(name);
			_level = new NInt(level);
			_gold = new NInt(currentGold);
			//Health
			_currentHealth = new NInt(currentHealth);
			_maxHealth = new NAttribute(maxHealth);
			//Mentality
			_mentality = new NAttribute(mentality);
			//Rock
			_rock = new NAttribute(rock);
			_rockCost = new NInt(rockCost);
			//Paper
			_paper = new NAttribute(paper);
			_paperCost = new NInt(paperCost);
			//Scissor
			_scissor = new NAttribute(scissor);
			_scissorCost = new NInt(scissorCost);
			//Defense
			_defense = new NAttribute(defense);
			_defenseCost = new NInt(defenseCost);
			//Thorns
			_thorns = new NAttribute(thorns);
			//Energy & Recovery
			_currentEnergy = new NInt(currentEnergy);
			_baseEnergy = new NAttribute(baseEnergy);
			_initialEnergy = new NInt(initialEnergy);
			_energyRecovery = new NAttribute(energyRecovery);
			//Crit & SuperPower
			_crit = new NAttribute(crit);
			_superpower = new NAttribute(superpower);

			//Helpers
			_statAttributes = new Dictionary<Stats, NAttribute>
			{
				{ Stats.HEALTH, _maxHealth },
				{ Stats.MENTALITY, _mentality },
				{ Stats.ROCK, _rock },
				{ Stats.PAPER, _paper },
				{ Stats.SCISSOR, _scissor },
				{ Stats.DEFENSE, _defense },
				{ Stats.THORNS, _thorns },
				{ Stats.ENERGY_BASE, _baseEnergy },
				{ Stats.ENERGY_RECOVERY, _energyRecovery },
				{ Stats.CRIT, _crit },
				{ Stats.SUPERPOWER, _superpower }
			};
		}

		#endregion

		#region EVENTS

		public event UnityAction<int> OnGoldValueChanged
		{
			add { _gold.OnValueChanged += value; }
			remove { _gold.OnValueChanged -= value; }
		}

		public event UnityAction<int> OnLevelValueChanged
		{
			add { _level.OnValueChanged += value; }
			remove { _level.OnValueChanged -= value; }

		#endregion
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