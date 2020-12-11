using Cryptography;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegistrationMenu : TimeoutBehaviour
{
	[Header("Input")]
	public InputField nickname;
	public InputField password;
	public InputField dublicatePassword;

	[Header("Sender"), SerializeField]
	private TestRegistrationSender sender = null;

	[Header("Menu")]
	public AccountMenu menu;

	[Header("UI elements")]
	public GameObject loadingWindow;
	public Text prompt;

	

	private void Awake()
	{
		password.asteriskChar = '•';
		dublicatePassword.asteriskChar = '•';
	}

	private void OnDisable()
	{
		prompt.text = "";
	}

	public void OnRegister()
	{
		if (ValidateNickname() && ValidatePassword())
		{
			loadingWindow.SetActive(true);

			InvokeTimeoutAction(sender.Send(nickname.text, Crypt.ComputeHash(password.text), Verificate), sender.ConnectionTimeout);
		}		
	}

	protected override void OnTimeout()
	{
		base.OnTimeout();
		prompt.text = "No connection: timeout";
	}

	protected override void OnPostTimeout()
	{
		base.OnPostTimeout();
		loadingWindow.SetActive(false);
	}

	private bool ValidateNickname()
	{
		if (nickname.text != string.Empty)
		{
			return true;
		}
		else
		{
			nickname.image.color = Color.red;
			prompt.text = "You must input nickaname";
		}

		return false;
	}

	private bool ValidatePassword()
	{
		if (password.text == string.Empty)
		{
			password.image.color = Color.red;
			prompt.text = "You must input password";
			return false;
		}
		if (password.text.Length < 8)
		{
			password.image.color = Color.red;
			prompt.text = "Passwords length must greater or equal 8 signs";
			return false;
		}
		if (dublicatePassword.text == string.Empty)
		{
			dublicatePassword.image.color = Color.red;
			prompt.text = "You must dublicate password";
			return false;
		}		
		if (password.text.Length != dublicatePassword.text.Length || password.text != dublicatePassword.text)
		{
			dublicatePassword.image.color = Color.red;
			prompt.text = "Passwords do not match";
			return false;
		}

		return true;
	}

	private void Verificate(bool isVerified)
	{
		StopCoroutine(timeOutCoroutine);

		if (isVerified)
		{
			menu.Register(nickname.text);
			ResetMenu();
			gameObject.SetActive(false);
		}
		else
		{
			nickname.image.color = Color.red;
			prompt.text = "This login is already taken";
		}

		loadingWindow.SetActive(false);
	}

	private void ResetMenu()
	{
		nickname.text = string.Empty;
		password.text = string.Empty;
		dublicatePassword.text = string.Empty;
	}
}
