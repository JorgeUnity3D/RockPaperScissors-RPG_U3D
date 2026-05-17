using System;
using System.Collections.Generic;
using UnityEngine;

namespace Kapibara.RPS
{
	public class Enemy
	{
		#region DATA

		public string             Name           { get; private set; }
		public Sprite             Portrait       { get; private set; }
		public int                MaxHealth      { get; private set; }
		public int                Mentality      { get; private set; }
		public int                Rock           { get; private set; }
		public int                RockCost       { get; private set; }
		public int                Paper          { get; private set; }
		public int                PaperCost      { get; private set; }
		public int                Scissor        { get; private set; }
		public int                ScissorCost    { get; private set; }
		public int                Defense        { get; private set; }
		public int                DefenseCost    { get; private set; }
		public int                Thorns         { get; private set; }
		public int                Crit           { get; private set; }
		public int                Superpower     { get; private set; }
		public int                InitialEnergy  { get; private set; }
		public int                EnergyRecovery { get; private set; }
		public int                MaxEnergy      { get; private set; }
		public int                Level          { get; private set; }
		public int                RockProb       { get; private set; }
		public int                PaperProb      { get; private set; }
		public int                ScissorProb    { get; private set; }
		public int                DefenseProb    { get; private set; }
		public int                EnergyProb     { get; private set; }
		public int                MentalityMod   { get; private set; }
		public int                GoldMin        { get; private set; }
		public int                GoldMax        { get; private set; }
		public List<LanguageScrObj> Languages    { get; private set; }

		#endregion

		#region RUNTIME STATE

		public int      CurrentHealth   { get; set; }
		public int      CurrentEnergy   { get; set; }
		public Actions  CurrentAction   { get; set; }
		public Actions  ThinkingAction  { get; set; }
		public bool     IsSuperAction   { get; set; }
		public int      StoredMentality { get; private set; }
		public Language CurrentLanguage { get; private set; }

		#endregion

		#region CONSTRUCTOR

		public Enemy(EnemyData data)
		{
			Name           = data.Name;
			Portrait       = data.Portrait;
			MaxHealth      = data.MaxHealth;
			Mentality      = data.Mentality;
			Rock           = data.Rock;
			RockCost       = data.RockCost;
			Paper          = data.Paper;
			PaperCost      = data.PaperCost;
			Scissor        = data.Scissor;
			ScissorCost    = data.ScissorCost;
			Defense        = data.Defense;
			DefenseCost    = data.DefenseCost;
			Thorns         = data.Thorns;
			Crit           = data.Crit;
			Superpower     = data.Superpower;
			InitialEnergy  = data.InitialEnergy;
			EnergyRecovery = data.EnergyRecovery;
			RockProb       = data.RockProb;
			PaperProb      = data.PaperProb;
			ScissorProb    = data.ScissorProb;
			DefenseProb    = data.DefenseProb;
			EnergyProb     = data.EnergyProb;
			MentalityMod   = data.MentalityMod;
			GoldMin        = data.GoldMin;
			GoldMax        = data.GoldMax;
			Languages      = data.Languages;
			Level          = 1;
			MaxEnergy      = GameConsts.COMBAT_MAX_ENERGY;

			CurrentHealth   = MaxHealth;
			CurrentEnergy   = MaxEnergy;
			StoredMentality = Mentality;
			CurrentAction   = Actions.NONE;
			ThinkingAction  = Actions.NONE;
		}

		#endregion

		#region COMBAT

		public void ActionRoll()
		{
			List<int> ponderatedActions = new List<int>();
			Actions[] attackTypes = (Actions[])Enum.GetValues(typeof(Actions));

			foreach (Actions attackType in attackTypes)
			{
				int prob;
				switch (attackType)
				{
					case Actions.ROCK:    prob = RockProb;    break;
					case Actions.PAPER:   prob = PaperProb;   break;
					case Actions.SCISSOR: prob = ScissorProb; break;
					case Actions.DEFENSE: prob = DefenseProb; break;
					case Actions.ENERGY:  prob = EnergyProb;  break;
					default:              continue;
				}

				int cost = ActionCost(attackType);
				if (CurrentEnergy >= cost)
				{
					for (int i = 0; i < prob; i++)
						ponderatedActions.Add((int)attackType);
				}
			}

			if (ponderatedActions.Count == 0)
			{
				CurrentAction  = Actions.ENERGY;
				ThinkingAction = Actions.ENERGY;
				return;
			}

			Actions chosen = (Actions)ponderatedActions[UnityEngine.Random.Range(0, ponderatedActions.Count)];
			CurrentAction  = chosen;
			ThinkingAction = chosen;
		}

		public int MentalityRollAgainst(int playerMentality)
		{
			return CombatResolver.VariabilityRoll(Level) + StoredMentality - playerMentality;
		}

		public void IncreaseMentality()  => StoredMentality += MentalityMod;
		public void ResetMentality()     => StoredMentality  = Mentality;

		public void LanguageRoll()
		{
			if (Languages == null || Languages.Count == 0) return;
			CurrentLanguage = Languages[RNGGenerator.RandomBetween(0, Languages.Count - 1)].Data;
		}

		public int RewardRoll() => RNGGenerator.RandomBetween(GoldMin, GoldMax);

		#endregion

		#region ENERGY / HP

		public bool HasEnoughEnergy(Actions action) => CurrentEnergy >= ActionCost(action);

		public int ActionCost(Actions action)
		{
			switch (action)
			{
				case Actions.ROCK:    return RockCost;
				case Actions.PAPER:   return PaperCost;
				case Actions.SCISSOR: return ScissorCost;
				case Actions.DEFENSE: return DefenseCost;
				case Actions.ENERGY:  return -EnergyRecovery;
				default:              return 0;
			}
		}

		public void ReceiveDamage(int damage)
		{
			if (damage <= 0) return;
			CurrentHealth = Mathf.Max(0, CurrentHealth - damage);
		}

		public void RecoverEnergy()      => CurrentEnergy = Mathf.Min(MaxEnergy, CurrentEnergy + EnergyRecovery);
		public void PayActionEnergyCost() => CurrentEnergy -= ActionCost(CurrentAction);

		public void ResetCombatState()
		{
			CurrentAction  = Actions.NONE;
			ThinkingAction = Actions.NONE;
			IsSuperAction  = false;
		}

		#endregion
	}
}
