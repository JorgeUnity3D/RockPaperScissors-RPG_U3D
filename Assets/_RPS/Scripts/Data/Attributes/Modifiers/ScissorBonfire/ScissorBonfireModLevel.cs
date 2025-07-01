using System;
using Newtonsoft.Json;
using Unity.VisualScripting;

namespace Kapibara.RPS
{
	public class ScissorBonfireModLevel
	{
		private int _healthMod;
		private int _energyMod;
		private int _rockMod;
		private int _paperMod;
		private int _scissorsMod;
		private int _defenseMod;
		private int _thornsMod;
		private int _critMod;
		private int _superPowerMod;

		//indexer operator this[] overload
		public int this[Stats stat]
		{
			get
			{
				switch (stat)
				{
					case Stats.HEALTH:
						return _healthMod;
					case Stats.ENERGY_BASE:
						return _energyMod;
					case Stats.ROCK:
						return _rockMod;
					case Stats.PAPER:
						return _paperMod;
					case Stats.SCISSOR:
						return _scissorsMod;
					case Stats.DEFENSE:
						return _defenseMod;
					case Stats.THORNS:
						return _thornsMod;
					case Stats.CRIT:
						return _critMod;
					case Stats.SUPERPOWER:
						return _superPowerMod;
					default:
						throw new ArgumentException("[ScissorBonfireMod] Invalid stat");
				}
			}
		}

		[JsonConstructor]
		public ScissorBonfireModLevel(int healthMod, int energyMod, int rockMod, int paperMod, int scissorsMod, int defenseMod, int thornsMod,
			int critMod, int superPowerMod)
		{
			_healthMod = healthMod;
			_energyMod = energyMod;
			_rockMod = rockMod;
			_paperMod = paperMod;
			_scissorsMod = scissorsMod;
			_defenseMod = defenseMod;
			_thornsMod = thornsMod;
			_critMod = critMod;
			_superPowerMod = superPowerMod;
		}
	}
}