using System;

namespace Kapibara.Util.NotificableFields
{
	/// <summary>NotificableField especializado para valores enteros.</summary>
	[Serializable]
	public class NInt : NotificableField<int>
	{
		public NInt(int value)
		{
			Value = value;
		}
	}
}