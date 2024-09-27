using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class FinalPlayerScore : MonoBehaviour
{
    private Text uiText;

    void Start()
    {
        uiText = GetComponent<Text>();
        uiText.text = "Your Score: " + PlayerPrefs.GetInt("PlayerScore").ToString("#,0");
    }
}
