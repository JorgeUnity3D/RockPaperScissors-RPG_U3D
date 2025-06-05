using System;
using System.Collections.Generic;

namespace Kapibara.Util.NotificableFields
{
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