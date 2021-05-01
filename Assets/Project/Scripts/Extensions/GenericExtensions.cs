using System;
using System.Collections.Generic;

namespace Assets.Scripts.Extensions
{
	public static class GenericExtensions
	{
		public static IList<T> Shuffle<T>(this IList<T> list)
		{
			return list.Shuffle(DateTime.Now.Second);
		}
		public static IList<T> Shuffle<T>(this IList<T> list, int seed)
		{
			Random random = new Random(seed);
			for (int i = 0; i < list.Count - 1; i++)
			{
				int randomIndex = random.Next(i, list.Count);

				T temp = list[randomIndex];
				list[randomIndex] = list[i];
				list[i] = temp;
			}

			return list;
		}

		public static void Swap<T>(ref T left, ref T right)
		{
			T temp = left;
			left = right;
			right = temp;
		}
	}
}