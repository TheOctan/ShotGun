using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AccountMenu : MonoBehaviour
{
	[Header("Buttons")]
	[SerializeField] private Button saveButton;
	[SerializeField] private Button loadButton;
	[SerializeField] private Button loginButton;
	[SerializeField] private Button logoutButton;
	[SerializeField] private Button registrationButton;

	[Header("UI elements")]
	[SerializeField] private GameObject loginedText;
	[SerializeField] private Text login;

	private void OnEnable()
	{
		if(ConfigurationManager.configData.IsLogined)
		{
			UpdateLoginUI(ConfigurationManager.configData.Nickname);
		}
	}

	public void Login(string nickname)
	{
		ConfigurationManager.configData.IsLogined = true;
		ConfigurationManager.configData.Nickname = nickname;

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
		ConfigurationManager.configData.IsLogined = false;
		ConfigurationManager.configData.Nickname = string.Empty;

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
