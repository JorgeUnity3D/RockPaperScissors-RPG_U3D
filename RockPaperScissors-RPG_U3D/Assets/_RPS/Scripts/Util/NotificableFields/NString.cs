using System;

namespace Kapibara.Util.NotificableFields
{
	/// <summary>NotificableField especializado para cadenas de texto.</summary>
	[Serializable]
	public class NString : NotificableField<string>
	{
		public NString(string value)
		{
			Value = value;
		}
	}
}