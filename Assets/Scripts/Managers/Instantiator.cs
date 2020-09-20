using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Instantiator : MonoBehaviourPunCallbacks
{
    [SerializeField] GameManager _gameManager;

    void Start()
    {
        /* Instancio el prefab PlayerPrefab, esto supongo que o en la clase 5 explicara otro metodo,
        o tendremos que adaptarlo para que dependiendo de la clase del jugador tome el prefab correspondiente
        
         Para que esto funcione los prefabs tienen que tener el componente PhotonView 

        PhotonNetwork.Instantiate("Prefabs/PlayerPrefabs/PlayerPrefab", Vector3.zero, Quaternion.identity); */
        string playerClass = "";
        int playerClassIndex = ClassManager.Instance.PlayerClass;
        switch(playerClassIndex)
        {
            case 0:
                playerClass = "Prefabs/PlayerPrefabs/FireClassPrefab";
                break;
            case 1:
                playerClass = "Prefabs/PlayerPrefabs/IceClassPrefab";
                break;
            case 2:
                playerClass = "Prefabs/PlayerPrefabs/ConfusionClassPrefab";
                break;
        }

        PhotonNetwork.Instantiate(playerClass, Vector3.zero, Quaternion.identity);
    }
}
