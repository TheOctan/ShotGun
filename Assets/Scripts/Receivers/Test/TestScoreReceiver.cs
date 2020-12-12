using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Data;

[CreateAssetMenu(menuName = "Receiver/TestScore")]
public class TestScoreReceiver : BaseScoreReceiver
{
	public override int ConnectionTimeout => connectionTimeout;

	[SerializeField] private int connectionTimeout;
	[SerializeField] private List<ScoreData> scoreData;
	private System.Random random = new System.Random();

	public override IEnumerator GetScore(int minCount, Action<IEnumerable<ScoreData>> callback)
	{
		yield return new WaitForSecondsRealtime(random.Next(1, 4));

		callback(scoreData.OrderByDescending(e => e.Score));
	}

	public override IEnumerator GetScore(string nickname, SessionData session, int minCount, Action<IEnumerable<ScoreData>> callback)
	{
		yield return new WaitForSecondsRealtime(random.Next(1, 4));

		scoreData.Add(new ScoreData() { Name = nickname, Date = session.Date, Score = session.Score });

		callback(scoreData.OrderByDescending(e => e.Score));
	}	
}
