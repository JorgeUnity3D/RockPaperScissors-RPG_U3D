using System;
using Kapibara.Util.NotificableFields;
using Newtonsoft.Json;

namespace Kapibara.RPS
{
	[Serializable]
	public class ScissorBonfireModifier : BaseModifier
	{
		[JsonIgnore]
		public override int TotaModifier
		{
			get => Modifier;
		}
		
		#region CONSTRUCTOR

		public ScissorBonfireModifier(Stats stat, int initialModifier = 0)
		{
			ModifierType = GameConsts.ATTRIBUTE_TYPE_VALUE[this.GetType()];
			_stat = stat;
			_modifier = new NInt(initialModifier);
			_level = new NInt(1);
			_experience = new NInt(0);
		}

		[JsonConstructor]
		public ScissorBonfireModifier(Stats stat, int modifier, int level, int experience)
		{
			ModifierType = GameConsts.ATTRIBUTE_TYPE_VALUE[this.GetType()];
			_stat = stat;
			_modifier = new NInt(modifier);
			_level = new NInt(level);
			_experience = new NInt(experience);
		}

		#endregion

	}
}