using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    public Animator monsterAnimator;
    //Simple FSM (FiniteStateMachine)
    public enum NPCState{ Patrolling, Chasing, Attacking};
    public float CloseEnoughtDistance = 2f; // <=2m => attack
    //To be able to simulate different states

    public NPCState currentState = NPCState.Patrolling;
    public bool bCanSeePlayer;
    public GameObject goPlayer;

    public bool playerIsInReach;
    private float attackDelayTimer;
    public float attackAnimationDelay;
    public float delayBetweenAttacks;

    //new as of sept 20th

    GameObject[] WayPoints;
    int CurrentWayPointIndex = 0;
    public float speed = 1.0f;

    //Spet 22
    public float angularSpeedDegPerSec = 60.0f; //deg per second

    void ChangeState(NPCState newState)
    {
        currentState = newState;
    }

    float Deg2Rad (float deg)
    {
        //return deg * Mathf.Deg2Rad;
        return deg / 180f * Mathf.PI;
    }

    float Rad2Deg (float rad)
    {
        //return rad * Mathf.Rad2Deg;
        return rad / Mathf.PI * 180;
    }
    void Start()
    {
        goPlayer = GameObject.FindGameObjectWithTag("Player");
        //First Method use tags
        WayPoints = GameObject.FindGameObjectsWithTag("WayPoint");

        monsterAnimator = GetComponent<Animator>();
        angularSpeedDegPerSec = Deg2Rad(angularSpeedDegPerSec);
        

    }

    // Update is called once per frame
    void Update()
    {
        angularSpeedDegPerSec = Deg2Rad(angularSpeedDegPerSec);
        HandleFSM();
    }
    void HandleFSM()
    {
        switch(currentState)
        {
            case NPCState.Patrolling:
                HandlePatrollingState();
                break;
            case NPCState.Chasing:
                HandleChasingState();
                break;
            case NPCState.Attacking:
                HandleAttackingState();
                break;
            default:
                break;
        }
    }

    private void HandleAttackingState()
    {
        Debug.Log((goPlayer.transform.position - this.transform.position).magnitude);
        Debug.Log("In NPCController.HandleAttackingState");
        

        bool playerAlive = goPlayer.GetComponent<PlayerManager>();
      
            float distance = Vector3.Distance(this.transform.position, goPlayer.transform.position);
            if(distance <= CloseEnoughtDistance)
            {
                monsterAnimator.SetBool("isAttackRange", true);
                monsterAnimator.SetBool("isChasing", true);
               
               
            }
            else
            {
                monsterAnimator.SetBool("isChasing", false);
                monsterAnimator.SetBool("isAttackRange", false);
                ChangeState(NPCState.Patrolling);

            }
        

    }

    private void HandleChasingState()
    {
        Debug.Log((goPlayer.transform.position - this.transform.position).magnitude);
        Debug.Log("In NPCController.HandleChasingState");
        if (!CanSeeAdversary()) // can't see so patrolling
        {
            ChangeState(NPCState.Patrolling);
        }
        if(Vector3.Distance(this.transform.position, goPlayer.transform.position) <= CloseEnoughtDistance)
        {
            ChangeState(NPCState.Attacking);
            
        }
        else
        {
            this.transform.position = MyMoveTowards(this.transform.position, goPlayer.transform.position, speed * Time.deltaTime);
            monsterAnimator.SetBool("isAttackRange", false);
        }
        this.transform.position = MyMoveTowards(this.transform.position, goPlayer.transform.position, speed * Time.deltaTime);
       
    }
    private void HandlePatrollingState()
    {
        Debug.Log((goPlayer.transform.position - this.transform.position).magnitude);
        Debug.Log("In NPCController.HandlePatrollingState");
        if (CanSeeAdversary()) // can see so chasing
        {
            ChangeState(NPCState.Chasing);
            monsterAnimator.SetBool("isChasing", true);
        }
        else if (CanSeeAdversary() && ((goPlayer.transform.position - this.transform.position).magnitude < CloseEnoughtDistance) == true) // can see too close so attacking
        {
            ChangeState(NPCState.Attacking);
            monsterAnimator.SetBool("isAttackingRange", true);
        }
        //monsterAnimator.SetBool("isChasing", true);
        FollowPatrolingPath();
    }
    //----------------------------------------------------------------------------------------------------------------------------------------
    /*
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == goPlayer)
        {
            playerIsInReach = true;

        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (playerIsInReach)
        {
            attackDelayTimer += Time.deltaTime;
        }

        if (attackDelayTimer >= delayBetweenAttacks - attackAnimationDelay && attackDelayTimer <= delayBetweenAttacks && playerIsInReach)
        {
            monsterAnimator.SetTrigger("isAttacking");
        }

        if (attackDelayTimer >= delayBetweenAttacks && playerIsInReach)
        {
            goPlayer.GetComponent<PlayerManager>().Hit(50);
            attackDelayTimer = 0;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == goPlayer)
        {
            playerIsInReach = false;
            attackDelayTimer = 0;
        }
    }
    */
    //-----------------------------------------------------------------------------------------------------------------------------------------
    
    private void FollowPatrolingPath()
    {
        Vector3 target = WayPoints[CurrentWayPointIndex].transform.position;

        if (Vector3.Distance(this.transform.position, target) < 0.1f)
        {
            CurrentWayPointIndex = CalculateNextWayPointIndex();
            target = WayPoints[CurrentWayPointIndex].transform.position;
        }
        
        Vector3 movement =  MyMoveTowards(this.transform.position, target, speed * Time.deltaTime);
        this.transform.position = movement;
        
    }

    Vector3 MyMoveTowards(Vector3 current, Vector3 target, float maxDistanceDelta)
    {
        Vector3 c2t = target - current;
        Quaternion qtargetRotation = Quaternion.LookRotation(c2t);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, qtargetRotation, Time.deltaTime);

        Vector3 movement = current + c2t.normalized * maxDistanceDelta;
        return movement;
    }

    private int CalculateNextWayPointIndex()
    {
        //Strategy 1 - follow in order
        return CurrentWayPointIndex = (CurrentWayPointIndex + 1) % WayPoints.Length;
    }


    private bool CanSeeAdversary()
    {
       
        Vector3 playerPos = goPlayer.transform.position;
        Vector3 enemyToPlayerHeading = playerPos - this.transform.position;
        float cosAngleE2P = Vector3.Dot(this.transform.forward, enemyToPlayerHeading)/enemyToPlayerHeading.magnitude;
        //float cosAngleE2P = Vector3.Dot(this.transform.forward, enemyToPlayerHeading); // we need only the sign of cosAngel, so no need to devinde by a positive value (optimization)
        bCanSeePlayer = (cosAngleE2P > 0);
        float angle = Vector3.Angle(this.transform.forward, enemyToPlayerHeading);
        Debug.Log("angle = " + angle);
        return bCanSeePlayer; //for testing prposes


    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        if(WayPoints != null && WayPoints.Length > 0)
        {
            for (int i = 0; i < WayPoints.Length; i++)
            {
                Vector3 from = WayPoints[i].transform.position;
                Vector3 to = WayPoints[(i + 1) % WayPoints.Length].transform.position;
                Gizmos.DrawLine(from, to);
            }
        }
        
    }
}
