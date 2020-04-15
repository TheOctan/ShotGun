using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
	public AudioClip mainTheme;
	public AudioClip menuTheme;

	string sceneName;

	void OnEnable()
	{
		SceneManager.sceneLoaded += OnLevelFinishedLoading;
	}

	void OnDisable()
	{
		SceneManager.sceneLoaded -= OnLevelFinishedLoading;
	}

	void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
	{
		string newSceneName = scene.name;
		if (newSceneName != sceneName)
		{
			sceneName = newSceneName;
			Invoke("PlayMusic", 0.2f);
		}
	}

	void PlayMusic()
	{
		AudioClip clipToPlay = null;

		if(sceneName == "Menu")
		{
			clipToPlay = menuTheme;
		}
		else if(sceneName == "Game")
		{
			clipToPlay = mainTheme;
		}

		if(clipToPlay != null)
		{
			AudioManager.instance.PlayMusic(clipToPlay, 2);
			Invoke("PlayMusic", clipToPlay.length);
		}
	}
}
