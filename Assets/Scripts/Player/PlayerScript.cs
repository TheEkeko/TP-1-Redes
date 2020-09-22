using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerScript : MonoBehaviourPun
{
    [SerializeField] float speed = 7f;

    [SerializeField] string bulletPrefabPath;
    [SerializeField] CharacterController cc;
    [SerializeField] Vector3 movement;
    [SerializeField] Vector3 mousePosition;

    [SerializeField] bool isDead;
    [SerializeField] Health hp;
    
    GameManager _gameManager;

    private void Awake()
    {

        cc = GetComponent<CharacterController>();

        if (PhotonNetwork.IsMasterClient)
        {
            _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            _gameManager.AddPlayerToList(gameObject);
        }

        hp = new Health(100, Die);
        
    }

    void Update()
    {
        if (!photonView.IsMine) return;

        if(!isDead)
        { 
            var h = Input.GetAxis("Horizontal");
            var v = Input.GetAxis("Vertical");
        
            movement = new Vector3(h, 0, v)*Time.deltaTime*speed;
            cc.Move(movement);

            var dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.down);


            if (Input.GetKeyDown(KeyCode.Mouse0)) photonView.RPC("Shoot", RpcTarget.MasterClient);
        }
    }

    public void ChangeLife(float value)
    {
        hp.ChangeLife(value);
    }

    void Die()
    {
        isDead = true;
        Vector3 deadPos = new Vector3(500, 500, 500);
        transform.position = deadPos;
    }

    void Respawn()
    {
        hp.MaxLife();
        isDead = false;
        transform.position = new Vector3(0, 0, 0); //Esto lo hago para tener algo, despues habria que hacer un sistema de respawneo por "lugares"
    }



    //Todas estas funciones que deban ser ejecutadas en otros clientes, requieren esta propiedad, sino, nos tira un error de que no encuentra la funcion
    [PunRPC]
    void Shoot()
    {
        var bullet = PhotonNetwork.Instantiate(bulletPrefabPath, transform.position, Quaternion.identity).GetComponent<BulletTest>();
        var dir = transform.forward;
        dir.y = 0.5f;
        bullet.Shoot(dir);
    }

    [PunRPC] //Esto esta para que cuando se conecte un usuario a la sala, el MC lo agarre y lo guarde en una lista (Esto lo usan los enemigos)
    void AddPlayerToList()
    {
        _gameManager.AddPlayerToList(gameObject);
    }
}
