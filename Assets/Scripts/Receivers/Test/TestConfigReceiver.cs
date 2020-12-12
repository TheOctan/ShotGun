using Assets.Scripts.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Receiver/TestConfig")]
public class TestConfigReceiver : BaseConfigReceiver
{
	public override int ConnectionTimeout => connectionTimeout;

	[SerializeField] private int connectionTimeout;
	[SerializeField] private List<UserConfig> configs;

	public override IEnumerator GetConfigurations(string nickname, Action<ConfigData> callback)
	{
		System.Random random = new System.Random();
		yield return new WaitForSeconds(random.Next(1, 4));

		var user = configs.SingleOrDefault(e => e.Nickname == nickname);
		if (user != null)
		{
			callback(user.Config);
		}
		else
		{
			callback(null);
		}
	}

	public override IEnumerator SendConfigurations(string nickname, ConfigData config, Action callback)
	{
		System.Random random = new System.Random();
		yield return new WaitForSeconds(random.Next(1, 4));

		var user = configs.SingleOrDefault(e => e.Nickname == nickname);
		if(user != null)
		{
			user.Config = config;
		}
		else
		{
			configs.Add(new UserConfig() { Nickname = nickname, Config = config });
		}
	}

	[Serializable]
	public class UserConfig
	{
		public string Nickname;
		public ConfigData Config;
	}
}
