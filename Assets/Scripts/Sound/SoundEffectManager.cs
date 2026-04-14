using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundEffectManager : MonoBehaviour
{
    private static SoundEffectManager Instance;

    private static AudioSource audioSource;
    private static AudioSource randomPitchAudioSource;
    private static AudioSource voiceAudioSource; //NPC audio sound
    private static AudioSource musicSource; //background music
    private static SoundEffectLibrary soundEffectLibrary;
    [SerializeField] private Slider sfxSlider;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            AudioSource[] audioSources = GetComponents<AudioSource>();
            audioSource = audioSources[0];
            randomPitchAudioSource = audioSources[1];
            voiceAudioSource = audioSources[2];
            voiceAudioSource.volume = 0.5f;
            musicSource = audioSources[3];
            musicSource.volume = 0.2f; 
            soundEffectLibrary = GetComponent<SoundEffectLibrary>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    private void Start()
    {
        sfxSlider.onValueChanged.AddListener(delegate { OnValueChanged(); });
    }

    public static void Play(string soundName, bool randomPitch = false)
    {
        AudioClip audioClip = soundEffectLibrary.GetRandomClip(soundName);
        if(audioClip != null)
        {
            if (randomPitch)
            {
                randomPitchAudioSource.pitch = Random.Range(1f, 1.5f);
                randomPitchAudioSource.PlayOneShot(audioClip);
            }
            else
            {
                audioSource.PlayOneShot(audioClip);
            }
        }
    }
    
    public static void PlayVoice(AudioClip audioClip, float pitch = 1f)
    {
        voiceAudioSource.pitch = pitch;
        voiceAudioSource.PlayOneShot(audioClip);
    }
    public static void SetVolume(float volume)
    {
        audioSource.volume = volume;
        randomPitchAudioSource.volume = volume;
        audioSource.volume = volume;
        musicSource.volume = volume;
    }
    public void OnValueChanged()
    {
        SetVolume(sfxSlider.value);
    }

    public static void PlayLooping(string soundName)
    {
        AudioClip clip = soundEffectLibrary.GetRandomClip(soundName);
        if(clip != null)
        {
            musicSource.loop = true;
            musicSource.clip = clip;
            musicSource.Play();
        }
    }

    public static void StopLoopingMusic()
    {
        if(musicSource != null && musicSource.isPlaying)
        {
            musicSource.Stop();
        }
    }
}
