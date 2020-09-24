using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviourPun
{
    [SerializeField] float speed;
    [SerializeField] string bulletPrefabPath;
    [SerializeField] CharacterController cc;
    [SerializeField] Vector3 movement;
    [SerializeField] Vector3 mousePosition;
    [SerializeField] bool isDead;
    [SerializeField] float initialhealth;
    [SerializeField] Text hpText;
    Health hp;
    GameManager _gameManager;

    [SerializeField] CameraBehaviour cB;
    private void Awake()
    {

        cc = GetComponent<CharacterController>();

        if (PhotonNetwork.IsMasterClient)
        {
            _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            _gameManager.AddPlayerToList(this);
        }

        hp = new Health(initialhealth, Die);
        if(photonView.IsMine)
        { 
            cB = Camera.main.gameObject.GetComponentInParent<CameraBehaviour>();
            cB.GetPlayer(this.gameObject);
        }
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
            if(!cc.isGrounded)
            {
                cc.Move(new Vector3(0, -9.8f, 0)*Time.deltaTime);
            }
            var dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.down);


            if (Input.GetKeyDown(KeyCode.Mouse0)) photonView.RPC("Shoot", RpcTarget.MasterClient);
        }
    }

    [PunRPC]
    public void ChangeLife(float value)
    {
        if (photonView.IsMine) 
        { 
            hp.ChangeLife(value);
            hpText.text = ("HP: " + hp.HP);
        }
    }

    void Die()
    {
        if(photonView.IsMine)
        { 
            isDead = true;
            Vector3 deadPos = new Vector3(500, 500, 500);
            transform.position = deadPos;
        
            cB.StopFollowing();
            hpText.text = ("Dead");
            _gameManager.CheckEndgame();
        }
    }

    [PunRPC]
    void RespawnRPC(Vector3 pos)
    {
        if (photonView.IsMine)
        {
            pos.y += 1;
            hp.MaxLife();
            isDead = false;
            hpText.text = ("HP: 100");
            Debug.Log(hp.HP);
            cB.GetPlayer(this.gameObject);
            this.gameObject.transform.position = pos; //POR QUE VERGA ESTO NO FUNCA??? Lo probe escribir de todas las maneras que se me ocurrio
            
            //setPosition(pos);
        }
    }

    public bool IsDead
    {
        get { return isDead; }
    }

    public void getHpText(Text t)
    {
        if(photonView.IsMine)
        {
            hpText = t;
        }
    }

    public void GetDamaged(float value)
    {
        photonView.RPC("ChangeLife", RpcTarget.All, value);
    }

    public void Respawn(Vector3 pos)
    {
        photonView.RPC("RespawnRPC", RpcTarget.All, pos);
    }

    /*public void setPosition(Vector3 pos) //Probe esto a ver si ahi si cambiaba la posicion pero nada
    {
        transform.position = pos;
    }*/

    //Todas estas funciones que deban ser ejecutadas en otros clientes, requieren esta propiedad, sino, nos tira un error de que no encuentra la funcion
    [PunRPC]
    void Shoot()
    {
        var bullet = PhotonNetwork.Instantiate(bulletPrefabPath, transform.position, Quaternion.identity).GetComponent<BulletTest>();
        var dir = transform.forward;
        dir.y = 0.5f;
        bullet.Shoot(dir);
    }
}
