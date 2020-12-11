using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Senders
{
	public interface ISender
	{
		int ConnectionTimeout { get; }
		IEnumerator Send(string nickname, string passwordHash, Action<bool> verificate);
	}
}
