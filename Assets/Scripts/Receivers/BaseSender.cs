using Assets.Scripts.Receivers;
using System;
using System.Collections;
using UnityEngine;

public abstract class BaseReceiver : ScriptableObject, IReceiver
{
	public virtual int ConnectionTimeout => -1;
	public abstract IEnumerator Send(string nickname, string passwordHash, Action<bool> verificate);
}
