using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class InventorySlotButton : MonoBehaviour
{
    [Header("Inscribed")]
    public GameObject parentObject;

    [Header("Do Not Use")]
    public string itemID = null;

    private InventoryBoxDisplay iBD;

    private void Start()
    {
        iBD = FindFirstObjectByType<InventoryBoxDisplay>();
    }

    public void TriggerEquipButton()
    {
        Sprite sprite = parentObject.GetComponent<Image>().sprite;
        if (sprite == null) return;

        itemID = iBD.iC.catalog.FindIDFromSprite(sprite);
        //Debug.Log("ItemID trying to Equip: " + itemID);
        iBD.iC.EquipItem(itemID);
    }

    public void TriggerUnEquipButton()
    {
        Sprite sprite = parentObject.GetComponent<Image>().sprite;
        if (sprite == null) return;

        itemID = iBD.iC.catalog.FindIDFromSprite(sprite);
        iBD.iC.UnEquipItem(itemID);
    }
}
