using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.UI;


public class InventoryBoxDisplay : MonoBehaviour
{
    [Header("Inscribed")]
    public GameObject[] inventoryBoxes;
    public GameObject[] equippedBoxes;

    [Header("Dynamic")]
    public bool updateBox = false;

    private List<Sprite> itemPics = new List<Sprite>();
 

    public InventoryController iC {  get; private set; }

    private void Awake()
    {
        iC = FindFirstObjectByType<InventoryController>();
    }

    private void Update()
    {

        if (!gameObject.activeInHierarchy) return;

        UpdateInventory();
        UpdateEquipped();
        
    }

    public void UpdateInventory()
    {
        itemPics.Clear();

        foreach (string itemID in iC.invItems)
        {
            itemPics.Add(iC.catalog.FindSprite(itemID));
        }

        int extra = 0;
        for (int i = 0; i < itemPics.Count; i++)
        {
            Image temp = inventoryBoxes[i].GetComponent<Image>();
            temp.sprite = itemPics[i];
            temp.color = new Color(255, 255, 255, 255);

            inventoryBoxes[i].GetComponentInChildren<TextMeshProUGUI>().text = iC.catalog.FindAmountFromSprite(temp.sprite);
            inventoryBoxes[i].GetComponentInChildren<TextMeshProUGUI>().color = new Color(0, 0, 0, 255);
            extra = i;

            //Debug.Log("Sprite Name in Sprite Renderer in inventory box " + i + ": " + inventoryBoxes[i].GetComponent<Image>().sprite.name);
        }

        if (extra >= inventoryBoxes.Length) return;
        for (int i = extra + 1; i < inventoryBoxes.Length; i++)
        {
            inventoryBoxes[i].GetComponent<Image>().sprite = null;
            inventoryBoxes[i].GetComponent<Image>().color = new Color(0, 0, 0, 0);

            inventoryBoxes[i].GetComponentInChildren<TextMeshProUGUI>().color = new Color(0, 0, 0, 0);
        }

        if (itemPics.Count == 0)
        {
            inventoryBoxes[0].GetComponent<Image>().sprite = null;
            inventoryBoxes[0].GetComponent<Image>().color = new Color(0, 0, 0, 0);

            inventoryBoxes[0].GetComponentInChildren<TextMeshProUGUI>().color = new Color(0, 0, 0, 0);
        }
    }

    public void UpdateEquipped()
    {
        itemPics.Clear();

        foreach (string itemID in iC.equipItems)
        {
            if (itemID != null)
                itemPics.Add(iC.catalog.FindSprite(itemID));
            else
                itemPics.Add(null);
        }

        for (int i = 0; i < equippedBoxes.Length; i++)
        {
            if (itemPics[i] != null)
            {
                Image temp = equippedBoxes[i].GetComponent<Image>();
                temp.sprite = itemPics[i];
                temp.color = new Color(255, 255, 255, 255);
            }
            else
            {
                equippedBoxes[i].GetComponent<Image>().sprite = null;
                equippedBoxes[i].GetComponent<Image>().color = new Color(0, 0, 0, 0);
            }
        }


    }
}
