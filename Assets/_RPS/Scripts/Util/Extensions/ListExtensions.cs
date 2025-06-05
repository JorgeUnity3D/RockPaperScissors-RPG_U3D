using System.Collections.Generic;

namespace Kapibara.Util.Extensions
{
	public static class ListExtensions
	{
		public static void ShuffleList<T>(this List<T> list)
		{
			System.Random rng = new System.Random();
			int n = list.Count;
			while (n > 1)
			{
				n--;
				int k = rng.Next(n + 1);
				while (k == n)
				{
					k = rng.Next(n + 1);
				}

				T value = list[k];
				list[k] = list[n];
				list[n] = value;
			}
		}
	}
}