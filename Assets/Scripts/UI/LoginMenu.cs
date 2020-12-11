using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginMenu : MonoBehaviour
{
	[Header("Input")]
	public InputField nickname;
	public InputField password;

	[Header("Sender"), SerializeField]
	private BaseSender sender = null;

	[Header("UI elements")]
	public GameObject loadingWindow;
	public Text prompt;

	private void Awake()
	{
		password.asteriskChar = '•';
	}

	public void OnLogin()
	{
		loadingWindow.SetActive(true);

		StartCoroutine(sender.Send(nickname.text, password.text, Verificate));
	}



	private void Verificate(bool isVerified)
	{
		loadingWindow.SetActive(false);

	}
}
