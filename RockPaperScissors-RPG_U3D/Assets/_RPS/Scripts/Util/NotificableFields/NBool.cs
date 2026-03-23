using System;

namespace Kapibara.Util.NotificableFields
{
	/// <summary>NotificableField especializado para valores booleanos.</summary>
	[Serializable]
	public class NBool : NotificableField<bool>
	{
		public NBool(bool value)
		{
			Value = value;
		}
	}
}