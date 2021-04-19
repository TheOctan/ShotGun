using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Data;

[CreateAssetMenu(menuName = "Receiver/Test/Score")]
public class TestScoreReceiver : BaseScoreReceiver
{
	public override int ConnectionTimeout => connectionTimeout;

	[SerializeField] private int connectionTimeout;
	[SerializeField] private List<ScoreData> scoreData;
	private System.Random random = new System.Random();

	public override IEnumerator GetScore(int maxCount, Action<IEnumerable<ScoreData>> callback)
	{
		yield return new WaitForSecondsRealtime(random.Next(1, 4));
		callback(scoreData.OrderByDescending(e => e.Score).Take(maxCount));
	}

	public override IEnumerator GetScore(int maxCount, Action<IEnumerable<ScoreData>> callback, ScoreData score)
	{
		yield return new WaitForSecondsRealtime(random.Next(1, 4));

		scoreData.Add(score);
		callback(scoreData.OrderByDescending(e => e.Score).Take(maxCount));
	}	
}
