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
	public InputController nicknameController;
	public InputController spareNicknameController;
	
	public void Play()
	{
		if (ConfigurationManager.configData.Nickname == string.Empty || !nicknameController.IsValid)
		{
			mainMenuHolder.SetActive(false);
			inputMenuHolder.SetActive(true);

			if (!nicknameController.IsValid)
			{
				spareNicknameController.SetValue(nicknameController.text);
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
	}

	public void MainMenu()
	{
		mainMenuHolder.SetActive(true);
		optionMenuHolder.SetActive(false);
	}
}
