using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
	public GameObject mainMenuHolder;
	public GameObject optionMenuHolder;
	public GameObject inputMenuHolder;

	[Header("Nickname control")]
	public InputField nicknameInput;
	public InputField spareNicknameInput;

	private InputController nicknameController;
	private InputController spareNicknameController;

	
	private string nickname;

	private void Awake()
	{
		nicknameController = nicknameInput.GetComponent<InputController>();
		spareNicknameController = spareNicknameInput.GetComponent<InputController>();
	}

	public void Play()
	{
		if (nicknameInput.text == string.Empty || !nicknameController.IsValid)
		{
			mainMenuHolder.SetActive(false);
			inputMenuHolder.SetActive(true);

			if (!nicknameController.IsValid)
			{
				spareNicknameInput.text = nicknameInput.text;
				spareNicknameInput.GetComponent<Image>().color = Color.red;
			}
		}
		else
		{
			SceneManager.LoadScene("Game");
		}
	}

	public void Next()
	{
		if (spareNicknameController.IsValid)
		{
			PlayerPrefs.SetString(ConfigurationManager.NICKNAME_KEY, nicknameInput.text);
			SceneManager.LoadScene("Game");
		}
	}

	public void Quit()
	{
#if UNITY_EDITOR
		EditorApplication.isPlaying = false;
#else
	Application.Quit();
#endif
	}

	public void OptionsMenu()
	{
		mainMenuHolder.SetActive(false);
		optionMenuHolder.SetActive(true);

		//if(spareNicknameInput.text != string.Empty)
		//{
		//	nicknameInput.text = spareNicknameInput.text;

		//	if (!spareNicknameController.IsValid)
		//	{
		//		nicknameInput.GetComponent<Image>().color = Color.red;
		//	}
		//}
		//else
		//{
		//	nicknameInput.text = nickname;
		//}
	}

	public void MainMenu()
	{
		mainMenuHolder.SetActive(true);
		optionMenuHolder.SetActive(false);
	}
}
