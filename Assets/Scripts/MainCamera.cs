using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class MainCamera : MonoBehaviour
{
    [Header("Inscribed")]
    public Vector2 bottomLCorner;
    public Vector2 topRCorner;
    public float speed;

    private Transform playerTransform;

    private float cameraWidth;
    private float cameraHeight;

    private void Awake()
    {
        cameraHeight = Camera.main.orthographicSize * 2f;
        cameraWidth = cameraHeight * Camera.main.aspect;

        PlayerController player = FindFirstObjectByType<PlayerController>();
        playerTransform = player.gameObject.GetComponent<Transform>();

    }

    private void FixedUpdate()
    {

        if (StaticVariables.combatMode)
        {
            Vector3 position = transform.position;

            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) position.x += 1 * speed;
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) position.y += 1 * speed;
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) position.y -= 1 * speed;
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) position.x -= 1 * speed;

            if (position.x - cameraWidth / 2 < bottomLCorner.x) position.x = bottomLCorner.x + cameraWidth / 2;
            else if (position.x + cameraWidth / 2 > topRCorner.x) position.x = topRCorner.x - cameraWidth / 2;

            if (position.y - cameraHeight / 2 < bottomLCorner.y) position.y = bottomLCorner.y + cameraHeight / 2;
            else if (position.y + cameraHeight / 2 > topRCorner.y) position.y = topRCorner.y - cameraHeight / 2;

            if (position.x - playerTransform.position.x > cameraWidth / 2) position.x = transform.position.x;
            else if (playerTransform.position.x - position.x > cameraWidth / 2) position.x = transform.position.x;

            if (position.y - playerTransform.position.y > cameraHeight / 2) position.y = transform.position.y;
            else if (playerTransform.position.y - position.y > cameraHeight / 2) position.y = transform.position.y;

            transform.position = position;
        }
        else
        {
            Vector3 position = new Vector3(0, 0, -10);

            if (playerTransform.position.x - cameraWidth / 2 < bottomLCorner.x) position.x = bottomLCorner.x + cameraWidth / 2;
            else if (playerTransform.position.x + cameraWidth / 2 > topRCorner.x) position.x = topRCorner.x - cameraWidth / 2;
            else position.x = playerTransform.transform.position.x;

            if (playerTransform.position.y - cameraHeight / 2 < bottomLCorner.y) position.y = bottomLCorner.y + cameraHeight / 2;
            else if (playerTransform.position.y + cameraHeight / 2 > topRCorner.y) position.y = topRCorner.y - cameraHeight / 2;
            else position.y = playerTransform.transform.position.y;

            transform.position = position;
        }

    }
}
