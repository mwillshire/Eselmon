//Controls Theme songs and when they play. 

using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class SongController : MonoBehaviour
{
    [Header("Inscribed")]
    public string menuSong;
    public string roamSong;
    public int menuSceneID;
    public int firstGridSceneID;

    private string currentlyPlaying;

    private AudioHandler aH;

    private void Awake()
    {
        aH = gameObject.GetComponent<AudioHandler>();
    }

    private void FixedUpdate()
    {
        int sceneID = SceneManager.GetActiveScene().buildIndex;

        if (sceneID == menuSceneID && currentlyPlaying != menuSong)
        {
            aH.Play(menuSong);
            currentlyPlaying = menuSong;
        }
        else if (sceneID >= firstGridSceneID && currentlyPlaying != roamSong)
        {
            aH.Play(roamSong);
            currentlyPlaying = roamSong;
        }
    }
}
