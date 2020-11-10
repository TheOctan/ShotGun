﻿using Assets.Scripts.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Receivers
{
	public interface IReceiver
	{
		IEnumerator GetItems(int minCount, Action<IEnumerable<ScoreData>> callback);
	}
}
