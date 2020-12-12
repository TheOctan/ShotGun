using Assets.Scripts.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Receivers
{
	public interface IScoreReceiver
	{
		int ConnectionTimeout { get; }
		IEnumerator GetScore(int minCount, Action<IEnumerable<ScoreData>> callback);
		IEnumerator GetScore(string nickname, SessionData session, int minCount, Action<IEnumerable<ScoreData>> callback);
	}
}
