using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class DisplayMusicVol : MonoBehaviour
{
    static private Text _UI_TEXT;
    static private float _VOLUMEDMV = 1;

    private void Awake()
    {
        _UI_TEXT = this.gameObject.GetComponent<Text>();

        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            VOLUMEDMV = PlayerPrefs.GetFloat("MusicVolume");
        }

        PlayerPrefs.SetFloat("MusicVolume", VOLUMEDMV);
    }

    static public float VOLUMEDMV
    {
        get { return _VOLUMEDMV; }
        private set
        {
            _VOLUMEDMV = value;
            //PlayerPrefs.SetFloat("MusicVolume", value);

            if (_UI_TEXT != null)
            {
                _UI_TEXT.text = value.ToString();
            }
        }
    }

    public void Update()
    {
        VOLUMEDMV = PlayerPrefs.GetFloat("MusicVolume");
    }
}
