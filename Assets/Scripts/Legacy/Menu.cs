using OctanGames.Experimental;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.Legacy
{
	[Obsolete("Class menu has been depricated: Use class MainMenu")]
	public class Menu : MonoBehaviour
	{
		[Header("Menu")]
		public GameObject mainMenuHolder;
		public GameObject optionMenuHolder;

		[Header("UIelements")]
		[SerializeField] private GameObject loginedText;
		[SerializeField] private Text login;

		private void OnEnable()
		{
			if (ConfigurationManager.LoginData.IsLogined)
			{
				loginedText.SetActive(true);
				login.text = ConfigurationManager.LoginData.Nickname;
			}
		}

		public void Play()
		{
			SceneManager.LoadScene("Game");
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
}