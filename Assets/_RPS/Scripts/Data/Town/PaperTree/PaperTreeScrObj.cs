using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS
{

	[CreateAssetMenu(fileName = "PaperTreeSkillTrees", menuName = "RPSRPG/PaperTreeSkillTree")]
	public class PaperTreeScrObj : SerializedScriptableObject
	{
		[SerializeField] private List<PaperTreeNode> _rockSkillTree;
		[SerializeField] private List<PaperTreeNode> _paperSkillTree;
		[SerializeField] private List<PaperTreeNode> _scissorsSkillTree;
		[SerializeField] private List<PaperTreeNode> _defenseSkillTree;
		[SerializeField] private List<PaperTreeNode> _energyRecoverySkillTree;
		
		//indexer operator this[] overload
		public List<PaperTreeNode> this[Stats stat]
		{
			get
			{
				switch (stat)
				{
					case Stats.ROCK:
						return _rockSkillTree;
					case Stats.PAPER:
						return _paperSkillTree;
					case Stats.SCISSOR:
						return _scissorsSkillTree;
					case Stats.DEFENSE:
						return _defenseSkillTree;
					case Stats.ENERGY_RECOVERY:
						return _energyRecoverySkillTree;
					default:
						throw new ArgumentException("[PaperTreeScrObj] Invalid stat");
				}
			}
		}
	}
}