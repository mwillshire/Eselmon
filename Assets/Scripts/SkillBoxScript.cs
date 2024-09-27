//This script handles displaying all of the possible skills that can be used by the player
//Will also hold which skill is currently selected including move action, and other non weapon actions
//Other scripts will reference which action/skill is selected at the moment.

using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;

public class SkillBoxScript : MonoBehaviour
{
    [Header("Inscribed")]
    public GameObject skillBoxSlotPrefab;
    public Vector3 firstSlotPos;
    public float xDiff;
    public float yDiff;

    [Header("Dynamic")]
    public List<GameObject> skillBoxSlots = new List<GameObject>();

    private bool hasDisplayed = false;

    private GameObject playerRef;
    private Action[] skills;

    private void Start()
    {
        playerRef = FindFirstObjectByType<PlayerController>().gameObject;
        skillBoxSlots = new List<GameObject>();
    }

    private void FixedUpdate()
    {
        if (!hasDisplayed && StaticVariables.combatMode)
        {
            GetAllSkills();
            DisplaySkills();
            //Debug.Log("Called GetAllSkills() and DisplaySkills()");
            hasDisplayed = true;
        }
    }

    //Retrieves all possible skills the player could use. This method will be added to
    //as more sources of skills are created. For example spells, armor abilities, 
    //and the basic actions. These will all be stored seperately.
    private void GetAllSkills()
    {
        skills = playerRef.GetComponentsInChildren<Action>();
        //Debug.Log("Skills in List<Action> length: " + skills.Length);
    }

    //Note: When displaying all skills, the attackSkills list must go first unless I rewrite the code a little bit.
    private void DisplaySkills()
    {
        if (hasDisplayed)
        {
            foreach (GameObject obj in skillBoxSlots)
            {
                Destroy(obj);
                skillBoxSlots.Remove(obj);
            }
            hasDisplayed = false;
        }

        //Instantiate first skill slot here
        if (skills.Length == 0) return;

        skillBoxSlots.Add(Instantiate(skillBoxSlotPrefab, firstSlotPos, Quaternion.identity));
        skillBoxSlots[0].transform.SetParent(transform, false);
        skillBoxSlots[0].GetComponentInChildren<TextMeshProUGUI>().text = skills[0].displayName;
        skillBoxSlots[0].GetComponent<SkillBoxSlot>().component = skills[0];

        if (skills.Length > 1)
        {
            float xPos = firstSlotPos.x;
            float yPos = firstSlotPos.y - yDiff;

            for (int i = 1; i < skills.Length; i++)
            {
                //Instantiate the slot at the current pos
                //Set the parent of the new slot to the canvas
                //Set the text of the new slot to the attack component's name

                skillBoxSlots.Add(Instantiate(skillBoxSlotPrefab, new Vector3(xPos, yPos, 0), Quaternion.identity));
                skillBoxSlots[i].transform.SetParent(transform, false);
                skillBoxSlots[i].GetComponentInChildren<TextMeshProUGUI>().text = skills[i].displayName;
                skillBoxSlots[i].GetComponent<SkillBoxSlot>().component = skills[i];

                if (i % 2 == 1)
                {
                    yPos = firstSlotPos.y;
                    xPos += xDiff;
                }
                else
                {
                    yPos -= yDiff;
                }
            }
        }

        hasDisplayed = true;
    }

}
