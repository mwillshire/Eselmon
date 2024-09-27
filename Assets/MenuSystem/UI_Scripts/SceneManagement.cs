using System.Collections;
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{

    [Header("Timer Transition")]
    public bool isTimed = false;
    public float time = 0;

    [Header("Key Press Transition")]
    public bool isSkipableByKey = false;
    public List<KeyCode> keys = new List<KeyCode> { KeyCode.Escape };
    public bool withSound = false;
    public string soundName;

    [Header("Basic Information")]
    public int sceneID;

    private void Start()
    {
        if (isTimed)
            Invoke("LoadScene", time);
    }

    private void Update()
    {
        if (isSkipableByKey)
            if (Input.GetKeyDown(keys[0]))
            {
                if (withSound) FindObjectOfType<AudioHandler>().Play(soundName);
                Invoke("LoadScene", .01f);
            }
    }
    public void LoadScene ()
    {
        SceneManager.LoadScene (sceneID);
    }

    public void LoadScene(int sceneID)
    {
        SceneManager.LoadScene(sceneID);
    }

    public void ExitGame ()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit;
#endif
    }
}
