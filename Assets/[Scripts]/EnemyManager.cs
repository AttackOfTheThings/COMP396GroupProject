using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Photon.Pun;

public class EnemyManager : MonoBehaviour
{
    public GameObject player;
    public Animator enemyAnimator;
    public float damage = 20f;
    public float enemyHealth = 100f;
    public GameManager gameManager;
    public Slider slider;

    public bool playerIsInReach;
    private float attackDelayTimer;

    public float attackAnimationDelay;
    public float delayBetweenAttacks;

    public AudioSource audioSource;

    public AudioClip[] zombieSounds;
    GameObject[] playersInScene;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playersInScene = GameObject.FindGameObjectsWithTag("Player");
        slider.maxValue = enemyHealth;
        slider.value = enemyHealth;

    }

    // Update is called once per frame
    void Update()
    {
        playersInScene = GameObject.FindGameObjectsWithTag("Player");
        if (!audioSource.isPlaying)
        {
            audioSource.clip = zombieSounds[Random.Range(0, zombieSounds.Length)];
            audioSource.Play();
        }
        if(PhotonNetwork.InRoom && !PhotonNetwork.IsMasterClient)
        {
            return;
        }

        GetClosestPlayer();
        if(player != null)
        {
            slider.transform.LookAt(player.transform);
            GetComponent<NavMeshAgent>().destination = player.transform.position;
        }

        
        if(GetComponent<NavMeshAgent>().velocity.magnitude > 1)
        {
            enemyAnimator.SetBool("isRunning", true);
        }
        else
        {
            enemyAnimator.SetBool("isRunning", false);
        }
    }

    private void GetClosestPlayer()
    {
        float mindistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach(GameObject thisPlayer in playersInScene)
        {
            if(thisPlayer != null)
            {
                float distance = Vector3.Distance(thisPlayer.transform.position, currentPosition);
                if (distance < mindistance)
                {
                    player = thisPlayer;
                    mindistance = distance;
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject == player)
        {
            playerIsInReach = true;
            
        }
    }

    public void Hit(float damageEnemy)
    {
        enemyHealth -= damageEnemy;
        slider.value = enemyHealth;
        
        if(enemyHealth <= 0)
        {
            enemyAnimator.SetTrigger("isDead");
            gameManager.enemiesAlive--;
            Destroy(gameObject, 3f);
            Destroy(GetComponent<NavMeshAgent>());
            Destroy(GetComponent<EnemyManager>());
            Destroy(GetComponent<CapsuleCollider>());
            //Destroy(GetComponent<Canvas>());
            Debug.Log("Died");
        }
        Debug.Log(enemyHealth);
    }
    private void OnCollisionStay(Collision collision)
    {
        if (playerIsInReach)
        {
            attackDelayTimer += Time.deltaTime;
        }

        if(attackDelayTimer >= delayBetweenAttacks -attackAnimationDelay && attackDelayTimer <= delayBetweenAttacks && playerIsInReach)
        {
            enemyAnimator.SetTrigger("isAttacking");
        }

        if(attackDelayTimer >= delayBetweenAttacks && playerIsInReach)
        {
            player.GetComponent<PlayerManager>().Hit(damage);
            attackDelayTimer = 0;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject == player)
        {
            playerIsInReach = false;
            attackDelayTimer = 0;
        }
    }
}
