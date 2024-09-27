using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name; 

    public AudioClip clip;

    public bool multipleClips;
    public AudioClip[] mClips;

    [Range(0f, 1f)]
    public float volume;
    [Range(.1f, 3f)]
    public float pitch;

    public bool loop;
    public bool isMusic;

    [HideInInspector]
    public AudioSource source;
}
