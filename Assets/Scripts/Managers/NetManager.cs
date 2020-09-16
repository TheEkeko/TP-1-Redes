﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class NetManager : MonoBehaviourPunCallbacks
{
    [SerializeField] Text _username;

    public void SetUpGame() //En la clase 5 el flaco hace esta misma funcion, pero la llama "ConnectedToRoom" por si llegas a revisar el codigo de él
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected");

        //Creo las options de la room para que sea de 4 jugadores
        RoomOptions rO = new RoomOptions();
        rO.MaxPlayers = 4;

        //Le pongo el nombre al usuario
        var pName = _username.text;
        if (pName.Trim() == "") pName = "usuario" + Random.Range(0, 100).ToString(); //Esto es basicamente por si el usuario es tan picarón de dejar el usarname vacio :)
        PhotonNetwork.LocalPlayer.NickName = pName;

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
