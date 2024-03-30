using System;
namespace Kapibara.RPS
{
	[Serializable]
	public class NString : NotificableField<string>
	{
		public NString(string value)
		{
			Value = value;
		}
	}
}