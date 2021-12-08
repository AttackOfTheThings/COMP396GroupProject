using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerManager : MonoBehaviour
{
    public float health = 100f;
    public Text healthText;
    public GameManager gameManager;
    public GameObject playerCamera;
    private float shakeTime;
    private float shakeDuration;
    //private Quaternion playerCameraOriginalRotation;
    public CanvasGroup hurtPannel;
    public PhotonView photonView;



    public void Hit(float damage)
    {
        health -= damage;
        healthText.text = "Health: " + health.ToString() + " %";
        if (health <= 0)
        {
            gameManager.GameOver();
        }
        else
        {
            shakeTime = 0f;
            shakeDuration = 0.2f;
            hurtPannel.alpha = 1;
        }
    }

    
    // Start is called before the first frame update
    void Start()
    {
        //playerCameraOriginalRotation = playerCamera.transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.InRoom && !photonView.IsMine)
        {
            playerCamera.SetActive(false);
            return;
        }

        if (hurtPannel.alpha > 0)
        {
            hurtPannel.alpha -= Time.deltaTime;
        }
        /*
        if(shakeTime < shakeDuration)
        {
            shakeTime += Time.deltaTime;
            CameraShake();
        }
        else if (playerCamera.transform.localRotation != playerCameraOriginalRotation)
        {
            playerCamera.transform.rotation = playerCameraOriginalRotation;
        }
        */
        
    }
    public void CameraShake()
    {
        playerCamera.transform.localRotation = Quaternion.Euler(Random.Range(-2, 2), 0, 0);
    }
}
