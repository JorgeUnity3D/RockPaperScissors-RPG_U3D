using System;
using System.Collections.Generic;
using Kapibara.Util.NotificableFields;
using Newtonsoft.Json;
using UnityEngine;

namespace Kapibara.RPS
{
	/// <summary>
	/// Modificador del árbol de habilidades Paper Tree. Registra los nodos desbloqueados y suma su bonificación.
	/// </summary>
	[Serializable]
	public class PaperTreeModifier : BaseModifier
	{
		private List<SkillNode> _unlockedNodes;

		/// <summary>Lista de IDs de nodos del árbol que ya han sido desbloqueados para esta estadística.</summary>
		public List<SkillNode> UnlockedNodes => _unlockedNodes;

		[JsonIgnore]
		public override int TotalModifier
		{
			get => Modifier;
		}

		#region CONSTRUCTOR

		public PaperTreeModifier(Stats stat, int initialModifier = 0)
		{
			ModifierType = GameConsts.ATTRIBUTE_TYPE_VALUE[this.GetType()];
			_stat = stat;
			_modifier = new NInt(initialModifier);
			_level = new NInt(1);
			_experience = new NInt(0);
			_unlockedNodes = new List<SkillNode>();
		}

		[JsonConstructor]
		public PaperTreeModifier(Stats stat, int modifier, int level, int experience, List<SkillNode> unlockedNodes)
		{
			ModifierType = GameConsts.ATTRIBUTE_TYPE_VALUE[this.GetType()];
			_stat = stat;
			_modifier = new NInt(modifier);
			_level = new NInt(level);
			_experience = new NInt(experience);
			_unlockedNodes = unlockedNodes ?? new List<SkillNode>();
		}

		#endregion
	}
}