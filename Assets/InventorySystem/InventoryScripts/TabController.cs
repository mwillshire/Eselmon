using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class TabController : MonoBehaviour
{
    [Header("Inscribed")]
    public GameObject[] tabs;

    public void ChangeTab(string tabName)
    {
        foreach (GameObject go in tabs)
        {
            if (go.name == tabName) go.SetActive(true);
            else go.SetActive(false);
        }
    }
}
