using Kapibara.RPS;
using System;

namespace Kapibara.Util.NotificableFields
{
	[Serializable]
	public class NAttribute : NotificableField<StatAttribute>
	{
		#region CONSTRUCTOR

		public NAttribute(StatAttribute value)
		{
			Value = value;
		}

		public NAttribute(Stats stat, int value)
		{
			Value = new StatAttribute(stat, value);
			Value.AddModifier(new TrainingHouseModifier(stat));
			Value.AddModifier(new PaperTreeModifier(stat));
		}

		#endregion
	}
}