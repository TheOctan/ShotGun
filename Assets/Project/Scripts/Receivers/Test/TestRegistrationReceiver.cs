using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "Receiver/Test/Registration")]
public class TestRegistrationReceiver : BaseReceiver
{
	public override int ConnectionTimeout => connectionTimeout;

	[SerializeField] private int connectionTimeout;
	[SerializeField] private bool checkNickname;
	[SerializeField] private TestUserStore store;

	public override IEnumerator Send(string nickname, string passwordHash, Action<bool> verificate)
	{
		System.Random random = new System.Random();
		yield return new WaitForSecondsRealtime(random.Next(1, 4));

		Debug.Log($"Nickname: {nickname}, PasswordHash: {passwordHash}");

		bool isVerified = !checkNickname || !store.users.Select(e => e.Nickname).Contains(nickname);
		if (isVerified)
		{
			store.users.Add(new User() { Nickname = nickname, Hash = passwordHash });
		}
		verificate(isVerified);
	}
}
