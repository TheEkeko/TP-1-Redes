using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerScript : MonoBehaviourPun
{
    float speed = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Esto lo vamos a tener que hacer mas prolijo con un script de movimiento supongo, aparte no se si esto funcionara cuando hagamos el server pero creo que eso es para el proximo tp
        if (photonView.IsMine == true) 
        { 
            var h = Input.GetAxis("Horizontal");
            var v = Input.GetAxis("Vertical");
            transform.position += new Vector3(h, 0, v) * Time.deltaTime * speed;
        }
    }
}
