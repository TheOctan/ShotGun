using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	private float masterVolumePercent = 0.2f;
	private float sfxVolumePercent = 1;
	private float musicVolumePercent = 1;

	private AudioSource[] musicSources;
	private int activeMusicSourceIndex;

	public static AudioManager instance;

	private Transform audioListener;
	private Transform playerT;

	private SoundLibrary library;

	void Awake()
	{
		instance = this;

		library = GetComponent<SoundLibrary>();

		musicSources = new AudioSource[2];
		for (int i = 0; i < musicSources.Length; i++)
		{
			GameObject newMusicSource = new GameObject("Music source " + (i + 1));
			musicSources[i] = newMusicSource.AddComponent<AudioSource>();
			newMusicSource.transform.parent = transform;
		}

		audioListener = FindObjectOfType<AudioListener>().transform;
		playerT = FindObjectOfType<Player>().transform;
	}

	void Update()
	{
		if(playerT != null)
		{
			audioListener.position = playerT.position;
		}
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
		if(clip != null)
		{
			AudioSource.PlayClipAtPoint(clip, pos, sfxVolumePercent * masterVolumePercent);
		}
	}

	public void PlaySound(string sound, Vector3 pos)
	{
		PlaySound(library.GetClipFromName(sound), pos);
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
