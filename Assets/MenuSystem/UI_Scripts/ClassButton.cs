using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class ClassButton : MonoBehaviour
{
    [Header("Inscribed")]
    public GameObject displayClass;
    public GameObject buttonTextObject;

    private NewGameScene nGS;
    private TextMeshProUGUI tMP;

    private void Awake()
    {
        nGS = displayClass.GetComponent<NewGameScene>();
        tMP = buttonTextObject.GetComponent<TextMeshProUGUI>();
    }
    public void OnClick()
    {
        nGS.selectedClassName = tMP.text;
        nGS.ChangeText();
        nGS.ChangeDescription();
    }

   
}
