using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetScores : MonoBehaviour
{
    public void ResetPlayerScore()
    {
        PlayerPrefs.SetInt("PlayerScore", 0);
        PlayerPrefs.SetInt("BeginScore", 0);
    }
}
