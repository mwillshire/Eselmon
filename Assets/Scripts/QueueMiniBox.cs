using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;

public class QueueMiniBox : MonoBehaviour
{
    [Header("Inscribed from Code")]
    public GameObject objectRef;

    private bool firstPassSetValues = false; //Used in FixedUpdate to set the value of the text in the TMP after objectRef is filled

    private void FixedUpdate()
    {
        if ( !firstPassSetValues && objectRef != null )
        {
            string[] temp = null;
            try
            {
                temp = objectRef.name.Split("(");
            }
            catch { }
            gameObject.GetComponentInChildren<TextMeshProUGUI>().text = temp[0];
            firstPassSetValues = true;
        }
    }
}
