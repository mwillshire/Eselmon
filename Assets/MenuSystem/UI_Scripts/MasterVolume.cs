using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class MasterVolume : MonoBehaviour
{
    private Slider volumeSlider;
    private float volume;

    private void Awake()
    {
        volumeSlider = GetComponent<Slider>();
        volumeSlider.value = PlayerPrefs.GetFloat("MasterVolume");
        volume = volumeSlider.value;
    }

    private float Volume
    { get { return volume; }
        set 
        {
            PlayerPrefs.SetFloat("MasterVolume", value);
            AudioHandler aH = FindObjectOfType<AudioHandler>();

            aH.ChangeMusicVolume();
            aH.ChangeSndEfxVolume();
        }
    }

    private void Update()
    {
        Volume = volumeSlider.value;
    }
}
