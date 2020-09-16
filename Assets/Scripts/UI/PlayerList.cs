using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class PlayerList : MonoBehaviour
{
    [SerializeField] Text _playerText;
    [SerializeField] Text _playerPing;

    //El flaco uso TestMeshpro, pero alta paja :D No afecta al online jajaja

    //Esto es basicamente para tener una lista de usuarios y el ping

    private void Update()
    {
        string _pList = "";
        var _playerList = PhotonNetwork.PlayerList; //esto devuelve a todos los players

        foreach (var p in _playerList)
        {
            _pList += p.NickName + "\n";
        }

        _playerText.text = _pList;

        _playerPing.text = "Ping: " + PhotonNetwork.GetPing().ToString(); //Cabe resaltar que este ping es solo entre mi cliente y el server de photon. Si usamos a un cliente como server, hay qeu multiplicarlo por 4 para aproximar
    }
}