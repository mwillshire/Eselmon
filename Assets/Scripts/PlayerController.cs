using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum eMode { idle, move, attack };

    [Header("Attack Animation Information")]
    public float attackAnimDuration;

    [Header("Dynamic")]
    public int facing = 0;
    public eMode mode = eMode.idle;
    public int dirHeld = -1;
    public List<GameObject> patrolPoints = new List<GameObject>();
    public bool isMoving = false;
    public int patrolIndex = 0;
    public Action skillSelected = null;
    public float timeToPassAnimation = 0f;

    private Vector3[] directions = new Vector3[] { new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(-1, 0, 0), new Vector3(0, -1, 0) };
    private KeyCode[] keys = new KeyCode[] {KeyCode.RightArrow, KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.DownArrow,
                                            KeyCode.D, KeyCode.W, KeyCode.A, KeyCode.S };

    private float speed;
    private bool enteredCombat = false;
    private bool takingTurn = false;

    private float timeToPlayAudio = 0.0f;
    private float audioInterval = 0.5f;

    //Turn Control
    private bool hasDecided = false;

    private Rigidbody2D rb;
    private Animator anim;
    private PlayerStats pS;
    private CombatController cC;
    private AudioHandler aH;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        pS = GetComponent<PlayerStats>();
        cC = FindFirstObjectByType<CombatController>();
        aH = FindFirstObjectByType<AudioHandler>();

        speed = pS.freeRoamSpeed;

    }

    private void FixedUpdate()
    {
        if (StaticVariables.combatMode)
        {
            //environmentCollider.enabled = false;
            if (!enteredCombat)
            {
                enteredCombat = true;
                cC.AddToSorter(gameObject);
            }
            if (takingTurn)
            {
                if (isMoving)
                {
                    Vector3 direction;
                    if (patrolIndex == 0) direction = patrolPoints[patrolIndex].GetComponent<PatrolPoint>().direction;
                    else direction = patrolPoints[patrolIndex - 1].GetComponent<PatrolPoint>().direction;

                    if (direction == new Vector3(1, 0, 0)) facing = 0;
                    else if (direction == new Vector3(-1, 0, 0)) facing = 2;
                    else if (direction == new Vector3(0, -1, 0)) facing = 3;
                    else if (direction == new Vector3(0, 1, 0)) facing = 1;

                    rb.velocity = direction * speed;
                    ChangeDirectionFacing("Player_SideWalk", "Player_UpWalk", "Player_DownWalk");

                    if (Time.time >= timeToPlayAudio)
                    {
                        aH.PlayMultiClipSound("FootstepsGrass");
                        timeToPlayAudio = Time.time + audioInterval;
                    }
                }
                else if (mode == eMode.attack)
                {
                    //Play animation then once animation is complete, set hasDecided to false
                    ChangeDirectionFacing("Player_SideAttack", "Player_UpAttack", "Player_DownAttack");
                    

                    hasDecided = false;
                    mode = eMode.idle;
                }
                else if ( Time.time >= timeToPassAnimation && timeToPassAnimation != 0f)
                {
                    ChangeDirectionFacing("Player_SideIdle", "Player_UpIdle", "Player_DownIdle");
                    timeToPassAnimation = 0f;
                }
                return;
            }
            rb.velocity = Vector2.zero;
            mode = eMode.idle;
            ChangeDirectionFacing("Player_SideIdle", "Player_UpIdle", "Player_DownIdle");
            return;
        }

        if (mode == eMode.idle || mode == eMode.move)
        {
            dirHeld = -1;
            for (int i = 0; i < keys.Length; i++)
            {
                if (Input.GetKey(keys[i])) dirHeld = i % 4;
            }

            if (dirHeld == -1)
                mode = eMode.idle;
            else
            {
                facing = dirHeld;
                mode = eMode.move;
            }
        }

        Vector3 vel = Vector3.zero;
        switch (mode)
        {
            case eMode.idle:
                ChangeDirectionFacing("Player_SideIdle", "Player_UpIdle", "Player_DownIdle");
                //timeToPlayAudio = Time.time;
                break;
            case eMode.move:
                vel = directions[dirHeld];

                ChangeDirectionFacing("Player_SideWalk", "Player_UpWalk", "Player_DownWalk");


                if (Time.time >= timeToPlayAudio)
                {
                    aH.PlayMultiClipSound("FootstepsGrass");
                    timeToPlayAudio = Time.time + audioInterval;
                }
                break;
        }

        rb.velocity = vel * speed;

        enteredCombat = false;
    }

    private void ChangeDirectionFacing(string side, string up, string down)
    {
        transform.rotation = Quaternion.Euler(0, 180, 0);
        if (facing == 0) anim.Play(side);
        else if (facing == 1) anim.Play(up);
        else if (facing == 2)
        {
            anim.Play(side);
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (facing == 3) anim.Play(down);
        else Debug.LogError("Invalid facing value");
    }

    public void TakeTurn()
    {
        takingTurn = true;
        pS.remainingSpeed = pS.speed;
        //Debug.Log("Player is taking their turn");
    }
    public void PassTurn()
    {
        if (!takingTurn) return;
        cC.IterateQueue();
        takingTurn = false;
        //Debug.Log("Player passed the turn");
    }

    //Called when a tile is selected. Controls whether or not the call is valid
    //To be valid, it must be the player's turn, and the player can not be doing something already.
    public void TakeAction(Vector3 location)
    {
        if (!takingTurn || hasDecided || skillSelected == null) return;

        skillSelected.TriggeringEffect(location);
        hasDecided = true;

    }

    //Iterates through the patrolpoints and will stop when they reach the end of the path made, or ran out of movement
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PatrolPoint")
        {
            //Debug.Log("Triggered Patrolpoint at: " + collision.transform.position.x + ", " + collision.transform.position.y);
            if (collision.GetComponent<PatrolPoint>().index == patrolIndex)
            {
                if (patrolIndex != 0) pS.remainingSpeed -= 5;
                patrolIndex++;
                //Debug.Log("Iterated to Patrol Index: " + patrolIndex + ", with speed remaining: " + pS.remainingSpeed);

                if (patrolIndex >= patrolPoints.Count || pS.remainingSpeed <= 0)
                {
                    rb.velocity = Vector3.zero;
                    patrolIndex = 0;
                    foreach (GameObject p in patrolPoints)
                    {
                        Destroy(p);
                    }
                    patrolPoints.Clear();
                    isMoving = false;
                    hasDecided = false;
                    transform.position = new Vector3(UtilityMethods.RoundToNumber(transform.position.x, StaticVariables.blockInterval), UtilityMethods.RoundToNumber(transform.position.y, StaticVariables.blockInterval, StaticVariables.offset), 0);
                    ChangeDirectionFacing("Player_SideIdle", "Player_UpIdle", "Player_DownIdle");
                }
                
            }
        }
    }

    //Stops the player from moving in the direction they want when running into ground tiles or an enemy
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Enemy") && isMoving)
        {
            rb.velocity = Vector3.zero;
            patrolIndex = 0;
            foreach (GameObject p in patrolPoints)
            {
                Destroy(p);
            }
            patrolPoints.Clear();
            isMoving = false;
            hasDecided = false;
            transform.position = new Vector3(UtilityMethods.RoundToNumber(transform.position.x, StaticVariables.blockInterval), UtilityMethods.RoundToNumber(transform.position.y, StaticVariables.blockInterval, StaticVariables.offset), 0);
            ChangeDirectionFacing("Player_SideIdle", "Player_UpIdle", "Player_DownIdle");
        }
    }

}
