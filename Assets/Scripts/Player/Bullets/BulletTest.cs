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
    [SerializeField] GameObject impactParticle;
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
        yield return new WaitForSeconds(2f);
        PhotonNetwork.Destroy(gameObject);
    }
    
    private void OnCollisionEnter(Collision collision) //PADRE ESCUCHE, aca no tendriamos que hacer algun tipo de checkeo que la bala solo tome el impacto si isMine o si es el masterclient?
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
        Quaternion particleRot = new Quaternion(-90, 0, 0,0);
        Instantiate(impactParticle, transform.position, particleRot);
        PhotonNetwork.Destroy(gameObject);
    }

    /*[PunRPC]
    void SpawnParticles()
    {
        PhotonNetwork.Instantiate(particlePrefabPath, transform.position, Quaternion.identity);
    }*/
}