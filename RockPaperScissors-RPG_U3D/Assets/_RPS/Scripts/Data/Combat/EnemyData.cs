using System;
using System.Collections.Generic;
using UnityEngine;

namespace Kapibara.RPS
{
	/// <summary>
	/// Configuración estática de un enemigo. Solo datos de diseño — sin estado de runtime.
	/// Phase 4 (CombatContext) leerá estos valores para inicializar el combate.
	/// </summary>
	[Serializable]
	public class EnemyData
	{
		[Header("Identity")]
		[SerializeField] private EnemyId _id;
		[SerializeField] private string  _name;
		[SerializeField] private Sprite  _portrait;

		[Header("Combat Stats")]
		[SerializeField] private int _maxHealth;
		[SerializeField] private int _mentality;
		[SerializeField] private int _rock;
		[SerializeField] private int _paper;
		[SerializeField] private int _scissor;
		[SerializeField] private int _defense;
		[SerializeField] private int _thorns;
		[SerializeField] private int _crit;
		[SerializeField] private int _superpower;

		[Header("Energy")]
		[SerializeField] private int _initialEnergy;
		[SerializeField] private int _energyRecovery;

		[Header("Action Costs")]
		[SerializeField] private int _rockCost;
		[SerializeField] private int _paperCost;
		[SerializeField] private int _scissorCost;
		[SerializeField] private int _defenseCost;

		[Header("Action Probabilities (must sum 100)")]
		[SerializeField] private int _rockProb;
		[SerializeField] private int _paperProb;
		[SerializeField] private int _scissorProb;
		[SerializeField] private int _defenseProb;
		[SerializeField] private int _energyProb;

		[Header("Mentality")]
		[SerializeField] private int _mentalityMod;

		[Header("Reward")]
		[SerializeField] private int _goldMin;
		[SerializeField] private int _goldMax;

		[Header("Languages")]
		[SerializeField] private List<LanguageScrObj> _languages;

		public EnemyId Id               => _id;
		public string Name              => _name;
		public Sprite Portrait          => _portrait;
		public int MaxHealth            => _maxHealth;
		public int Mentality            => _mentality;
		public int Rock                 => _rock;
		public int Paper                => _paper;
		public int Scissor              => _scissor;
		public int Defense              => _defense;
		public int Thorns               => _thorns;
		public int Crit                 => _crit;
		public int Superpower           => _superpower;
		public int InitialEnergy        => _initialEnergy;
		public int EnergyRecovery       => _energyRecovery;
		public int RockCost             => _rockCost;
		public int PaperCost            => _paperCost;
		public int ScissorCost          => _scissorCost;
		public int DefenseCost          => _defenseCost;
		public int RockProb             => _rockProb;
		public int PaperProb            => _paperProb;
		public int ScissorProb          => _scissorProb;
		public int DefenseProb          => _defenseProb;
		public int EnergyProb           => _energyProb;
		public int MentalityMod         => _mentalityMod;
		public int GoldMin              => _goldMin;
		public int GoldMax              => _goldMax;
		public List<LanguageScrObj> Languages => _languages;
	}
}
