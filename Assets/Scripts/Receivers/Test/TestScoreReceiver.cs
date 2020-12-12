using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Data;

[CreateAssetMenu(menuName = "Receiver/TestScore")]
public class TestScoreReceiver : BaseScoreReceiver
{
	public override IEnumerator GetItems(int minCount, Action<IEnumerable<ScoreData>> callback)
	{
		System.Random random = new System.Random();
		yield return new WaitForSeconds(random.Next(1, 4));

		List<ScoreData> scoreData = new List<ScoreData>();
		for (int i = 0; i < minCount; i++)
		{
			scoreData.Add(new ScoreData()
			{
				Name = $"Name{i}",
				Score = random.Next(100, 5000)
			});
		}

		callback(scoreData.OrderByDescending(e => e.Score));
	}
}
