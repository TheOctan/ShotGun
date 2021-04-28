using Assets.Scripts.Data;
using Assets.Scripts.Receivers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseConfigReceiver : ScriptableObject, IConfigReceiver
{
	public virtual int ConnectionTimeout => -1;

	public abstract IEnumerator GetConfigurations(string nickname, Action<ConfigData> callback);

	public abstract IEnumerator SendConfigurations(string nickname, ConfigData config, Action callback);
}
