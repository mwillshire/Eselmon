using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Enemy
{
    public string enemyID { get; private set; }
    public string enemyName { get; private set; }
    public string prefabName { get; private set; }
    public GameObject prefab {  get; private set; }
    public List<bool> hasDied { get; private set; }
    public List<Vector2> location { get; private set; }

    public Enemy ( string enemyID, string enemyName, string prefabName)
    {
        this.enemyID = enemyID;
        this.enemyName = enemyName;
        this.prefabName = prefabName;

        hasDied = new List<bool>();
        location = new List<Vector2>();

        prefab = Resources.Load<GameObject>(prefabName);
    }

    public void SetHasDied (bool deathValue, Vector2 startingPos)
    {
        for ( int i = 0; i < location.Count; i++ )
        {
            if (startingPos == location[i]) hasDied[i] = deathValue;
        }
    }

    //The location is the starting location
    public void AddEnemy(Vector2 location)
    {
        this.location.Add(location);
        hasDied.Add(false);
    }

    public void SpawnEnemy()
    {
        for (int i = 0; i < location.Count; i++)
        {
            if (!hasDied[i])
            {
                GameObject.Instantiate(prefab, location[i], Quaternion.identity);
            }
        }
    }
}
