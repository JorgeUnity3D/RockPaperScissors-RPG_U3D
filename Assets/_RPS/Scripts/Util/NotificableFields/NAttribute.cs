using System;
namespace Kapibara.RPS
{
	[Serializable]
	public class NAttribute : NotificableField<Attribute>
	{
		#region CONSTRUCTOR

		public NAttribute(Attribute value)
		{
			Value = value;
		}

		public NAttribute(Stats stat, int value)
		{
			Value = new Attribute(stat, value);
			Value.AddModifier(new TrainingHouseModifier(stat));
			Value.AddModifier(new PaperTreeModifier(stat));
		}

		#endregion
	}
}