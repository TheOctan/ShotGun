using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundLibrary : MonoBehaviour
{
	public SoundGroup[] soundGroups;

	private Dictionary<string, AudioClip[]> groupDictionary = new Dictionary<string, AudioClip[]>();

	void Awake()
	{
		foreach (var soundGroup in soundGroups)
		{
			groupDictionary.Add(soundGroup.groupID, soundGroup.group);
		}
	}

	public AudioClip GetClipFromName(string name)
	{
		if (groupDictionary.ContainsKey(name))
		{
			AudioClip[] sounds = groupDictionary[name];
			return sounds[Random.Range(0, sounds.Length)];
		}
		return null;
	}

	[System.Serializable]
	public class SoundGroup
	{
		public string groupID;
		public AudioClip[] group;
	}
}
