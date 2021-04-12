using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
	[Header("Scenes")]
	public string firstSceneName = "ModernMainMenu";
	public string secondSceneName = "Game";

	[Header("Audio clips")]
	public AudioClip mainTheme;
	public AudioClip menuTheme;

	string sceneName;

	private void OnEnable()
	{
		SceneManager.sceneLoaded += OnLevelFinishedLoading;
	}

	private void OnDisable()
	{
		SceneManager.sceneLoaded -= OnLevelFinishedLoading;
	}

	private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
	{
		string newSceneName = scene.name;
		if (newSceneName != sceneName)
		{
			sceneName = newSceneName;
			Invoke("PlayMusic", 0.2f);
		}
	}

	private void PlayMusic()
	{
		AudioClip clipToPlay = null;

		if(sceneName == firstSceneName)
		{
			clipToPlay = menuTheme;
		}
		else if(sceneName == secondSceneName)
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
