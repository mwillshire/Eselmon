//Sets up a new game for the player on click of the button.

using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;

public class StartGame : MonoBehaviour
{
    [Header("Inscribed")]
    public NewGameScene nGS;
    public GameObject nameTextBox;
    

    public void StartNewGame()
    {
        if (nameTextBox.GetComponent<TextMeshProUGUI>().text == "" || nGS.selectedClassName == null) return;

        //Take the steps to creating the new player and setting any values neccesary
    }
}
