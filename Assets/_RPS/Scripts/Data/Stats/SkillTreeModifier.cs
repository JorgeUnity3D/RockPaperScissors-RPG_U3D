using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Kapibara.RPS
{
	[Serializable]
	public class SkillTreeModifier : BaseModifier
	{
		private NotificableField<bool> _skillTreeData;

		public bool SkillTreeData
		{
			get => _skillTreeData.Value;
			set => _skillTreeData.Value = value;
		}
		
		#region CONSTRUCTOR

		public SkillTreeModifier(int initialModifier)
		{
			ObjType = 1;
			_modifier = new NotificableField<int>() { Value = initialModifier };
			_level = new NotificableField<int>() { Value = 1 };
			_experience = new NotificableField<int>() { Value = 0 };
			_skillTreeData = new NotificableField<bool>() { Value = false };
		}

		[JsonConstructor]
		public SkillTreeModifier(int modifier, int level, int experience, bool skillTreeData)
		{
			ObjType = 1;
			_modifier = new NotificableField<int>() { Value = modifier };
			_level = new NotificableField<int>() { Value = level };
			_experience = new NotificableField<int>() { Value = experience };
			_skillTreeData = new NotificableField<bool>() { Value = skillTreeData };
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