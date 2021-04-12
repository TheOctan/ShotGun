using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace OctanGames.UI
{
	public class MainMenu : MonoBehaviour
	{
		public Selectable firstSelectedItem = null;

		public void OnPlay()
		{
			SceneManager.LoadScene("Game");
		}

		public void OnQuit()
		{
#if UNITY_EDITOR
			EditorApplication.isPlaying = false;
#else
	Application.Quit();
#endif
		}
	}
}