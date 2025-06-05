using Kapibara.Util.NotificableFields;
using System;

namespace Kapira.Util.NotificableFields
{
	[Serializable]
	public class NFloat : NotificableField<float>
    {
		public NFloat(float value)
		{
			Value = value;
		}
	}
}