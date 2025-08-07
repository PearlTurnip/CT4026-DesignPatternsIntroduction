using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    [System.Serializable]
    public struct AudioItem
    {
        public string name;
        public AudioClip clip;
    }

    [SerializeField]
    private AudioItem[] audioItems;

    [RuntimeInitializeOnLoadMethod]
    public static void OnStartUnity()
    {
        var instance = GameObject.Instantiate(Resources.Load("SoundController"));
        DontDestroyOnLoad(instance);
    }

    private static SoundController _instance;

    public static SoundController Instance
    {
        get
        {
            return _instance;
        }
    }

    private AudioSource musicSource;
    private AudioSource soundfxSource;
    private float musicVolume = 1f;
    private float soundfxVolume = 1f;

    void Awake()
    {
        _instance = this;

        // Setup the music looping
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
    }

    private AudioClip GetClip(string name)
    {
        foreach (AudioItem item in audioItems)
        {
            if (item.name == name)
            {
                return item.clip;
            }
        }
        return null;
    }

    public void PlayMusic(string trackName)
    {
        if (musicVolume <= 0)
            return;

        musicSource.clip = GetClip(trackName);
        musicSource.volume = musicVolume;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PlaySoundEffect(string soundEffectName)
    {
        if (soundfxVolume <= 0)
            return;

        AudioClip clip = GetClip(soundEffectName);
        if (clip == null)
            return;

        AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();
        newAudioSource.clip = GetClip(soundEffectName);
        newAudioSource.Play();
        Destroy(newAudioSource, clip.length);
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        musicSource.volume = volume;
    }

    public void SetSoundFXVolume(float volume)
    {
        soundfxVolume = volume;
    }
}
