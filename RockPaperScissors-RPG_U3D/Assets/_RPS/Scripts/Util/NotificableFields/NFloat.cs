using Kapibara.Util.NotificableFields;
using System;

namespace Kapira.Util.NotificableFields
{
	/// <summary>NotificableField especializado para valores en coma flotante.</summary>
	[Serializable]
	public class NFloat : NotificableField<float>
    {
		public NFloat(float value)
		{
			Value = value;
		}
	}
}