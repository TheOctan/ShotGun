using Assets.Scripts.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Receiver/Test/Config")]
public class TestConfigReceiver : BaseConfigReceiver
{
	public override int ConnectionTimeout => connectionTimeout;

	[SerializeField] private int connectionTimeout;
	[SerializeField] private List<UserConfig> configs;

	private System.Random random = new System.Random();

	public override IEnumerator GetConfigurations(string nickname, Action<ConfigData> callback)
	{
		yield return new WaitForSecondsRealtime(random.Next(1, 4));

		var user = configs.SingleOrDefault(e => e.Nickname == nickname);
		if (user != null)
		{
			callback(CloneConfig(user.Config));
		}
		else
		{
			callback(null);
		}
	}

	public override IEnumerator SendConfigurations(string nickname, ConfigData config, Action callback)
	{
		yield return new WaitForSecondsRealtime(random.Next(1, 4));

		var user = configs.SingleOrDefault(e => e.Nickname == nickname);
		if(user != null)
		{
			user.Config = CloneConfig(config);
		}
		else
		{
			configs.Add(new UserConfig() { Nickname = nickname, Config = CloneConfig(config) });
		}

		callback();
	}

	private ConfigData CloneConfig(ConfigData config)
	{
		return new ConfigData()
		{
			MasterVolume = config.MasterVolume,
			MusicVolume = config.MusicVolume,
			SoundVolume = config.SoundVolume,
			ResolutionIndex = config.ResolutionIndex,
			IsFullScreen = config.IsFullScreen
		};
	}

	[Serializable]
	public class UserConfig
	{
		public string Nickname;
		public ConfigData Config;
	}
}
