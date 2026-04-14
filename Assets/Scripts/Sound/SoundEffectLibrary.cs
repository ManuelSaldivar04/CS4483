using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectLibrary : MonoBehaviour
{
    [SerializeField] private SoundEffctGroup[] soundEffctGroups;
    private Dictionary<string, List<AudioClip>> soundDictionary;

    private void Awake()
    {
        InitializeDictionary();
    }

    private void InitializeDictionary()
    {
        soundDictionary = new Dictionary<string, List<AudioClip>>();
        foreach(SoundEffctGroup soundEffctGroup in soundEffctGroups)
        {
            soundDictionary[soundEffctGroup.name] = soundEffctGroup.audioClips;
        }
    }

    public AudioClip GetRandomClip(string name)
    {
        if (soundDictionary.ContainsKey(name))
        {
            List<AudioClip> audioClips = soundDictionary[name];
            if(audioClips.Count > 0)
            {
                return audioClips[UnityEngine.Random.Range(0,audioClips.Count)];
            }
        }
        return null;
    }

    public string[] GetAllSoundGroupNames()
    {
        string[] names = new string[soundEffctGroups.Length];
        for (int i = 0; i < soundEffctGroups.Length; i++)
        {
            names[i] = soundEffctGroups[i].name;
        }
        return names;
    }
}

[System.Serializable]
public struct SoundEffctGroup
{
    public string name;
    public List<AudioClip> audioClips;
}
