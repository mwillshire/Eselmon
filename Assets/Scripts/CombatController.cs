using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CombatController : MonoBehaviour
{
    public enum eCombatAction { none, move, weaponAttack };

    [Header("Inscribed")]
    public LayerMask obstructionMask;
    public float blockInterval;
    public GameObject patrolPoint;

    [Header("Dynamic")]
    public List<GameObject> queue { get; private set; } //contains a reference to each player and npc in initiative
    public List<GameObject> beforeSort { get; private set; }
    public eCombatAction combatAction = eCombatAction.none;

    private bool hasSorted = false;
    private float timeToBeginSort;
    private float timeIntervalBeforeSort = 2f; //This will probably be a lot smaller but is at this value for testing
    private bool combatStarted = false;
    
    private CombatQueueBox cQB;
    private EventBoxText eBT;
    private EventHandler eH;
    private AudioHandler aH;

    private void Awake()
    {
        queue = new List<GameObject>();
        beforeSort = new List<GameObject>();

        cQB = FindFirstObjectByType<CombatQueueBox>();
        //Debug.Log("The CombatQueBox Object is: " +  cQB);
        eBT = FindFirstObjectByType<EventBoxText>();
        eH = FindFirstObjectByType<EventHandler>();
        aH = FindFirstObjectByType<AudioHandler>();
    }

    private void FixedUpdate()
    {
        if (!combatStarted && StaticVariables.combatMode)
        {
            combatStarted = true;
            timeToBeginSort = Time.time + timeIntervalBeforeSort;
        }

        if (combatStarted && timeToBeginSort < Time.time && !hasSorted)
        {
            SortList();
            hasSorted = true;
        }

        if (!StaticVariables.combatMode) return;
        //This if statement turns the combat mode off for the entire game
        if (queue.Count == 1 && queue[0].tag == "Player")
        {
            StaticVariables.combatMode = false;
            combatStarted = false;
            hasSorted = false;
            timeToBeginSort = 0;
            while (queue.Count > 0)
            {
                RemoveFromQueue(queue[0]);
            }
        }
    }

    //Initiative Methods, controls the queue of initiative
    //----------------------------------------------------------------------------
    public void AddToSorter(GameObject gameObject)
    {
        beforeSort.Add(gameObject);
    }

    //This method may have a nested loop, but the initiative should never go over 10 npcs and 
    //definitely not over 20 so it should not have as much of an impact.
    private void SortList()
    {
        List<int> initiativeScores = new List<int>();

        foreach (GameObject obj in beforeSort)
        {
            //try
            //{
            //    if (obj.name != "Player")
            //    {
            //        initiativeScores.Add(Random.Range(1, 21) + obj.GetComponent<EnemyStats>().initiativeBonus);
            //    }
            //    else
            //    {
            //        initiativeScores.Add(Random.Range(1, 21) + obj.GetComponent<PlayerStats>().initiativeBonus);
            //    }
            //}
            //catch { continue; }
            try { initiativeScores.Add(Random.Range(0, 21) + obj.GetComponent<PlayerStats>().initiativeBonus); }
            catch { }

            try { initiativeScores.Add(Random.Range(0, 21) + obj.GetComponent<EnemyStats>().initiativeBonus); }
            catch { }
        }

        //Debug.Log("Number of Combaters: " + beforeSort[0].name + " , " + beforeSort[1]);
        //Debug.Log("Number in Initiative: " + initiativeScores[0] + " , " + initiativeScores[1]);

        GameObject temp;
        int tempInt;
        for (int i = 0; i < beforeSort.Count - 1; i++)
        {
            for ( int j = i + 1; j < beforeSort.Count; j++)
            {
                if (initiativeScores[j] > initiativeScores[i])
                {
                    tempInt = initiativeScores[i];
                    temp = beforeSort[i];

                    initiativeScores[i] = initiativeScores[j];
                    beforeSort[i] = beforeSort[j];

                    initiativeScores[j] = tempInt;
                    beforeSort[j] = temp;
                }
            }
        }

        //Debug.Log("Number of Combaters: " + beforeSort[0].name + " , " + beforeSort[1]);
        //Debug.Log("Number in Initiative: " + initiativeScores[0] + " , " + initiativeScores[1]);

        CreateQueue();
    }

    private void CreateQueue ()
    {
        foreach (GameObject gameObject in beforeSort) 
        {
            queue.Add(gameObject);
            cQB.AddToQueue(gameObject);
        }

        beforeSort.Clear();
        MoveCharactersToTiles();
        
        if (queue[0].tag == "Player") queue[0].GetComponent<PlayerController>().TakeTurn();
        else if (queue[0].tag == "Enemy") queue[0].GetComponent<EnemyController>().TakeTurn();
    }

    public void IterateQueue()
    {
        queue.Add(queue[0]);
        queue.Remove(queue[0]);

        cQB.IterateQueue();

        if (queue[0].tag == "Player") queue[0].GetComponent<PlayerController>().TakeTurn();
        else if (queue[0].tag == "Enemy") queue[0].GetComponent<EnemyController>().TakeTurn();
    }

    public void RemoveFromQueue(GameObject gameObject)
    {
        queue.Remove(gameObject);

        cQB.RemoveFromQueue(gameObject);
    }

    private void MoveCharactersToTiles()
    {
        foreach(GameObject character in queue)
        {
            character.transform.position = new Vector3(UtilityMethods.RoundToNumber(character.transform.position.x, blockInterval), UtilityMethods.RoundToNumber(character.transform.position.y, blockInterval, StaticVariables.offset), 0);
        }
    }


    //Combat Mode Methods for Interaction between Player and NPCs
    //---------------------------------------------------------------------------

    //Handles whether this attack can be made or not then calls DealDamage() to do the damage
    //Calls FindCharacter and if there is not a character at that location, automatically misses
    //Else, calls roll to hit then based on that calls DealDamage()
    public void MakeAnAttack(GameObject provoker, Vector3 otherLocation)
    {
        GameObject target = FindCharacter(otherLocation);
        //Debug.Log("Attacking target: " + target.name + " at location: " + otherLocation.x + ", " + otherLocation.y);

        if (target == null)
        {
            eBT.AddText("Attacker, " + provoker.name + ", had no target");
            return;
        }
        if (!UtilityMethods.isAdjacent(provoker.transform.position, otherLocation, StaticVariables.blockInterval))
        {
            Debug.Log("Attacker, " + provoker.name + ", is out of range");
            return;
        }

        bool hit = RollToHit(provoker, target);
        if (provoker.tag == "Player")
        {
            PlayerController temp = provoker.GetComponent<PlayerController>();
            if (provoker.transform.position - otherLocation == Vector3.right * StaticVariables.blockInterval) temp.facing = 2;
            else if (provoker.transform.position - otherLocation == Vector3.down * StaticVariables.blockInterval) temp.facing = 1;
            else if (provoker.transform.position - otherLocation == Vector3.left * StaticVariables.blockInterval) temp.facing = 0;
            else if (provoker.transform.position - otherLocation == Vector3.up * StaticVariables.blockInterval) temp.facing = 3;

            temp.timeToPassAnimation = Time.time + temp.attackAnimDuration;
        }

        if (hit)
        {
            DealDamage(provoker.GetComponentInChildren<Weapon>().RollDamage(), target);
        }
        else
        {
            eBT.AddText("Attacker, " + provoker.name + ", missed");
        }
    }

    //Takes a gameobject and deals the damage to that object. Can only be called from other methods in this class
    //Does not currently control whether the enemy or player should die
    private void DealDamage(int damage, GameObject target)
    {
        aH.Play("TakeDamage");
        eBT.AddText("Attacker " + queue[0].name + " dealt " + damage + " damage to " + target.name);
        if (target.tag == "Player")
        {
            PlayerStats playerStats = target.GetComponent<PlayerStats>();
            playerStats.health -= damage;
            if (playerStats.health <= 0)
            {
                eH.TransDeathScreen();
            }
        }
        else
        {
            EnemyStats eS = target.GetComponent<EnemyStats>();
            eS.health -= damage;
            if (eS.health <= 0)
            {
                RemoveFromQueue(target);
                Destroy(target);
            }
        }
    }

    //Finds the character at a given location
    public GameObject FindCharacter(Vector3 location)
    {
        foreach (GameObject obj in queue)
        {
            if (location == obj.transform.position) return obj;
        }

        return null;
    }

    //Both attack and defender must have a weapon and armor,
    //Will have backups in case the player is not wearing armor nor a weapon equipped
    //Will access the weapons and armor on the attacker and defender along with the stats scripts
    private bool RollToHit(GameObject attacker, GameObject defender)
    {
        int hitNum = attacker.GetComponentInChildren<Weapon>().RollToHit();
        int aC = defender.GetComponentInChildren<Armor>().ArmorClass();

        if (hitNum >= aC) return true;
        else return false;
    }

    //When called, tries to move the character to the given location
    //This method is typically called by the enemyController or the CombatGridManager
    //When the player selects a tile the tile will have a OnMouseClick() that will call this method
    public void MoveCharacter(GameObject character, Vector3 targetPos)
    {
        List<Vector2> position = new List<Vector2>();
        position = UtilityMethods.PathToPosition(character.transform.position, targetPos, blockInterval, obstructionMask);

        foreach (Vector2 pos in position)
        {
            GameObject obj = Instantiate(patrolPoint, pos, Quaternion.identity);

            if (character.tag == "Player")
            {
                PlayerController pC = character.GetComponent<PlayerController>();
                Vector3 direction = Vector3.zero;

                pC.patrolPoints.Add(obj);
                pC.patrolPoints[pC.patrolPoints.Count - 1].GetComponent<PatrolPoint>().index = pC.patrolPoints.Count - 1;


                if (pC.patrolPoints.Count > 1)
                {
                    Vector3 temp1 = pC.patrolPoints[pC.patrolPoints.Count - 1].transform.position;
                    Vector3 temp2 = pC.patrolPoints[pC.patrolPoints.Count - 2].transform.position;

                    if (temp1.x != temp2.x) direction.x = (temp1.x - temp2.x) / Mathf.Abs(temp1.x - temp2.x);
                    if (temp1.y != temp2.y) direction.y = (temp1.y - temp2.y) / Mathf.Abs(temp1.y - temp2.y);

                    pC.patrolPoints[pC.patrolPoints.Count - 2].GetComponent<PatrolPoint>().direction = direction;
                }

                pC.isMoving = true;
                pC.patrolIndex = 0;
            }
            else if (character.tag == "Enemy")
            {
                EnemyController eC = character.GetComponent<EnemyController>();
                Vector3 direction = Vector3.zero;

                eC.patrolPoints.Add(obj);
                eC.patrolPoints[eC.patrolPoints.Count - 1].GetComponent<PatrolPoint>().index = eC.patrolPoints.Count - 1;

                if (eC.patrolPoints.Count > 1)
                {
                    Vector3 temp1 = eC.patrolPoints[eC.patrolPoints.Count - 1].transform.position;
                    Vector3 temp2 = eC.patrolPoints[eC.patrolPoints.Count - 2].transform.position;

                    if (temp1.x != temp2.x) direction.x = (temp1.x - temp2.x) / Mathf.Abs(temp1.x - temp2.x);
                    if (temp1.y != temp2.y) direction.y = (temp1.y - temp2.y) / Mathf.Abs(temp1.y - temp2.y);

                    eC.patrolPoints[eC.patrolPoints.Count - 2].GetComponent<PatrolPoint>().direction = direction;
                }

                eC.isMoving = true;
                eC.patrolIndex = 0; 
            }
            else
            {
                Debug.Log("Character received in MoveCharacter() is not tagged as an Enemy or Player");
                Destroy(obj);
                return;
            }

            if (character.tag == "Player")
            {
                PlayerController pC = character.GetComponent<PlayerController>();
                if (pC.patrolPoints.Count > 1) pC.patrolPoints[pC.patrolPoints.Count - 1].GetComponent<PatrolPoint>().direction = pC.patrolPoints[pC.patrolPoints.Count - 2].GetComponent<PatrolPoint>().direction;
            }
            else if (character.tag == "Enemy")
            {
                EnemyController eC = character.GetComponent<EnemyController>();
                if (eC.patrolPoints.Count > 1) eC.patrolPoints[eC.patrolPoints.Count - 1].GetComponent<PatrolPoint>().direction = eC.patrolPoints[eC.patrolPoints.Count - 2].GetComponent<PatrolPoint>().direction;
            }
        }
    }

    //Other Methods Involved
    //------------------------------------------------------------------------------
    //Used by the player when they click on a tile on the grid. If it is not the player's turn, then ignores the click.
    public void GridMouseClick(Vector3 location)
    {
        if (queue[0].tag != "Player") return;

        switch (combatAction)
        {
            case eCombatAction.move:
                if (queue[0].GetComponent<PlayerController>().isMoving) return;
                MoveCharacter(queue[0], location);
                //Debug.Log("Attempted to move the Player");
                break;
            case eCombatAction.weaponAttack:
                MakeAnAttack(queue[0], location);
                break;
        }
        //Debug.Log("Tile at location: " + location.x + ", " + location.y + " has been clicked");
    }
}
