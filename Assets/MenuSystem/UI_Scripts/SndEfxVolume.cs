using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class SndEfxVolume : MonoBehaviour
{
    private Slider volumeSlider;
    private float volume;

    private void Awake()
    {
        volumeSlider = GetComponent<Slider>();
        volumeSlider.value = PlayerPrefs.GetFloat("SoundEffectsVolume");
        volume = volumeSlider.value;
    }

    private float Volume
    {
        get { return volume; }
        set
        {
            PlayerPrefs.SetFloat("SoundEffectsVolume", value);
            FindObjectOfType<AudioHandler>().ChangeSndEfxVolume();
        }
    }

    private void Update()
    {
        Volume = volumeSlider.value;
    }
}
