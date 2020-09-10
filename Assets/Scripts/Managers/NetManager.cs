using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetManager : MonoBehaviourPunCallbacks
{
    void Start()
    {
        SetUpGame();
    }

    void Update()
    {

    }

    public void SetUpGame()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected");

        //Creo las options de la room para que sea de 4 jugadores
        RoomOptions rO = new RoomOptions();
        rO.MaxPlayers = 4;

        //Creo la room
        PhotonNetwork.JoinOrCreateRoom("GameRoom", rO, TypedLobby.Default);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Created room: " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room: " + PhotonNetwork.CurrentRoom.Name);

        //Cargo la scene del juego, a esto hay que agregarle cosas de la clase 5
        PhotonNetwork.LoadLevel("GameScene");
    }
}
