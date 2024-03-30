using System;

namespace Kapibara.RPS
{
	[Serializable]
	public class NInt : NotificableField<int>
	{
		public NInt(int value)
		{
			Value = value;
		}
	}
}