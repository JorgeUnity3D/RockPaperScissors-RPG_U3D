using System;
namespace Kapibara.RPS
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