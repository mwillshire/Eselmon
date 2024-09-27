using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public enum eMode { idle, combat };

    [Header("Inscribed")]
    public int speed;
    public float attackAnimDuration;

    [Header("Dynamic")]
    public int patrolIndex = 0;
    //public bool goIdle = false;
    public eMode mode = eMode.idle;
    public List<GameObject> patrolPoints = new List<GameObject>();
    public bool isMoving = false;

    [Header("FOV Information")]
    public float radius;
    public float allyCallRadius;
    [Range(0, 360)]
    public float angle;
    public LayerMask targetMask;
    public LayerMask obstructionMask;
    public LayerMask allyMask;

    //Enemy free movement
    private bool playerSeen = false;
    private float timeToNextDecision = 0f;
    private float decisionInterval = 1f;
    private Vector2 bottomLeftBoundary;
    private Vector2 topRightBoundary;

    //Combat Variables
    private bool takingTurn = false;
    private GameObject playerRef;
    private bool inRange = false;
    private bool madeAnAttack = false;
    private bool decidedToMove = false; //control variable so that the FindAdjacent method and movement call is called only once.
    private bool finishedStartingTurn = false;
    private float animationTime = 0f;

    private Rigidbody2D rb;
    private Animator anim;
    private EventBoxText eBT;
    private CombatController cC;
    private EnemyStats eS;
    private Weapon weapon;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        eBT = FindFirstObjectByType<EventBoxText>();
        cC = FindFirstObjectByType<CombatController>();
        eS = GetComponent<EnemyStats>();
        weapon = GetComponentInChildren<Weapon>();

        StartCoroutine(FOVRoutine());

        timeToNextDecision = Time.time + decisionInterval;

        bottomLeftBoundary = new Vector2(transform.position.x - 1.5f, transform.position.y - 1.5f);
        topRightBoundary = new Vector2(transform.position.x + 1.5f, transform.position.y + 1.5f);

    }

    private void Start()
    {
        FindPlayer();
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

    private void FixedUpdate()
    {
        if (StaticVariables.combatMode)
        {
            mode = eMode.combat;
            if (takingTurn)
            {
                if (!finishedStartingTurn) return;

                if (isMoving)
                {
                    if (patrolIndex > 0) rb.velocity = patrolPoints[patrolIndex - 1].GetComponent<PatrolPoint>().direction * speed;
                    else rb.velocity = patrolPoints[patrolIndex].GetComponent<PatrolPoint>().direction * speed;
                }
                else if (!inRange && eS.remainingSpeed > 0)
                {
                    MoveInRange();
                }
                else if (inRange && !madeAnAttack)
                {
                    animationTime = Time.time + attackAnimDuration;
                    madeAnAttack = true;
                    Attack();
                }
                else if (((eS.remainingSpeed == 0 && !inRange) || madeAnAttack) && Time.time >= animationTime)
                {
                    PassTurn();
                    animationTime = 0f;
                }

            }
            else
            {
                rb.velocity = Vector2.zero;
                anim.Play("Enemy_OrcIdle");
            }
        }
        else
        {
            mode = eMode.idle;
        }

        if (animationTime != 0f) return;

        if (rb.velocity.x == 0 && rb.velocity.y == 0) anim.Play("Enemy_OrcIdle");
        else anim.Play("Enemy_OrcWalk");

        if (rb.velocity.x < 0) transform.rotation = Quaternion.Euler(0, 180, 0);
        else transform.rotation = Quaternion.Euler(0, 0, 0);

        if (mode == eMode.idle) ChooseWalkDirection();
    }

    private void ChooseWalkDirection()
    {
        if (timeToNextDecision > Time.time) return;
        timeToNextDecision = Time.time + decisionInterval;

        bool canTravelLeft = true;
        bool canTravelRight = true;
        bool canTravelUp = true;
        bool canTravelDown = true;

        if (transform.position.x <= bottomLeftBoundary.x) canTravelDown = false;
        if (transform.position.x >= topRightBoundary.x) canTravelRight = false;
        if (transform.position.y <= bottomLeftBoundary.y) canTravelDown = false;
        if (transform.position.y >= topRightBoundary.y) canTravelUp = false;

        rb.velocity = Vector3.zero;
        int random = Random.Range(0, 4);
        //Debug.Log("The Value for the random number is: " + random);
        //Debug.Log("Left: " + canTravelLeft + " Up: " + canTravelUp + " Right: " + canTravelRight + " Down: " + canTravelDown);
        switch (random)
        { 
            case 0:
                if (!canTravelRight) return;
                rb.velocity = new Vector3(1, 0, 0) * speed;
                break;
            case 1:
                if (!canTravelDown) return;
                rb.velocity = new Vector3(0, -1, 0) * speed;
                break;
            case 2:
                if (!canTravelLeft) return;
                rb.velocity = new Vector3(-1, 0, 0) * speed;
                break;
            case 3:
                if (!canTravelUp) return;
                rb.velocity = new Vector3(0, 1, 0) * speed;
                break;
        }

    }

    private void CheckSurrounding() //Uses RayCasting to see if player is within circle
    {
        if (StaticVariables.combatMode) return;

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
                    StaticVariables.combatMode = true;
                    playerSeen = true;
                    cC.AddToSorter(gameObject);
                    eBT.AddText("You have entered combat!");
                    CallAllies();
                }
                else playerSeen = false;
            }
        }
    }

    private void CallAllies()
    {
        //Basically the above raycasting method but searching for any enemies in the radius. Will need to for loop everything inside the first if statement to catch each enemy ally.
        //If they are in range and not blocked, will add them to the initiative, or will turn a bool variable on their controller to true and that will add them to the initiative.
        Collider2D[] rangeChecks = Physics2D.OverlapCircleAll(transform.position, allyCallRadius, allyMask);

        if (rangeChecks.Length != 0)
        {
            //Debug.Log("RangeChecks larger than 0");
            for (int i = 0; i < rangeChecks.Length; i++)
            {
                Transform target = rangeChecks[i].transform;
                Vector2 directionToTarget = (target.position - transform.position).normalized;

                if (Vector2.Angle(transform.up, directionToTarget) < angle) // < angle / 2
                {
                    float distanceToTarget = Vector2.Distance(transform.position, target.position);

                    RaycastHit hit;
                    if (!Physics.Raycast(transform.position, directionToTarget, out hit, distanceToTarget, obstructionMask))
                    {
                        cC.AddToSorter(hit.collider.gameObject);

                    }
                    else playerSeen = false;
                }
            }
        }
    }

    public void TakeTurn()
    {
        //FindPlayer();
        eS.remainingSpeed = eS.speed;
        //inRange = UtilityMethods.isAdjacent(transform.position, playerRef.transform.position, StaticVariables.blockInterval);
        madeAnAttack = false;
        inRange = false;
        //Debug.Log("Enemy: " + gameObject.name + " is taking their turn");
        takingTurn = true;
        finishedStartingTurn = true;
    }

    private void PassTurn()
    {
        if (!takingTurn) return;
        takingTurn = false;
        cC.IterateQueue();
        //Debug.Log("Enemy: " + gameObject.name + " passed their turn");
        finishedStartingTurn = false;
    }

    private void FindPlayer()
    {
        playerRef = null;

        playerRef = FindFirstObjectByType<PlayerController>().gameObject;

        //Debug.Log("Player reference not found");
    }

    //This method is only currently usable for melee weapons.
    //Should eventually take into account the range on the weapon and move until they are in range of the player.
    private void MoveInRange()
    {
        if (playerRef == null) return;

        if (!isMoving && !decidedToMove)
        {
            //inRange = false;
            decidedToMove = true;
            //Debug.Log("Triggered !isMoving in MoveInRange()");
            Vector3 pos = UtilityMethods.FindAdjacentPosition(transform.position, playerRef.transform.position, StaticVariables.blockInterval);
            //Debug.Log("The Position from FindAdjacentPosition is: " + pos.x + ", " + pos.y);
            cC.MoveCharacter(this.gameObject, pos);
        }

    }

    //Contains any actions the enemy will take to attack
    //Will be expanded on as needed
    private void Attack()
    {
        cC.MakeAnAttack(this.gameObject, playerRef.transform.position);

        int randomNum = Random.Range(1, 3);
        string animation = "Enemy_OrcAttack0" + randomNum;
        //Debug.Log("Playing Animation: " + animation);

        if (playerRef.transform.position - transform.position == Vector3.left * StaticVariables.blockInterval) transform.rotation = Quaternion.Euler(0, 180, 0);
        else transform.rotation = Quaternion.Euler(0, 0, 0);

        anim.Play(animation);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PatrolPoint")
        {
            //Debug.Log("Triggered Patrolpoint at: " + collision.transform.position.x + ", " + collision.transform.position.y);
            if (collision.GetComponent<PatrolPoint>().index == patrolIndex)
            {
                if (patrolIndex != 0) eS.remainingSpeed -= 5;
                patrolIndex++;
                inRange = UtilityMethods.isAdjacent(transform.position, playerRef.transform.position, StaticVariables.blockInterval);
                //Debug.Log("Iterated to Patrol Index: " + patrolIndex + ", with speed remaining: " + pS.remainingSpeed);

                if (patrolIndex >= patrolPoints.Count || eS.remainingSpeed <= 0)
                {
                    rb.velocity = Vector3.zero;
                    patrolIndex = 0;
                    foreach (GameObject p in patrolPoints)
                    {
                        Destroy(p);
                    }
                    patrolPoints.Clear();
                    isMoving = false;
                    decidedToMove = false;
                    transform.position = new Vector3(UtilityMethods.RoundToNumber(transform.position.x, StaticVariables.blockInterval), UtilityMethods.RoundToNumber(transform.position.y, StaticVariables.blockInterval, StaticVariables.offset), 0);
                }

            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Player") && isMoving)
        {
            rb.velocity = Vector3.zero;
            patrolIndex = 0;
            foreach (GameObject p in patrolPoints)
            {
                Destroy(p);
            }
            patrolPoints.Clear();
            isMoving = false;
            decidedToMove = false;
            transform.position = new Vector3(UtilityMethods.RoundToNumber(transform.position.x, StaticVariables.blockInterval), UtilityMethods.RoundToNumber(transform.position.y, StaticVariables.blockInterval, StaticVariables.offset), 0);
        }
    }
}
