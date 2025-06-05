using System;

namespace Kapibara.Util.NotificableFields
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