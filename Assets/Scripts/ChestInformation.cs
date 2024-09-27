//Method using RayCasting to find when a player is within a specific distance
//If a player is in a certain distance turns a bool to true.
//Then in the update if the bool is true and the key "f" is clicked
//Adds to the player's inventory the items in the string[] itemsContained.
//Maybe later make it a UI where the player chooses what goes in their inventory.
//For now, it is automatic.

using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class ChestInformation : MonoBehaviour
{
    [Header("Inscribed")]
    public string[] itemsContained;
    public int[] itemAmounts;

    [Header("FOV Information")]
    public float radius;
    [Range(0, 360)]
    public float angle;
    public LayerMask targetMask;
    public LayerMask obstructionMask;

    private bool playerSeen = false;

    private InventoryController iC;
    private EventBoxText eBT;
    private AudioHandler aH;

    private void Awake()
    {
        iC = FindFirstObjectByType<InventoryController>();
        eBT = FindFirstObjectByType<EventBoxText>();
        aH = FindFirstObjectByType<AudioHandler>();

        StartCoroutine(FOVRoutine());
    }

    private void FixedUpdate()
    {
        if (StaticVariables.combatMode) return;
        if (!playerSeen) return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            for (int i = 0; i < itemsContained.Length; i++)
            {
                if ( itemAmounts.Length <= i )  iC.ChangeItemAmount(itemsContained[i], 1);
                else iC.ChangeItemAmount(itemsContained[i], itemAmounts[i]);

                //For now will display sprite name, will probably need to add a column to the csv for display name
                Sprite temp = iC.catalog.FindSprite(itemsContained[i]);
                eBT.AddText("Acquired " + temp.name);
            }

            aH.Play("LootChest");
            Destroy(gameObject);
        }
    }

    private IEnumerator FOVRoutine()
    {
        float delay = 0.2f;
        WaitForSeconds wait = new WaitForSeconds(delay);

        while (true)
        {
            yield return wait;
            CheckSurrounding();
        }
    }

    private void CheckSurrounding() //Uses RayCasting to see if player is within circle
    {
        
        Collider2D[] rangeChecks = Physics2D.OverlapCircleAll(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            //Debug.Log("RangeChecks larger than 0");
            Transform target = rangeChecks[0].transform;
            Vector2 directionToTarget = (target.position - transform.position).normalized;

            if (Vector2.Angle(transform.up, directionToTarget) < angle) // < angle / 2
            {
                float distanceToTarget = Vector2.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    playerSeen = true;
                    //Debug.Log("PlayerSeen");
                }
                else
                {
                    playerSeen = false;
                }
            }
            else
            {
                playerSeen = false;
            }
        }
    }

}
