//The queue list contains the queueMiniBoxPrefab for each player and enemy in combat
//These prefabs will receive a reference to their respective player or enemy 
//Each player and enemy will then have a TakeATurn Method that when iterating the queue, will be called
//This method will call IterateQueue at the end of the method or if it is the player,
//The button will trigger IterateQueue.

//There is probably a better way of doing this, but this is what I have now. The combat controller
//will order all the player and enemy initiatives then add them to the queue.

using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CombatQueueBox : MonoBehaviour
{
    [Header("Inscribed")]
    public GameObject queueMiniBoxPrefab;
    public Vector2 startingPos;
    public float spaceBetweenBoxes;


    [Header("Dynamic")]
    public List<GameObject> queueBoxes { get; private set; }

    private GameObject canvasObject;

    private void Start()
    {
        queueBoxes = new List<GameObject>();

        canvasObject = FindFirstObjectByType<Canvas>().gameObject;
    }

    private void FixedUpdate()
    {
        if (!StaticVariables.combatMode)
        {
            foreach (GameObject go in queueBoxes)
            {
                Destroy(go);
            }
            queueBoxes.Clear();
        }
        if (queueBoxes.Count == 0) return;

        DisplayBoxes();
    }

    private void DisplayBoxes()
    {
        for (int i = 0; i < queueBoxes.Count; i++)
        {
            if (i > 4)
            {
                queueBoxes[i].SetActive(false);
                return;
            }

            queueBoxes[i].SetActive(true);
            Vector2 temp = startingPos;
            temp.x += (spaceBetweenBoxes * i);
            queueBoxes[i].transform.position = temp; 
        }
    }

    public void AddToQueue(GameObject prefab)
    {
        GameObject tempObj = Instantiate(queueMiniBoxPrefab, Vector3.zero, Quaternion.identity);

        tempObj.SetActive(false);

        tempObj.GetComponent<QueueMiniBox>().objectRef = prefab;

        tempObj.transform.SetParent(canvasObject.transform, false);
        tempObj.transform.localScale = new Vector3(1, 1, 1);

        queueBoxes.Add(tempObj);
    }

    public void IterateQueue()
    {
        queueBoxes.Add(queueBoxes[0]);
        queueBoxes.Remove(queueBoxes[0]);
        DisplayBoxes();
    }

    public void RemoveFromQueue(GameObject prefab)
    {
        GameObject temp = FindBoxFromPrefab(prefab);

        //This may be doing the same thing twice
        Destroy(temp);
        queueBoxes.Remove(temp);
    }

    private GameObject FindBoxFromPrefab(GameObject prefab)
    {
        foreach (GameObject obj in queueBoxes)
        {
            if (prefab = obj.GetComponent<QueueMiniBox>().objectRef) return obj; 
        }

        return null;
    }
}
