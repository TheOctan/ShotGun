using System;
using System.Collections;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Receiver/TestAuthorization")]
public class TestAuthorizationReceiver : BaseReceiver
{
	public override int ConnectionTimeout => connectionTimeout;

	[SerializeField] private int connectionTimeout;
	[SerializeField] private bool checkUser;
	[SerializeField] private TestUserStore store;

	public override IEnumerator Send(string nickname, string passwordHash, Action<bool> verificate)
	{
		System.Random random = new System.Random();
		yield return new WaitForSecondsRealtime(random.Next(1, 4));

		Debug.Log($"Nickname: {nickname}, PasswordHash: {passwordHash}");

		bool isVerified = !checkUser || store.users.Any(e => e.Nickname == nickname && e.Hash == passwordHash);
		verificate(isVerified);
	}	
}
