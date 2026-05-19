using System;
using System.Collections.Generic;

namespace Kapibara.Util.NotificableFields
{
	/// <summary>NotificableField especializado para listas genéricas. Cualquier reasignación de Value dispara el guardado.</summary>
	[Serializable]
	public class NList<T> : NotificableField<List<T>>
	{
		public NList()
		{
			Value = new List<T>();
		}
		public NList(List<T> value)
		{
			Value = value;
		}
	}
}