using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class BackgroundMovement : MonoBehaviour
{
    [Header("Inscribed")]
    public float speed;
    public GameObject[] background;
    public int backgroundWidth;
    public int cameraWidth;

    private void Start()
    {
        background[0].transform.position = Vector3.zero;
        for (int i = 1; i < background.Length; i++)
        {
            background[i].transform.position = new Vector3(backgroundWidth * i, 0, 0);
        }
    }

    private void FixedUpdate()
    {
        foreach (GameObject back in background)
        {
            Vector3 newPos = back.transform.position;
            newPos.x -= speed;
            if (back.transform.position.x <= -1 * (cameraWidth/2 + backgroundWidth/2))
            {
                newPos.x = (background.Length - 1) * (cameraWidth / 2 + backgroundWidth / 2);
            }

            back.transform.position = newPos;
        }
    }
}
