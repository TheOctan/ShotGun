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

	public Slider[] volumeSliders;
	public Toggle[] resolutionToggles;
	public Toggle fullscreenToggle;
	public int[] screenWidths;

	[Header("Nickname control")]
	public InputField nicknameInput;
	public InputField spareNicknameInput;

	private InputController nicknameController;
	private InputController spareNicknameController;

	private int activeScreenResIndex;
	private string nickname;

	private void Awake()
	{
		nicknameController = nicknameInput.GetComponent<InputController>();
		spareNicknameController = spareNicknameInput.GetComponent<InputController>();
	}

	private void Start()
	{
		activeScreenResIndex = PlayerPrefs.GetInt("screen res index");
		bool isFullScreen = (PlayerPrefs.GetInt("fullscreen") == 1) ? true : false;

		volumeSliders[0].value = AudioManager.instance.masterVolumePercent;
		volumeSliders[1].value = AudioManager.instance.musicVolumePercent;
		volumeSliders[2].value = AudioManager.instance.sfxVolumePercent;

		for (int i = 0; i < resolutionToggles.Length; i++)
		{
			resolutionToggles[i].isOn = i == activeScreenResIndex;
		}

		fullscreenToggle.isOn = isFullScreen;

		if (PlayerPrefs.HasKey("Nickname"))
		{
			nickname = PlayerPrefs.GetString("Nickname");
		}
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
			PlayerPrefs.SetString("Nickname", nicknameInput.text);
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

		if(spareNicknameInput.text != string.Empty)
		{
			nicknameInput.text = spareNicknameInput.text;

			if (!spareNicknameController.IsValid)
			{
				nicknameInput.GetComponent<Image>().color = Color.red;
			}
		}
		else
		{
			nicknameInput.text = nickname;
		}
	}

	public void MainMenu()
	{
		mainMenuHolder.SetActive(true);
		optionMenuHolder.SetActive(false);

		if (nicknameController.IsValid)
		{
			PlayerPrefs.SetString("Nickname", nicknameInput.text);
		}
	}

	public void SetScreenResolution(int i)
	{
		if (resolutionToggles[i].isOn)
		{
			activeScreenResIndex = i;
			float aspectRatio = 16 / 9f;
			Screen.SetResolution(screenWidths[i], (int)(screenWidths[i] / aspectRatio), false);
			PlayerPrefs.SetInt("screen res index", activeScreenResIndex);
			PlayerPrefs.Save();
		}
	}

	public void SetFullscreen(bool isFullscreen)
	{
		for (int i = 0; i < resolutionToggles.Length; i++)
		{
			resolutionToggles[i].interactable = !isFullscreen;
		}

		if (isFullscreen)
		{
			Resolution[] allResolutions = Screen.resolutions;
			Resolution maxResolution = allResolutions[allResolutions.Length - 1];
			Screen.SetResolution(maxResolution.width, maxResolution.height, true);
		}
		else
		{
			SetScreenResolution(activeScreenResIndex);
		}

		PlayerPrefs.SetInt("fullscreen", isFullscreen ? 1 : 0);
		PlayerPrefs.Save();
	}

	public void SetMasterVolume(float value)
	{
		AudioManager.instance.SetVolume(value, AudioManager.AudioChannel.Master);
	}

	public void SetMusicVolume(float value)
	{
		AudioManager.instance.SetVolume(value, AudioManager.AudioChannel.Music);
	}

	public void SetSfxVolume(float value)
	{
		AudioManager.instance.SetVolume(value, AudioManager.AudioChannel.Sfx);
	}
}
