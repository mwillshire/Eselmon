using UnityEngine;

//This class is used for buttons and other UI sounds whether in the menu or in game
public class PlaySound : MonoBehaviour
{
    public string soundName;

    public void PlaySndEfx()
    {
        try
        {
            FindObjectOfType<AudioHandler>().Play(soundName);
        }
        catch 
        {
            Debug.LogWarning("Failed to Play: " + soundName);
        }
    }
}
