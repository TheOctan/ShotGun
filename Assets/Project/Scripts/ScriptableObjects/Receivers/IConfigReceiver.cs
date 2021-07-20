using Assets.Scripts.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Receivers
{
	interface IConfigReceiver
	{
		int ConnectionTimeout { get; }
		IEnumerator GetConfigurations(string nickname, Action<ConfigData> callback);
		IEnumerator SendConfigurations(string nickname, ConfigData config, Action callback);
	}
}
