using Assets.Scripts.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreReceiver : BaseScoreReceiver
{
	public override IEnumerator GetScore(int minCount, Action<IEnumerable<ScoreData>> callback)
	{
		throw new NotImplementedException();
	}

	public override IEnumerator GetScore(string nickname, SessionData session, int minCount, Action<IEnumerable<ScoreData>> callback)
	{
		throw new NotImplementedException();
	}
}