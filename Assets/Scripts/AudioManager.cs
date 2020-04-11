using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public enum AudioChannel
	{
		Master = 0,
		Sfx,
		Music
	}

	private float masterVolumePercent = 0.2f;
	private float sfxVolumePercent = 1;
	private float musicVolumePercent = 1;

	private AudioSource sfx2DSource;
	private AudioSource[] musicSources;
	private int activeMusicSourceIndex;

	public static AudioManager instance;

	private Transform audioListener;
	private Transform playerT;

	private SoundLibrary library;

	void Awake()
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
			playerT = FindObjectOfType<Player>().transform;

			masterVolumePercent = PlayerPrefs.GetFloat("master vol", masterVolumePercent);
			sfxVolumePercent = PlayerPrefs.GetFloat("sfx vol", sfxVolumePercent);
			musicVolumePercent = PlayerPrefs.GetFloat("music vol", musicVolumePercent);
		}
	}

	void Update()
	{
		if (playerT != null)
		{
			audioListener.position = playerT.position;
		}
	}

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

		PlayerPrefs.SetFloat("master vol", masterVolumePercent);
		PlayerPrefs.SetFloat("sfx vol", sfxVolumePercent);
		PlayerPrefs.SetFloat("music vol", musicVolumePercent);
	}

	public void PlayMusic(AudioClip clip, float fadeDuration = 1)
	{
		activeMusicSourceIndex = 1 - activeMusicSourceIndex;
		musicSources[activeMusicSourceIndex].clip = clip;
		musicSources[activeMusicSourceIndex].Play();

		StartCoroutine(AnimatemusicCrossfade(fadeDuration));
	}

	public void PlaySound(AudioClip clip, Vector3 pos)
	{
		if (clip != null)
		{
			AudioSource.PlayClipAtPoint(clip, pos, sfxVolumePercent * masterVolumePercent);
		}
	}

	public void PlaySound(string sound, Vector3 pos)
	{
		PlaySound(library.GetClipFromName(sound), pos);
	}

	public void PlaySound2D(string sound)
	{
		sfx2DSource.PlayOneShot(library.GetClipFromName(sound), sfxVolumePercent * masterVolumePercent);
	}

	IEnumerator AnimatemusicCrossfade(float duration)
	{
		float percent = 0;

		while (percent < 1)
		{
			percent += Time.deltaTime * 1 / duration;
			musicSources[activeMusicSourceIndex].volume = Mathf.Lerp(0, musicVolumePercent * masterVolumePercent, percent);
			musicSources[1 - activeMusicSourceIndex].volume = Mathf.Lerp(musicVolumePercent * masterVolumePercent, 0, percent);

			yield return null;
		}
	}
}
