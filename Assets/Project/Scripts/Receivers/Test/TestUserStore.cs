using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Receiver/Test/UserStore")]
public class TestUserStore: ScriptableObject
{
	public List<User> users;
}

[Serializable]
public struct User
{
	public string Nickname;
	public string Hash;
}

