using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Kapibara.RPS
{
	[Serializable]
	public class SkillTreeModifier : BaseModifier
	{
		private NBool _skillTreeData;

		public bool SkillTreeData
		{
			get => _skillTreeData.Value;
			set => _skillTreeData.Value = value;
		}
		
		#region CONSTRUCTOR

		public SkillTreeModifier(int initialModifier)
		{
			ModifierType = GameConsts.ATTRIBUTE_TYPE_VALUE[this.GetType()];;
			_modifier = new NInt(initialModifier);
			_level = new NInt(1);
			_experience = new NInt(0);
			_skillTreeData = new NBool(false);
		}

		[JsonConstructor]
		public SkillTreeModifier(int modifier, int level, int experience, bool skillTreeData)
		{
			ModifierType = GameConsts.ATTRIBUTE_TYPE_VALUE[this.GetType()];;
			_modifier = new NInt(modifier);
			_level = new NInt(level);
			_experience = new NInt(experience);
			_skillTreeData = new NBool(skillTreeData);
		}

		#endregion

		#region CONTROL

		public override int Apply(int baseValue)
		{
			return baseValue + (baseValue * _modifier.Value);
		}

		#endregion
	}
}