using System;
using System.Collections.Generic;
using Kapibara.Util.NotificableFields;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;

namespace Kapibara.RPS
{
	/// <summary>
	/// Datos persistentes del jugador: estadísticas, oro, nivel y sus modificadores activos.
	/// </summary>
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
		[SerializeField, JsonIgnore] private NInt _currentEnergy;
		[SerializeField] private NAttribute _initialEnergy;
		[SerializeField] private NAttribute _energyRecovery;
		//Crit & SuperPower
		[SerializeField] private NAttribute _crit;
		[SerializeField] private NAttribute _superpower;
		//Backpack
		[SerializeField] private NInt _attackItemLevel;
		[SerializeField] private NInt _healItemLevel;
		[SerializeField] private NInt _energyItemLevel;
		//Theater
		[SerializeField] private List<int> _unlockedStoryIds;

		//Helpers
		[JsonIgnore] private Dictionary<Stats, NAttribute> _statAttributes;

		#endregion

		#region PROPERTIES

		/// <summary>Nombre del personaje jugador.</summary>
		public string Name
		{
			get => _name.Value;
			set => _name.Value = value;
		}

		/// <summary>Nivel actual del jugador; subir de nivel aplica bonificaciones de ScissorBonfire.</summary>
		public int Level
		{
			get => _level.Value;
			set => _level.Value = value;
		}

		/// <summary>Cantidad de oro disponible para compras y desbloqueos.</summary>
		public int Gold
		{
			get => _gold.Value;
			set => _gold.Value = value;
		}

		/// <summary>Salud actual en combate; no se persiste en disco.</summary>
		[JsonIgnore]
		public int CurrentHealth
		{
			get => _currentHealth.Value;
			set => _currentHealth.Value = value;
		}

		/// <summary>Atributo de salud máxima con sus modificadores activos.</summary>
		public StatAttribute MaxHealth
		{
			get => _maxHealth.Value;
			set => _maxHealth.Value = value;
		}

		/// <summary>Atributo de mentalidad; influye en iniciativa y rollos de mente.</summary>
		public StatAttribute Mentality
		{
			get => _mentality.Value;
			set => _mentality.Value = value;
		}

		/// <summary>Atributo de poder de ataque Piedra.</summary>
		public StatAttribute Rock
		{
			get => _rock.Value;
			set => _rock.Value = value;
		}

		/// <summary>Coste de energía al usar el movimiento Piedra.</summary>
		public int RockCost
		{
			get => _rockCost.Value;
			set => _rockCost.Value = value;
		}

		/// <summary>Atributo de poder de ataque Papel.</summary>
		public StatAttribute Paper
		{
			get => _paper.Value;
			set => _paper.Value = value;
		}

		/// <summary>Coste de energía al usar el movimiento Papel.</summary>
		public int PaperCost
		{
			get => _paperCost.Value;
			set => _paperCost.Value = value;
		}

		/// <summary>Atributo de poder de ataque Tijera.</summary>
		public StatAttribute Scissor
		{
			get => _scissor.Value;
			set => _scissor.Value = value;
		}

		/// <summary>Coste de energía al usar el movimiento Tijera.</summary>
		public int ScissorCost
		{
			get => _scissorCost.Value;
			set => _scissorCost.Value = value;
		}

		/// <summary>Atributo de defensa que reduce el daño recibido.</summary>
		public StatAttribute Defense
		{
			get => _defense.Value;
			set => _defense.Value = value;
		}

		/// <summary>Coste de energía al usar el movimiento Defensa.</summary>
		public int DefenseCost
		{
			get => _defenseCost.Value;
			set => _defenseCost.Value = value;
		}

		/// <summary>Atributo de espinas; devuelve daño al atacante.</summary>
		public StatAttribute Thorns
		{
			get => _thorns.Value;
			set => _thorns.Value = value;
		}

		/// <summary>Energía actual; persiste entre steps de un mismo nivel, se resetea a InitialEnergy al inicio de cada viaje.</summary>
		[JsonIgnore]
		public int CurrentEnergy
		{
			get => _currentEnergy.Value;
			set => _currentEnergy.Value = value;
		}

		/// <summary>Energía con la que arranca el jugador al entrar a un nivel. Cap en COMBAT_MAX_ENERGY.</summary>
		public StatAttribute InitialEnergy
		{
			get => _initialEnergy.Value;
			set => _initialEnergy.Value = value;
		}

		/// <summary>Atributo de recuperación de energía por turno.</summary>
		public StatAttribute EnergyRecovery
		{
			get => _energyRecovery.Value;
			set => _energyRecovery.Value = value;
		}

		/// <summary>Atributo de probabilidad de golpe crítico.</summary>
		public StatAttribute Crit
		{
			get => _crit.Value;
			set => _crit.Value = value;
		}

		/// <summary>Atributo de superpoder; activa habilidades especiales al acumularse.</summary>
		public StatAttribute Superpower
		{
			get => _superpower.Value;
			set => _superpower.Value = value;
		}

		/// <summary>Nivel del consumible de daño del backpack. 0 = no mejorado.</summary>
		public int AttackItemLevel
		{
			get => _attackItemLevel.Value;
			set => _attackItemLevel.Value = value;
		}

