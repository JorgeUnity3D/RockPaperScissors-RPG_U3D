using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Kapibara.RPS
{
	[Serializable]
	public class PaperTreeModifier : BaseModifier
	{
		private NBool _skillTreeData;

		public bool SkillTreeData
		{
			get => _skillTreeData.Value;
			set => _skillTreeData.Value = value;
		}
		
		[JsonIgnore]
		public override int TotaModifier
		{
			get => Modifier;
		}
		
		#region CONSTRUCTOR

		public PaperTreeModifier(Stats stat, int initialModifier = 0)
		{
			ModifierType = GameConsts.ATTRIBUTE_TYPE_VALUE[this.GetType()];;
			_stat = stat;
			_modifier = new NInt(initialModifier);
			_level = new NInt(1);
			_experience = new NInt(0);
			_skillTreeData = new NBool(false);
		}

		[JsonConstructor]
		public PaperTreeModifier(Stats stat, int modifier, int level, int experience, bool skillTreeData)
		{
			ModifierType = GameConsts.ATTRIBUTE_TYPE_VALUE[this.GetType()];;
			_stat = stat;
			_modifier = new NInt(modifier);
			_level = new NInt(level);
			_experience = new NInt(experience);
			_skillTreeData = new NBool(skillTreeData);
		}

		#endregion
	}
}