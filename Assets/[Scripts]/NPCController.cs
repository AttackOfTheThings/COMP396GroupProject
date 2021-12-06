using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    //Simple FSM (FiniteStateMachine)
    public enum NPCState{ Patrolling, Chasing, Attacking};
    public float CloseEnoughtDistance = 2f; // <=2m => attack
    //To be able to simulate different states

    public NPCState currentState = NPCState.Patrolling;
    public bool bCanSeePlayer;
    public GameObject goPlayer;
    public float enemyHealth = 100;

    int CurrentWayPointIndex = 0;
    public float speed = 1.0f;

    public Animator monsterAnimator;
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

    void Start()
    {
        goPlayer = GameObject.FindGameObjectWithTag("Player");
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
        if(!playerAlive)
        {
            ChangeState(NPCState.Patrolling);
        }
        else
        {
            float distance = Vector3.Distance(this.transform.position, goPlayer.transform.position);
            if(distance > CloseEnoughtDistance)
            {
                if(CanSeeAdversary())
                {
                    ChangeState(NPCState.Chasing);
                }
                else
                {
                    ChangeState(NPCState.Patrolling);
                }
            }
        }
       //If player dies => patrol
       // d(this, target) > attack dist =>
       // if see target => chase
       //else patrol


    }

    private void HandleChasingState()
    {
        Debug.Log((goPlayer.transform.position - this.transform.position).magnitude);
        Debug.Log("In NPCController.HandleChasingState");

        float distance = Vector3.Distance(this.transform.position, goPlayer.transform.position);
        Debug.Log("Distance Between"  + distance);
        if (distance <= CloseEnoughtDistance)
        {
            ChangeState(NPCState.Attacking);
            monsterAnimator.SetBool("isChasing", true);
            monsterAnimator.SetBool("isAttackRange", true);

            Invoke( "Attack2", 0.8f);

        }
        else
        {
            this.transform.position = MyMoveTowards(this.transform.position, goPlayer.transform.position, speed * Time.deltaTime);
            monsterAnimator.SetBool("isChasing", true);
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
        }

       
    }
    
    

    Vector3 MyMoveTowards(Vector3 current, Vector3 target, float maxDistanceDelta)
    {
        Vector3 c2t = target - current;
        Quaternion qtargetRotation = Quaternion.LookRotation(c2t);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, qtargetRotation, Time.deltaTime);

        Vector3 movement = current + c2t.normalized * maxDistanceDelta;
        return movement;
    }

  


    private bool CanSeeAdversary()
    {
        Vector3 playerPos = goPlayer.transform.position;
        Vector3 enemyToPlayerHeading = playerPos - this.transform.position;
        float cosAngleE2P = Vector3.Dot(this.transform.forward, enemyToPlayerHeading)/enemyToPlayerHeading.magnitude;
        bCanSeePlayer = (cosAngleE2P > 0);
        float angle = Vector3.Angle(this.transform.forward, enemyToPlayerHeading);
        Debug.Log("angle = " + angle);
        return bCanSeePlayer; //for testing prposes

        //cos (theta)=v1.v2/(|v1|*|v2|)
        //if v1 is a unit vector => |v1|=1 //foward = (0,0,1)

    }
    
    public void Hit(float damageEnemy)
    {
        enemyHealth -= damageEnemy;
        //slider.value = enemyHealth;

        if (enemyHealth <= 0)
        {
            monsterAnimator.SetTrigger("isDead");
            //gameManager.enemiesAlive--;
            Destroy(gameObject, 3f);
            //Destroy(GetComponent<NavMeshAgent>());
            //Destroy(GetComponent<EnemyManager>());
            Destroy(GetComponent<CapsuleCollider>());
            //Destroy(GetComponent<Canvas>());
            Debug.Log("Died");
        }
        Debug.Log(enemyHealth);
    }

   
    public void  Attack2()
    {
        goPlayer.GetComponent<PlayerManager>().Hit(50);


    }
}
