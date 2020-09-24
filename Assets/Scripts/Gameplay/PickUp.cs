using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviourPunCallbacks
{
    private void OnTriggerEnter(Collider other)
    {
        other.transform.gameObject.GetComponent<PlayerScript>().MaxHp();
        photonView.RPC("DestroyMe", RpcTarget.MasterClient);
    }

    [PunRPC]
    void DestroyMe()
    {
        PhotonNetwork.Destroy(gameObject);
    }
}