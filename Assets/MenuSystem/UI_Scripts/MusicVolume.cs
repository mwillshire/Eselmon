using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class MusicVolume : MonoBehaviour
{
    private Slider volumeSlider;
    private float volume;

    private void Awake()
    {
        volumeSlider = GetComponent<Slider>();
        volumeSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        volume = volumeSlider.value;
    }

    private float Volume
    {
        get { return volume; }
        set
        {
            PlayerPrefs.SetFloat("MusicVolume", value);
            FindObjectOfType<AudioHandler>().ChangeMusicVolume();
        }
    }

    private void Update()
    {
        Volume = volumeSlider.value;
    }
}
