using System;

namespace Assets.Scripts.Extensions
{
	public static class GenericExtensions
	{
		public static T[] Shuffle<T>(this T[] array)
		{
			return array.PhisherShuffle(DateTime.Now.Second);
		}
		public static T[] PhisherShuffle<T>(this T[] array, int seed)
		{
			Random random = new Random(seed);
			for (int i = 0; i < array.Length - 1; i++)
			{
				int randomIndex = random.Next(i, array.Length);
				Swap(ref array[randomIndex], ref array[i]);
			}

			return array;
		}

		public static void Swap<T>(ref T left, ref T right)
		{
			T temp = left;
			left = right;
			right = temp;
		}
	}
}