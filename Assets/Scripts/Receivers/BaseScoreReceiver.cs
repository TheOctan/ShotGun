using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Receivers;
using Assets.Scripts.Data;
using UnityEngine;

public abstract class BaseScoreReceiver : ScriptableObject, IScoreReceiver
{
	public virtual int ConnectionTimeout => -1;
	public abstract IEnumerator GetItems(int minCount, Action<IEnumerable<ScoreData>> callback);
}
