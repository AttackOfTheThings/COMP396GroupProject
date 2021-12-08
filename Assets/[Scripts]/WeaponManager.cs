using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WeaponManager : MonoBehaviour
{
    public GameObject playerCamera;
    public float weaponRange = 100f;
    public int weaponDamage = 50;
    public Animator playerAnimator;

    public ParticleSystem muzzleFlash;
    public GameObject hitParticle;

    public AudioClip gunShot;
    public AudioSource audioSource;

    public PhotonView photonView;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        weaponDamage = PlayerPrefs.GetInt("Diff");
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.InRoom && !photonView.IsMine)
        {
            return;
        }
        //turn off Shooting
        if (playerAnimator.GetBool("isShooting"))
        {
            playerAnimator.SetBool("isShooting", false);
        }

        if (Input.GetButtonDown("Fire1"))
        {
            ShootWeapon();

        }
    }
    void ShootWeapon()
    {
        muzzleFlash.Play();
        audioSource.PlayOneShot(gunShot);
        playerAnimator.SetBool("isShooting", true);
        RaycastHit hit;
        //(origin, directionTraveled, whatItHit, weaponRange)
        if(Physics.Raycast(playerCamera.transform.position, transform.forward, out hit, weaponRange))
        {
            EnemyManager enemyManager = hit.transform.GetComponent<EnemyManager>();
            if(enemyManager !=null)
            {
                enemyManager.Hit(weaponDamage);
                GameObject instParticle = Instantiate(hitParticle, hit.point, Quaternion.LookRotation(hit.normal)); //intantiate particles
                instParticle.transform.parent = hit.transform;

                Destroy(instParticle, 2f);
            }
            NPCController npcController = hit.transform.GetComponent<NPCController>();
            if(npcController != null)
            {
                npcController.Hit(weaponDamage/2);
                GameObject instParticle = Instantiate(hitParticle, hit.point, Quaternion.LookRotation(hit.normal));
                instParticle.transform.parent = hit.transform;
            }
        }
    }

    
}
