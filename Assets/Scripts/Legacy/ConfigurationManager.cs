using Assets.Scripts.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Legacy
{
	public class ConfigurationManager : MonoBehaviour
	{
		public static ConfigData ConfigData { get; set; } = new ConfigData();
		public static LoginData LoginData { get; private set; } = new LoginData();

		public Slider[] volumeSliders;
		public Toggle[] resolutionToggles;
		public Toggle fullscreenToggle;
		public int[] screenWidths;

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
			ConfigData.ResolutionIndex = PlayerPrefs.GetInt(SCREEN_RES_KEY);
			ConfigData.IsFullScreen = PlayerPrefs.GetInt(FULLSCREEN_KEY) == 1 ? true : false;

			ConfigData.MasterVolume = PlayerPrefs.GetFloat(MASTER_VOLUME_KEY, 1);
			ConfigData.SoundVolume = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 1);
			ConfigData.MusicVolume = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, 1);

			LoginData.Nickname = PlayerPrefs.GetString(NICKNAME_KEY);
		}

		public void InitializeConfig()
		{
			volumeSliders[0].value = ConfigData.MasterVolume;
			volumeSliders[1].value = ConfigData.MusicVolume;
			volumeSliders[2].value = ConfigData.SoundVolume;

			for (int i = 0; i < resolutionToggles.Length; i++)
			{
				resolutionToggles[i].isOn = i == ConfigData.ResolutionIndex;
			}

			fullscreenToggle.isOn = ConfigData.IsFullScreen;
		}

		public void SetScreenResolution(int i)
		{
			if (resolutionToggles[i].isOn)
			{
				activeScreenResIndex = i;
				float aspectRatio = 16 / 9f;
				Screen.SetResolution(screenWidths[i], (int)(screenWidths[i] / aspectRatio), false);

				ConfigData.ResolutionIndex = activeScreenResIndex;
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

			ConfigData.IsFullScreen = isFullscreen;
			PlayerPrefs.SetInt(FULLSCREEN_KEY, isFullscreen ? 1 : 0);
			PlayerPrefs.Save();
		}

		public void SetMasterVolume(float value)
		{
			AudioManager.instance.SetVolume(value, AudioManager.AudioChannel.Master);

			ConfigData.MasterVolume = value;
			PlayerPrefs.SetFloat(MASTER_VOLUME_KEY, value);
			PlayerPrefs.Save();
		}

		public void SetMusicVolume(float value)
		{
			AudioManager.instance.SetVolume(value, AudioManager.AudioChannel.Music);

			ConfigData.MusicVolume = value;
			PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, value);
			PlayerPrefs.Save();
		}

		public void SetSoundVolume(float value)
		{
			AudioManager.instance.SetVolume(value, AudioManager.AudioChannel.Sfx);

			ConfigData.SoundVolume = value;
			PlayerPrefs.SetFloat(SFX_VOLUME_KEY, value);
			PlayerPrefs.Save();
		}
	}
}