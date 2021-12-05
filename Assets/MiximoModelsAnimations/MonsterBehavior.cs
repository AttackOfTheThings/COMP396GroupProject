using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum MonsterState { Idle, Walk, Jump}
public class MonsterBehavior : MonoBehaviour
{
   
    private NavMeshAgent agent;
    private Animator animator;
    [Header("Line of site")]
    public Vector3 offset = new Vector3(0f, 2f, 5f);
    public bool hasLineOfSite;
    public Vector3 playerLocation;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>(); // detect animator controller Could also drag and drop
        agent = GetComponent<NavMeshAgent>(); // detect navMeshAgent
    }

    // Update is called once per frame
    void Update()
    {
        var size = new Vector3(2f, 3f, 10f);
        RaycastHit Hit;
       
        if(hasLineOfSite)
        {
            agent.SetDestination(playerLocation);
        }

        
       
        if(Input.GetKeyDown(KeyCode.U))
        {
            animator.SetInteger("AnimState", (int)MonsterState.Walk);
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            animator.SetInteger("AnimState", (int)MonsterState.Jump);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            animator.SetInteger("AnimState", (int)MonsterState.Idle);
        }
        //if(animator.SetInteger("AnimState"))
    }

    private void OnTriggerEnter(Collider other)
    {
        
        {
            Debug.Log(other.gameObject.name);
            hasLineOfSite = true;
            playerLocation = other.transform.position;
        }
       
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log(other.gameObject.name);
            hasLineOfSite = false;
        }
          
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerLocation = other.transform.position;
        }

    }
}
