using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Chest
{
    public string chestID { get; private set; }
    public string chestName { get; private set; }
    public string prefabName { get; private set; }
    public GameObject prefab {  get; private set; }
    public List<bool> hasBeenOpened { get; private set; }
    public List<Vector2> location { get; private set; } //the number of Vector2 determines the number of this type of chest

    public Chest (string chestID, string chestname, string prefabName)
    {
        this.chestID = chestID;
        this.chestName = chestname;
        this.prefabName = prefabName;

        hasBeenOpened = new List<bool>();
        location = new List<Vector2>();

        prefab = Resources.Load<GameObject>(prefabName);
    }

    public void SetHasBeenOpened(bool isOpened, Vector2 position)
    {
        for (int i = 0; i < location.Count; i++)
        {
            if (position == location[i]) hasBeenOpened[i] = isOpened;
        }
    }

    public void NewChest(Vector2 location)
    {
        this.location.Add(location);
        hasBeenOpened.Add(false);
    }

    //spawns all chests of this type
    public void SpawnChests()
    {
        for (int i = 0;i < location.Count;i++)
        {
            if (!hasBeenOpened[i])
            {
                GameObject.Instantiate(prefab, location[i], Quaternion.identity);
            }
        }
    }

}
