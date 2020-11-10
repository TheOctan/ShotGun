using UnityEngine;
using System.Collections;
using Assets.Scripts.Receivers;
using System;
using System.Collections.Generic;
using Assets.Scripts.Data;

public abstract class BaseReceiver : ScriptableObject, IReceiver
{
	public abstract IEnumerator GetItems(int minCount, Action<IEnumerable<ScoreData>> callback);
}
