using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;

public class EventBoxText : MonoBehaviour
{
    [Header("Inscribed")]
    public List<TextMeshProUGUI> textBoxes;
    public float disappearInterval;

    [Header("Dynamic")]
    public List<string> textList;
    public int indexPoint = -1;

    private float timeToDisappear = 0.0f;
    private bool moveText = false;

    private void FixedUpdate()
    {
        if (textList.Count == 0) return;

        if (Time.time > timeToDisappear) moveText = true;
        else moveText = false;

        if (moveText)
        {
            if (textList.Count > 0) timeToDisappear = Time.time + disappearInterval;
            MoveText();
        }
    }

    public void AddText(string text)
    {
        textList.Add(text);
    }

    public void MoveText()
    {
        indexPoint++;

        if (indexPoint >= textList.Count)
        {
            textList.Clear();
            indexPoint = -1;
            textBoxes[0].text = null;
            return;
        }

        for (int i = 0; i < textBoxes.Count; i++)
        {
            try
            {
                textBoxes[i].text = textList[indexPoint + i];
            }
            catch 
            {
                textBoxes[i].text = "";
            }
        }

    }
}
