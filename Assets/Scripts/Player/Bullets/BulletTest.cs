using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTest : MonoBehaviourPun //El nombre es bullet test porque pareciera que ya hay una clase llamada Bullet
{
    /*
    A ver, aca faltan que la bala y el jugador no colisionen. Pero es una boludes de cambiar las layers y las pyshics de Unity, pero al menos
    tenemos el modelo de como debe funcar la bala.
    */

    [SerializeField] float speed;
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
            Debug.Log("ded :(");
        }
    }
}