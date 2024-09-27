//This class will read, create, and store the items from the spreadsheet.

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;

public class Catalog : MonoBehaviour
{

    public List<Item> items { get; private set; }

    private bool firstPassDone = false; //For the csv reader

    private void Awake()
    {
        items = new List<Item>();
    }
    
    private void Start()
    {
        ReadItemCSVFile();
    }

    //Will eventually need to tell which list of items to check.
    public bool UpdateAmount(string itemID, int amount)
    {
        //Updates the amount of an item in a list

        foreach (var item in items)
        {
            if (itemID.Equals(item.itemID))
            {
                item.amount += amount;
                if (item.amount <= 0)
                {
                    item.amount = 0;
                    return true;
                }

                //Debug.Log("Item: " + itemID + " is at " + item.amount);
            }
        }

        return false;
    }

    public string FindAmountFromSprite(Sprite sprite)
    {

        foreach (var item in items)
        {
            if (item.sprite == sprite) return item.amount.ToString();
        }

        return null;
    }

    public string FindIDFromSprite(Sprite sprite)
    {
        //Debug.Log("FindIDFromSprite Method- sprite given: " + sprite.name);
        foreach (var item in items)
        {
            if (item.sprite == sprite) return item.itemID;
        }

        return null;
    }

    public Sprite FindSprite(string itemID)
    {
        foreach (var item in items)
        {
            if (item.itemID.Equals(itemID))
                return item.sprite;
        }

        return null;
    }

    public GameObject FindGameObject(string itemID)
    {
        foreach(var item in items)
        {
            if (item.itemID.Equals(itemID)) return item.prefab;
        }

        return null;
    }

    private void ReadItemCSVFile()
    {
        StreamReader strReader = new StreamReader("Assets/Resources/CSV/ItemID.csv");
        bool endOfFile = false;
        firstPassDone = false;

        while ( !endOfFile )
        {
            string dataString = strReader.ReadLine();
            if ( dataString == null )
            {
                endOfFile = true;
                break;
            }

            var dataValues = dataString.Split(",");

            if (firstPassDone)
            {
                items.Add(new Item(dataValues[0], 0, Convert.ToInt32(dataValues[2]), dataValues[3]));

            }

            firstPassDone = true;
        }

        strReader.Close();
    }

}
