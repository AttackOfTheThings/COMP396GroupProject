using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class NetworkingManager : MonoBehaviourPunCallbacks
{
    public GameObject connecting;
    public GameObject multiPlayer;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Connecting to Server");
        PhotonNetwork.ConnectUsingSettings();
    }

    // Update is called once per frame
    void Update()
    {
        OnJoinedLobby();
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("Joining Lobby");
        PhotonNetwork.JoinLobby();

        //base.OnConnectedToMaster();
    }

    public override void OnJoinedLobby()
    {
        int playerCount = PhotonNetwork.CountOfPlayers;
        Debug.Log("Ready for multiplayer");
        if (playerCount >= 2)
        {
            connecting.SetActive(false);

            multiPlayer.SetActive(true);
        }
        
        //base.OnJoinedLobby();
    }
    public void FindMatch()
    {
        Debug.Log("Finding A Room");
        PhotonNetwork.JoinRandomRoom();

    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        MakeRoom();
        //base.OnJoinRandomFailed(returnCode, message);
    }
    void MakeRoom()
    {
        int randomRoomName = Random.Range(0, 5000);


        RoomOptions roomOptions = new RoomOptions()
        {
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = 6,
            PublishUserId = true
        };

        PhotonNetwork.CreateRoom("RoomName_" + randomRoomName, roomOptions);
        Debug.Log("Room Made: " + randomRoomName);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Loading 1");
        PhotonNetwork.LoadLevel(7);
        //base.OnJoinedRoom();
    }
}
