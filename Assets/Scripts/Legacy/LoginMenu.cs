using Cryptography;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Legacy
{
	public class LoginMenu : TimeoutBehaviour
	{
		[Header("Input")]
		public InputField nickname;
		public InputField password;

		[Header("Sender"), SerializeField]
		private BaseReceiver sender = null;

		[Header("Menu")]
		public AccountMenu menu;

		[Header("UI elements")]
		public GameObject loadingWindow;
		public Text prompt;

		private void Awake()
		{
			password.asteriskChar = '•';
		}
		private void OnDisable()
		{
			ResetMenu();
			prompt.text = "";
		}

		public void OnLogin()
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
			if (password.text != string.Empty)
			{
				return true;
			}
			else
			{
				password.image.color = Color.red;
				prompt.text = "You must input password";
			}

			return false;
		}
		private void Verificate(bool isVerified)
		{
			if (timeoutCoroutine != null)
				StopCoroutine(timeoutCoroutine);

			if (isVerified)
			{
				menu.Login(nickname.text);
				gameObject.SetActive(false);
			}
			else
			{
				prompt.text = "Incorrect password or username";
			}

			loadingWindow.SetActive(false);
		}
		private void ResetMenu()
		{
			nickname.text = string.Empty;
			password.text = string.Empty;
		}
	}
}