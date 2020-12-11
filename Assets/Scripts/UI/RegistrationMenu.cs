using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegistrationMenu : MonoBehaviour
{
	[Header("Input")]
	public InputField nickname;
	public InputField password;
	public InputField dublicatePassword;

	[Header("Sender"), SerializeField]
	private TestRegistrationSender sender = null;

	[Header("UI elements")]
	public Button registerButton;
	public GameObject loadingWindow;
	public Text prompt;

	private void Awake()
	{
		password.asteriskChar = '•';
		dublicatePassword.asteriskChar = '•';
	}

	public void OnRegister()
	{
		loadingWindow.SetActive(true);

		StartCoroutine(sender.Send(nickname.text, password.text, Verificate));
	}

	

	private void Verificate(bool isVerified)
	{
		loadingWindow.SetActive(false);
		
	}
}
