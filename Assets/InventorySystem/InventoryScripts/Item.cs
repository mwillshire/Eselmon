using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Item
{
    public string itemID {  get; private set; }
    public int amount;
    public int itemStackAmount { get; private set; }
    public Sprite sprite { get; private set; }
    public int value { get; private set; }
    public string prefabName { get; private set; }
    public GameObject prefab { get; private set; }
    

    public Item(string itemID, int amount, int value, string prefabName)
    {
        this.itemID = itemID;
        this.amount = amount;  
        this.value = value;
        this.prefabName = prefabName;

        ConnectSpriteAndPrefab();
        FindStackAmount();

    }

    private void FindStackAmount() //Sets value for stack amount
    {
        itemStackAmount = 1;

        if (itemID.Substring(0, 1) == "I") itemStackAmount = 64;
    }

    private void ConnectSpriteAndPrefab() //adds the sprite to the item using the sprite name.
    {
        prefab = Resources.Load<GameObject>(prefabName);
        //Debug.Log("Prefab: " + prefabName);
        sprite = prefab.GetComponent<SpriteRenderer>().sprite;

        //Debug.Log("Sprite: " + sprite.name);
    }

}
