using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviourPunCallbacks
{
    //Variables
    [SerializeField] float speed;
    [SerializeField] float initialHealth;
    GameManager _gameManager;
    List<GameObject> _playerList;
    Transform _target;
    Rigidbody _rb;
    Health _health;

    private void Start()
    {
        //Esto solo lo tiene que hacer el MasterClient porque él maneja los enemigos
        if (PhotonNetwork.IsMasterClient) 
        {
            //Agarro la lista de objetos de player conectados y selecciono uno para perseguir
            _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            _playerList = _gameManager.GetPlayerList();
            GetTarget();
        }

        //Agarro el RB
        if (photonView.IsMine) _rb = GetComponent<Rigidbody>();

        //Le doy vida al enemigo
        _health = new Health(initialHealth, Death);
    }

    private void Update()
    {
        //Esto solamente lo ejecutará el MasterCliente, porque él es el dueño
        if (!photonView.IsMine) return;

        //calculo la distancia entre la entidad y el target, y si es mayor a 2 se acerca, sino, ataca
        var dir = _target.position - transform.position;

        if (dir.magnitude > 2)
        {
            _rb.velocity = dir.normalized * speed;
            dir.y = 0;
            transform.forward = dir.normalized;
        }
        else
        {
            //Atacar
        }
    }

    //Funcion que agarra al player más cercano y lo persigue
    void GetTarget()
    {
        float previousDist = 10000000000;
        foreach (var player in _playerList)
        {
            var dist = Vector3.Distance(transform.position, player.transform.position);

            if (dist < previousDist)
            {
                _target = player.transform;
                previousDist = dist;
            }
        }
    }

    //Funcion que se ejecuta al morir
    void Death()
    {
        Debug.Log("Me muero");
        _gameManager.EnemyList.Remove(gameObject);
    }
}