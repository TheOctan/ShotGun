using Assets.Scripts.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfigurationManager : MonoBehaviour
{
	public static ConfigData configData { get; private set; } = new ConfigData();

	public Slider[] volumeSliders;
	public Toggle[] resolutionToggles;
	public Toggle fullscreenToggle;
	public int[] screenWidths;

	[Header("Nickname control")]
	public InputController nicknameInputController;

	private int activeScreenResIndex;

	public const string SCREEN_RES_KEY = "screen res index";
	public const string FULLSCREEN_KEY = "fullscreen";
	public const string NICKNAME_KEY = "nickname";

	public const string MASTER_VOLUME_KEY = "master vol";
	public const string SFX_VOLUME_KEY = "sfx vol";
	public const string MUSIC_VOLUME_KEY = "music vol";

	private void Awake()
	{
		LoadConfig();
	}

	private void Start()
	{
		InitializeConfig();
	}

	public void LoadConfig()
	{
		configData.ResolutionIndex = PlayerPrefs.GetInt(SCREEN_RES_KEY);
		configData.IsFullScreen = (PlayerPrefs.GetInt(FULLSCREEN_KEY) == 1) ? true : false;

		configData.MasterVolume = PlayerPrefs.GetFloat(MASTER_VOLUME_KEY, 1);
		configData.SoundVolume = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 1);
		configData.MusicVolume = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, 1);

		configData.Nickname = PlayerPrefs.GetString(NICKNAME_KEY);
	}
	public void InitializeConfig()
	{
		volumeSliders[0].value = configData.MasterVolume;
		volumeSliders[1].value = configData.MusicVolume;
		volumeSliders[2].value = configData.SoundVolume;

		for (int i = 0; i < resolutionToggles.Length; i++)
		{
			resolutionToggles[i].isOn = i == configData.ResolutionIndex;
		}

		fullscreenToggle.isOn = configData.IsFullScreen;
		nicknameInputController.SetValue(configData.Nickname);
	}

	public void OnEndInputNickname(InputController inputController)
	{
		configData.Nickname = inputController.text;

		if (inputController.IsValid && inputController.text != string.Empty)
		{
			PlayerPrefs.SetString(NICKNAME_KEY, inputController.text);
		}
		else
		{
			PlayerPrefs.DeleteKey(NICKNAME_KEY);
		}
	}

	public void SetScreenResolution(int i)
	{
		if (resolutionToggles[i].isOn)
		{
			activeScreenResIndex = i;
			float aspectRatio = 16 / 9f;
			Screen.SetResolution(screenWidths[i], (int)(screenWidths[i] / aspectRatio), false);
			PlayerPrefs.SetInt(SCREEN_RES_KEY, activeScreenResIndex);
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

		PlayerPrefs.SetInt(FULLSCREEN_KEY, isFullscreen ? 1 : 0);
		PlayerPrefs.Save();
	}

	public void SetMasterVolume(float value)
	{
		AudioManager.instance.SetVolume(value, AudioManager.AudioChannel.Master);

		PlayerPrefs.SetFloat(MASTER_VOLUME_KEY, value);
		PlayerPrefs.Save();
	}

	public void SetMusicVolume(float value)
	{
		AudioManager.instance.SetVolume(value, AudioManager.AudioChannel.Music);

		PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, value);
		PlayerPrefs.Save();
	}

	public void SetSfxVolume(float value)
	{
		AudioManager.instance.SetVolume(value, AudioManager.AudioChannel.Sfx);

		PlayerPrefs.SetFloat(SFX_VOLUME_KEY, value);
		PlayerPrefs.Save();
	}
}
