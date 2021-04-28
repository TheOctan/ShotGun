using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace OctanGames.UI.Menu
{
	public class MainMenu : MonoBehaviour
	{
		public void OnPlay()
		{
			SceneManager.LoadScene("Game");
		}

		public void OnExit()
		{
#if UNITY_EDITOR
			EditorApplication.isPlaying = false;
#else
	Application.Quit();
#endif
		}
	}
}