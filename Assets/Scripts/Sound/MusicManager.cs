// MusicManager.cs (extended)
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [System.Serializable]
    public struct SceneMusic
    {
        [SceneName] public string sceneName;
        [SoundName] public string musicSoundName;   // dropdown of sound effect names
    }

    [SerializeField] private SceneMusic[] sceneMusicArray;
    [SerializeField] private SoundEffectLibrary soundEffectLibrary; // Drag reference here

    private AudioSource audioSource;
    private Dictionary<string, string> sceneMusicMap; // scene -> sound name

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            BuildSceneMap();
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void BuildSceneMap()
    {
        sceneMusicMap = new Dictionary<string, string>();
        foreach (var item in sceneMusicArray)
        {
            sceneMusicMap[item.sceneName] = item.musicSoundName;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
       
        if (sceneMusicMap.TryGetValue(scene.name, out string soundName))
        {
            // Play the sound using your existing SoundEffectManager
            
            SoundEffectManager.PlayLooping(soundName);
        }
        else
        {
            //no music defined for this scene therfore stop any currently playing music
            SoundEffectManager.StopLoopingMusic();
        }
      
    }
}