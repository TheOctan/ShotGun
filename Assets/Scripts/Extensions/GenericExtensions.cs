

namespace Assets.Scripts.Extensions
{
	public static class GenericExte1nsions
	{
		public static T[] Shuffle<T>(this T[] array)
		{
			return array.PhisherShuffle(System.DateTime.Now.Second);
		}
		public static T[] PhisherShuffle<T>(this T[] array, int seed)
		{
			System.Random random = new System.Random(seed);
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