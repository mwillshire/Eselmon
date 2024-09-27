using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NewGameScene : MonoBehaviour
{
    [Header("Inscribed")]
    public GameObject[] descriptions;

    [Header("Dynamic")]
    public string selectedClassName = null;

    public void ChangeText()
    {
        GetComponent<TextMeshProUGUI>().text = selectedClassName;
    }

    public void ChangeDescription()
    {
       foreach (GameObject go in descriptions)
       {
            if (go.name.Substring(0, 1) == selectedClassName.Substring(0, 1)) go.SetActive(true);
            else go.SetActive(false);
       }
    }

}
