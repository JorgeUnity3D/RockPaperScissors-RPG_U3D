using System;

namespace Kapibara.Util.NotificableFields
{
	[Serializable]
	public class NBool : NotificableField<bool>
	{
		public NBool(bool value)
		{
			Value = value;
		}
	}
}