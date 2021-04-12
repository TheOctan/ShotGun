using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts.Data;

namespace Assets.Scripts.Legacy
{
	public class AccountMenu : TimeoutBehaviour
	{
		[Header("Buttons")]
		[SerializeField] private Button saveButton;
		[SerializeField] private Button loadButton;
		[SerializeField] private Button loginButton;
		[SerializeField] private Button logoutButton;
		[SerializeField] private Button registrationButton;

		[Header("Receiver"), SerializeField]
		private BaseConfigReceiver configReceiver;

		[Header("Menu")]
		public ConfigurationManager configurationManager;

		[Header("UI elements")]
		public GameObject loadingWindow;
		[SerializeField] private GameObject loginedText;
		[SerializeField] private Text login;
		public Text prompt;

		private void OnEnable()
		{
			if (ConfigurationManager.LoginData.IsLogined)
			{
				UpdateLoginUI(ConfigurationManager.LoginData.Nickname);
			}
		}
		private void OnDisable()
		{
			prompt.text = "";
		}

		public void OnSave()
		{
			loadingWindow.SetActive(true);

			var login = ConfigurationManager.LoginData.Nickname;
			var config = ConfigurationManager.ConfigData;

			InvokeTimeoutAction(
				configReceiver.SendConfigurations(login, config, SaveCallback),
				configReceiver.ConnectionTimeout
				);
		}
		public void OnLoad()
		{
			loadingWindow.SetActive(true);

			var login = ConfigurationManager.LoginData.Nickname;

			InvokeTimeoutAction(
				configReceiver.GetConfigurations(login, LoadCallback),
				configReceiver.ConnectionTimeout
				);
		}

		public void Login(string nickname)
		{
			ConfigurationManager.LoginData.IsLogined = true;
			ConfigurationManager.LoginData.Nickname = nickname;

			UpdateLoginUI(nickname);
		}
		public void UpdateLoginUI(string nickname)
		{
			loginedText.SetActive(true);
			login.text = nickname;

			loginButton.gameObject.SetActive(false);
			logoutButton.gameObject.SetActive(true);
			registrationButton.interactable = false;

			EnableConfigButtons();
		}
		public void Logout()
		{
			ConfigurationManager.LoginData.IsLogined = false;
			ConfigurationManager.LoginData.Nickname = string.Empty;

			UpdateLogoutUI();
		}
		public void UpdateLogoutUI()
		{
			loginedText.SetActive(false);

			loginButton.gameObject.SetActive(true);
			logoutButton.gameObject.SetActive(false);
			registrationButton.interactable = true;

			DisableConfigButtons();
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

		private void SaveCallback()
		{
			if (timeoutCoroutine != null)
				StopCoroutine(timeoutCoroutine);

			loadButton.interactable = true;
			loadingWindow.SetActive(false);
			prompt.text = "";
		}
		private void LoadCallback(ConfigData config)
		{
			if (timeoutCoroutine != null)
				StopCoroutine(timeoutCoroutine);

			if (config != null)
			{
				ConfigurationManager.ConfigData = config;
				configurationManager.InitializeConfig();
				prompt.text = "";
			}
			else
			{
				loadButton.interactable = false;
				prompt.text = "No saved configurations";
			}

			loadingWindow.SetActive(false);
		}
		private void EnableConfigButtons()
		{
			saveButton.interactable = true;
			loadButton.interactable = true;
		}
		private void DisableConfigButtons()
		{
			saveButton.interactable = false;
			loadButton.interactable = false;
		}
	}
}