using Assets.Scripts.Legacy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
	public enum AudioChannel
	{
		Master = 0,
		Sfx,
		Music
	}

	public float masterVolumePercent { get; private set; }
	public float sfxVolumePercent { get; private set; }
	public float musicVolumePercent { get; private set; }

	[SerializeField] private Vector3 listenerOffset = Vector3.up;

	private AudioSource sfx2DSource;
	private AudioSource[] musicSources;
	private int activeMusicSourceIndex;

	public static AudioManager instance;

	private Transform audioListener;
	private Transform playerT;

	private SoundLibrary library;

	public void SetVolume(float volumePercent, AudioChannel channel)
	{
		switch (channel)
		{
			case AudioChannel.Master:
				masterVolumePercent = volumePercent;
				break;
			case AudioChannel.Sfx:
				sfxVolumePercent = volumePercent;
				break;
			case AudioChannel.Music:
				musicVolumePercent = volumePercent;
				break;
			default:
				break;
		}

		musicSources[0].volume = musicVolumePercent * masterVolumePercent;
		musicSources[1].volume = musicVolumePercent * masterVolumePercent;
	}
	public void PlayMusic(AudioClip clip, float fadeDuration = 1)
	{
		activeMusicSourceIndex = 1 - activeMusicSourceIndex;
		musicSources[activeMusicSourceIndex].clip = clip;
		musicSources[activeMusicSourceIndex].Play();

		StartCoroutine(AnimateMusicCrossfade(fadeDuration));
	}
	public void PlaySound(AudioClip clip, Vector3 pos)
	{
		if (clip != null)
		{
			AudioSource.PlayClipAtPoint(clip, pos, sfxVolumePercent * masterVolumePercent);
		}
	}
	public void PlaySound(string soundName, Vector3 pos)
	{
		PlaySound(library.GetClipFromName(soundName), pos);
	}
	public void PlaySound2D(string soundName)
	{
		sfx2DSource.PlayOneShot(library.GetClipFromName(soundName), sfxVolumePercent * masterVolumePercent);
	}

	private void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			DontDestroyOnLoad(gameObject);

			library = GetComponent<SoundLibrary>();

			musicSources = new AudioSource[2];
			for (int i = 0; i < musicSources.Length; i++)
			{
				GameObject newMusicSource = new GameObject("Music source " + (i + 1));
				musicSources[i] = newMusicSource.AddComponent<AudioSource>();
				newMusicSource.transform.parent = transform;
			}

			GameObject newSfx2Dsource = new GameObject("2D sfx source");
			sfx2DSource = newSfx2Dsource.AddComponent<AudioSource>();
			newSfx2Dsource.transform.parent = transform;

			audioListener = FindObjectOfType<AudioListener>().transform;
			var player = FindObjectOfType<PlayerController>();
			if (player != null)
			{
				playerT = player.transform;
			}

			masterVolumePercent = 1;
			sfxVolumePercent = 1;
			musicVolumePercent = 1;
		}
	}
	private void OnEnable()
	{
		SceneManager.sceneLoaded += OnLevelFinishedLoading;
	}
	private void OnDisable()
	{
		SceneManager.sceneLoaded -= OnLevelFinishedLoading;
	}
	private void LateUpdate()
	{
		if (playerT != null)
		{
			audioListener.position = playerT.position + listenerOffset;
		}
	}

	private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
	{
		if (playerT == null)
		{
			if (FindObjectOfType<PlayerController>() != null)
			{
				playerT = FindObjectOfType<PlayerController>().transform;
			}
		}
	}
	private IEnumerator AnimateMusicCrossfade(float duration)
	{
		float percent = 0;

		while (percent < 1)
		{
			percent += Time.deltaTime * 1 / duration;
			musicSources[activeMusicSourceIndex].volume = Mathf.Lerp(0, musicVolumePercent * masterVolumePercent, percent);
			musicSources[1 - activeMusicSourceIndex].volume = Mathf.Lerp(musicVolumePercent * masterVolumePercent, 0, percent);
			
			yield return null;
		}

		musicSources[1 - activeMusicSourceIndex].clip = null;
	}
}
