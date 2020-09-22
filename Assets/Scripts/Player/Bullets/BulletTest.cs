using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class BulletTest : MonoBehaviourPun //El nombre es bullet test porque pareciera que ya hay una clase llamada Bullet
{

    [SerializeField] float speed;
    [Tooltip("0 para fuego, 1 para hielo, 2 para confusion")]
    [SerializeField] int bulletType;
    Rigidbody _rb;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (photonView.IsMine) transform.forward = _rb.velocity.normalized; //Que mire hacia adelante
    }

    public void Shoot(Vector3 dir)
    {
        _rb.AddForce(dir * speed, ForceMode.Impulse);
        StartCoroutine(WaitToDestroy());
    }

    IEnumerator WaitToDestroy()
    {
        yield return new WaitForSeconds(1.5f);
        PhotonNetwork.Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (photonView.IsMine)
        {
            UnityEngine.Debug.Log("Bullet collision");
        }

        /*
        switch (bulletType)
        {
            case 0:
                //Agregar comportamiento correspondiente a la bala de fuego
                break;
            case 1:
                //Agregar comportamiento correspondiente a la bala de hielo
                break;
            case 2:
                //Agregar comportamiento correspondiente a la bala de confusion
                break;
        }
        */
        Destroy(this.gameObject);
    }
}