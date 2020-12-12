using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Receivers;
using Assets.Scripts.Data;
using UnityEngine;

public abstract class BaseScoreReceiver : ScriptableObject, IScoreReceiver
{
	public virtual int ConnectionTimeout => -1;
	public abstract IEnumerator GetScore(int minCount, Action<IEnumerable<ScoreData>> callback);
	public abstract IEnumerator GetScore(string nickname, SessionData session, int minCount, Action<IEnumerable<ScoreData>> callback);
}