		/// <summary>Nivel del consumible de curación del backpack. 0 = no mejorado.</summary>
		public int HealItemLevel
		{
			get => _healItemLevel.Value;
			set => _healItemLevel.Value = value;
		}

		/// <summary>Nivel del consumible de energía del backpack. 0 = no mejorado.</summary>
		public int EnergyItemLevel
		{
			get => _energyItemLevel.Value;
			set => _energyItemLevel.Value = value;
		}

		/// <summary>
		/// IDs de las historias desbloqueadas en el Theater. La historia 0 está disponible desde el inicio.
		/// No asumas orden secuencial — el jugador puede desbloquear rutas no contiguas.
		/// Usa UnlockStory() para añadir entradas; IsStoryUnlocked() para consultar.
		/// </summary>
		public List<int> UnlockedStoryIds => _unlockedStoryIds;

		/// <summary>Devuelve true si la historia con el índice dado está desbloqueada.</summary>
		public bool IsStoryUnlocked(int storyId)
		{
			return _unlockedStoryIds.Contains(storyId);
		}

		/// <summary>Desbloquea una historia por ID y dispara guardado. Ignorado si ya estaba desbloqueada.</summary>
		public void UnlockStory(int storyId)
		{
			if (_unlockedStoryIds.Contains(storyId)) return;
			_unlockedStoryIds.Add(storyId);
		}

		/// <summary>Lista plana de todos los StatAttribute del jugador para iteración genérica. Crea una nueva lista en cada acceso — no llamar en hot paths.</summary>
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
			_defense = new NAttribute(Stats.DEFENSE, 3);
			_defense.Value.AddModifier(new ScissorBonfireModifier(Stats.DEFENSE));
			_defenseCost = new NInt(0);
			//Thorns
			_thorns = new NAttribute(Stats.THORNS, 0);
			_thorns.Value.AddModifier(new ScissorBonfireModifier(Stats.THORNS));
			//Energy & Recovery
			_initialEnergy = new NAttribute(Stats.INITIAL_ENERGY, 25);
			_currentEnergy = new NInt(25);
			_energyRecovery = new NAttribute(Stats.ENERGY_RECOVERY, 5);
			//Crit & SuperPower
			_crit = new NAttribute(Stats.CRIT, 0);
			_crit.Value.AddModifier(new ScissorBonfireModifier(Stats.CRIT));
			_superpower = new NAttribute(Stats.SUPERPOWER, 0);
			_superpower.Value.AddModifier(new ScissorBonfireModifier(Stats.SUPERPOWER));
			//Backpack
			_attackItemLevel = new NInt(0);
			_healItemLevel   = new NInt(0);
			_energyItemLevel = new NInt(0);
			//Theater
			_unlockedStoryIds = new List<int> { 0 };

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
				{ Stats.INITIAL_ENERGY, _initialEnergy },
				{ Stats.ENERGY_RECOVERY, _energyRecovery },
				{ Stats.CRIT, _crit },
				{ Stats.SUPERPOWER, _superpower }
			};
		}

		[JsonConstructor]
		public Player(string name, int level, int gold, int currentHealth, StatAttribute maxHealth, StatAttribute mentality, StatAttribute rock,
			int rockCost, StatAttribute paper, int paperCost, StatAttribute scissor, int scissorCost, StatAttribute defense, int defenseCost, StatAttribute thorns,
			StatAttribute initialEnergy, StatAttribute energyRecovery, StatAttribute crit, StatAttribute superpower,
			int attackItemLevel = 0, int healItemLevel = 0, int energyItemLevel = 0, List<int> unlockedStoryIds = null)
		{
			_name = new NString(name);
			_level = new NInt(level);
			_gold = new NInt(gold);
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
			_initialEnergy = new NAttribute(initialEnergy);
			_currentEnergy = new NInt(initialEnergy.TotalValue);
			_energyRecovery = new NAttribute(energyRecovery);
			//Crit & SuperPower
			_crit = new NAttribute(crit);
			_superpower = new NAttribute(superpower);
			//Backpack
			_attackItemLevel = new NInt(attackItemLevel);
			_healItemLevel   = new NInt(healItemLevel);
			_energyItemLevel = new NInt(energyItemLevel);
			//Theater
			_unlockedStoryIds = unlockedStoryIds ?? new List<int> { 0 };

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
				{ Stats.INITIAL_ENERGY, _initialEnergy },
				{ Stats.ENERGY_RECOVERY, _energyRecovery },
				{ Stats.CRIT, _crit },
				{ Stats.SUPERPOWER, _superpower }
			};
		}

		#endregion

		#region EVENTS

		/// <summary>Se dispara cuando cambia el valor de oro del jugador.</summary>
		public event UnityAction<int> OnGoldValueChanged
		{
			add { _gold.OnValueChanged += value; }
			remove { _gold.OnValueChanged -= value; }
		}

		/// <summary>Se dispara cuando cambia el nivel del jugador.</summary>
		public event UnityAction<int> OnLevelValueChanged
		{
			add { _level.OnValueChanged += value; }
			remove { _level.OnValueChanged -= value; }
		}

		#endregion
	}

}