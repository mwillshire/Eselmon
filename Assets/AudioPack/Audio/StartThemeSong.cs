using UnityEngine;

public class StartThemeSong : MonoBehaviour
{
    public static StartThemeSong themeInstance;

    private void Awake()
    {
        if (themeInstance == null)
            themeInstance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        FindObjectOfType<AudioHandler>().Play("Theme");
    }
}
