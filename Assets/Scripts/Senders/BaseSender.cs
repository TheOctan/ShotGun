using System;
using System.Collections;
using Assets.Scripts.Senders;
using UnityEngine;

public abstract class BaseSender : ScriptableObject, ISender
{
	public virtual int ConnectionTimeout => -1;
	public abstract IEnumerator Send(string nickname, string passwordHash, Action<bool> verificate);
}
