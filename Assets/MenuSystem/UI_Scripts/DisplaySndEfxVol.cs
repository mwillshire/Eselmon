using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class DisplaySndEfxVol : MonoBehaviour
{
    static private Text _UI_TEXT;
    static private float _VOLUMESEV = 1;

    private void Awake()
    {
        _UI_TEXT = this.gameObject.GetComponent<Text>();

        if (PlayerPrefs.HasKey("SoundEffectsVolume"))
        {
            VOLUMESEV = PlayerPrefs.GetFloat("SoundEffectsVolume");
        }

        PlayerPrefs.SetFloat("SoundEffectsVolume", VOLUMESEV);
    }

    static public float VOLUMESEV
    {
        get { return _VOLUMESEV; }
        private set
        {
            _VOLUMESEV = value;
            //PlayerPrefs.SetFloat("SoundEffectsVolume", value);

            if (_UI_TEXT != null)
            {
                _UI_TEXT.text = value.ToString();
            }
        }
    }
    public void Update()
    {
        VOLUMESEV = PlayerPrefs.GetFloat("SoundEffectsVolume");
    }
}
