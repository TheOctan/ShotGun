using System.Collections;
using System.Collections.Generic;

public static class Utility
{
	public static T[] Shuffle<T>(this T[] array)
	{
		return Shuffle(array, System.DateTime.Now.Second);
	}
	public static T[] Shuffle<T>(this T[] array, int seed)
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
