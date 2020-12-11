using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class TestRegistrationSender : BaseSender
{
	public override int ConnectionTimeout => connectionTimeout;

	[SerializeField] private int connectionTimeout;
	[SerializeField] private bool checkNickname;
	[SerializeField] private string[] nicknames;

	public override IEnumerator Send(string nickname, string passwordHash, Action<bool> verificate)
	{
		System.Random random = new System.Random();
		yield return new WaitForSeconds(random.Next(1, 3));

		Debug.Log($"Nickname: {nickname}, PasswordHash: {passwordHash}");

		verificate(!checkNickname || !nicknames.Contains(nickname));
	}
}
