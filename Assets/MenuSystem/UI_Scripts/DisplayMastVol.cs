using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class DisplayMastVol : MonoBehaviour
{
    static private Text _UI_TEXT;
    static private float _VOLUMEDAV = 1;

    private void Awake()
    {
        _UI_TEXT = this.gameObject.GetComponent<Text>();

        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            VOLUMEDAV = PlayerPrefs.GetFloat("MasterVolume");
        }

        PlayerPrefs.SetFloat("MasterVolume", VOLUMEDAV);
    }

    static public float VOLUMEDAV
    {
        get { return _VOLUMEDAV; }
        private set
        {
            _VOLUMEDAV = value;
            //PlayerPrefs.SetFloat("MasterVolume", value);

            if (_UI_TEXT != null)
            {
                _UI_TEXT.text = value.ToString();
            }
        }
    }

    public void Update()
    {
        VOLUMEDAV = PlayerPrefs.GetFloat("MasterVolume");
    }
}
