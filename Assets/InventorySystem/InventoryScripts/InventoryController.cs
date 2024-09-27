//Probably need to rewrite this class to fit with catalog and save a lot of space.
using System.Collections;
using System.Collections.Generic;
 
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [Header("Inscribed")]
    public GameObject baseArmor;
    public GameObject unarmedWeapon;

    [Header("Dynamic")]
    public List<string> invItems {  get; private set; }
    public string[] equipItems { get; private set; }

    private bool isLogged = false;
    private bool isEmpty = false;

    private GameObject[] equippedPrefabs = new GameObject[5];

    public Catalog catalog { get; private set; }

    private AudioHandler aH;

    private void Awake()
    {
        catalog = GetComponent<Catalog>();
        aH = FindFirstObjectByType<AudioHandler>();
    }

    private void Start()
    {
        invItems = new List<string>();
        equipItems = new string[5]; //0 is Weapon, 1, is Armour, 2-4 are Assessories

        equippedPrefabs[0] = Instantiate(unarmedWeapon);
        equippedPrefabs[0].transform.SetParent(FindFirstObjectByType<PlayerStats>().gameObject.transform, false);

        equippedPrefabs[1] = Instantiate(baseArmor);
        equippedPrefabs[1].transform.SetParent(FindFirstObjectByType<PlayerStats>().gameObject.transform, false);

        //ChangeItemAmount("W001", 1);
        //ChangeItemAmount("Q001", 1);
    }

    //This method will update the amount of an item if it is in the inventory
    //If the item is not in the inventory it will add the item then continue to update the amount in the catalog
    //If the item amount reaches 0, the catalog method ChangeAmount will return true and the item will be removed.
    public void ChangeItemAmount(string itemID, int amount)
    {
        isLogged = false;
        isEmpty = false;

        foreach (string s in invItems)
        {
            if (s.Equals(itemID))
            {
                isEmpty = catalog.UpdateAmount(itemID, amount);
                isLogged = true;
                break;
            }
        }

        if (!isLogged && amount > 0)
        {
            invItems.Add(itemID);
            isEmpty = catalog.UpdateAmount(itemID, amount);
        }

        if (isEmpty)
        {
            invItems.Remove(itemID);
        }
    }

    //Neither has been set up for the assessories
    public void EquipItem(string itemID)
    {
        if (itemID.Substring(0, 1) == "W")
        {
            if (equipItems[0] == null)
            {
                Destroy(equippedPrefabs[0]);
                equipItems[0] = itemID;
                ChangeItemAmount(itemID, -1);

                GameObject prefab = catalog.FindGameObject(itemID);
                equippedPrefabs[0] = Instantiate(prefab);
                equippedPrefabs[0].transform.SetParent(FindFirstObjectByType<PlayerStats>().gameObject.transform, false);
                equippedPrefabs[0].GetComponent<SpriteRenderer>().enabled = false;

                aH.Play("EquipItem");
            }
        }
        else if (itemID.Substring(0, 1) == "Q")
        {
            if (equipItems[1] == null)
            {
                Destroy(equippedPrefabs[1]);
                equipItems[1] = itemID;
                ChangeItemAmount(itemID, -1);

                GameObject prefab = catalog.FindGameObject(itemID);
                equippedPrefabs[1] = Instantiate(prefab);
                equippedPrefabs[1].transform.SetParent(FindFirstObjectByType<PlayerStats>().gameObject.transform, false);
                equippedPrefabs[1].GetComponent<SpriteRenderer>().enabled = false;

                aH.Play("EquipItem");
            }
        }

        //Debug.Log("Attempted: " + itemID);
    }

    public void UnEquipItem(string itemID)
    {
        if (itemID.Substring(0, 1).Equals("W"))
        {
            equipItems[0] = null;
            ChangeItemAmount(itemID, 1);

            Destroy(equippedPrefabs[0]);
            equippedPrefabs[0] = null;

            equippedPrefabs[0] = Instantiate(unarmedWeapon);
            equippedPrefabs[0].transform.SetParent(FindFirstObjectByType<PlayerStats>().gameObject.transform, false);

            aH.Play("UnequipItem");
        }
        else if (itemID.Substring(0, 1).Equals("Q"))
        {
            equipItems[1] = null;
            ChangeItemAmount(itemID, 1);

            Destroy(equippedPrefabs[1]);
            equippedPrefabs[1]= null;

            equippedPrefabs[1] = Instantiate(baseArmor);
            equippedPrefabs[1].transform.SetParent(FindFirstObjectByType<PlayerStats>().gameObject.transform, false);

            aH.Play("UnequipItem");
        }
    }

}
