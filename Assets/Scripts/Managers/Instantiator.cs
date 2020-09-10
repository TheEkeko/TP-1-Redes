using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Instantiator : MonoBehaviour
{
    void Start()
    {
        /* Instancio el prefab PlayerPrefab, esto supongo que o en la clase 5 explicara otro metodo,
        o tendremos que adaptarlo para que dependiendo de la clase del jugador tome el prefab correspondiente
        
         Para que esto funcione los prefabs tienen que tener el componente PhotonView */

        PhotonNetwork.Instantiate("Prefabs/PlayerPrefabs/PlayerPrefab", Vector3.zero, Quaternion.identity);
    }
}
