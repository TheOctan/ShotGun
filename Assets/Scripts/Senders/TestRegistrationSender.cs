using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class TestRegistrationSender : BaseSender
{
	[SerializeField] private bool isRegistrated;

	public override IEnumerator Send(string nickname, string passwordHash, Action<bool> verificate)
	{
		System.Random random = new System.Random();
		yield return new WaitForSeconds(random.Next(1, 3));

		verificate(isRegistrated);
	}
}
