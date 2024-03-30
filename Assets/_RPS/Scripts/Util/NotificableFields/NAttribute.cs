using System;
namespace Kapibara.RPS
{
	[Serializable]
	public class NAttribute : NotificableField<Attribute>
	{
		public NAttribute(Attribute value)
		{
			Value = value;
		}
		public NAttribute(int value)
		{
			Value = new Attribute(value);
		}
	}
}