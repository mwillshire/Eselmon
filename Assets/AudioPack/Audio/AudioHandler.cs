//By Brackeys
using System;

using UnityEngine.Audio;
using UnityEngine;

public class AudioHandler : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioHandler instance;

    private static float mainAudioVolume;
    private static float musicAudioVolume;
    private static float sndEfxAudioVolume;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        mainAudioVolume = PlayerPrefs.GetFloat("MasterVolume");
        musicAudioVolume = PlayerPrefs.GetFloat("MusicVolume");
        sndEfxAudioVolume = PlayerPrefs.GetFloat("SoundEffectsVolume");

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();

            if (!s.multipleClips)
            {
                s.source.clip = s.clip;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;
            }

            if(s.isMusic)
                s.source.volume = s.volume * (mainAudioVolume / 100) * (musicAudioVolume / 100);
            else
                s.source.volume = s.volume * (mainAudioVolume / 100) * (sndEfxAudioVolume / 100);
        }
    }

    public void ChangeMusicVolume()
    {
        mainAudioVolume = PlayerPrefs.GetFloat("MasterVolume");
        musicAudioVolume = PlayerPrefs.GetFloat("MusicVolume");
        foreach (Sound s in sounds)
        {
            if (s.isMusic)
                s.source.volume = s.volume * (mainAudioVolume / 100) * (musicAudioVolume / 100);
        }
    }
    public void ChangeSndEfxVolume()
    {
        mainAudioVolume = PlayerPrefs.GetFloat("MasterVolume");
        sndEfxAudioVolume = PlayerPrefs.GetFloat("SoundEffectsVolume");

        foreach (Sound s in sounds)
        {
            if (!s.isMusic)
                s.source.volume = s.volume * (mainAudioVolume / 100) * (sndEfxAudioVolume / 100);
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.source.Play();
        //Debug.Log("Playing: " + name);
    }

    //For audio like walking where each type of step is a seperate clip, only call when there is an array of clips
    public void PlayMultiClipSound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        int index = UnityEngine.Random.Range(0, s.mClips.Length - 1);
        s.source.clip = s.mClips[index];

        s.source.Play();
    }
}
