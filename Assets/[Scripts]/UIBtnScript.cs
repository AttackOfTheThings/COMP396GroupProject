using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class UIBtnScript : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("Loading 1");
        PhotonNetwork.LoadLevel(7);
    }
}
