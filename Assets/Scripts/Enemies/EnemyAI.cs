using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviourPunCallbacks
{
    //Variables
    [SerializeField] float speed;
    [SerializeField] float initialHealth;
    [SerializeField] float damage;
    [SerializeField] SkeletonAnim _skeletonAnim;
    GameManager _gameManager;
    List<PlayerScript> _playerList;
    Transform _target;
    Rigidbody _rb;
    Health _health;
    PlayerScript _targetScript;
    float _attackCooldown;
    const float _maxAttackCooldown = 5f;

    private void Start()
    {
        //Esto solo lo tiene que hacer el MasterClient porque él maneja los enemigos
        if (!PhotonNetwork.IsMasterClient) return;
       
        //Agarro la lista de objetos de player conectados y selecciono uno para perseguir
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _playerList = _gameManager.GetPlayerList();
        GetTarget();
        
        //Agarro el RB
        if (photonView.IsMine) _rb = GetComponent<Rigidbody>();

        //Le doy vida al enemigo
        _health = new Health(initialHealth, Death);

        //Inicializo el timer
        _attackCooldown = 0;
    }

    private void Update()
    {
        //Esto solamente lo ejecutará el MasterCliente, porque él es el dueño
        if (!photonView.IsMine) return;

        if (!_targetScript.IsDead)
        {
            //calculo la distancia entre la entidad y el target, y si es mayor a 2 se acerca, sino, ataca
            var dir = _target.position - transform.position;

            if (dir.magnitude > 2)
            {
                _rb.velocity = dir.normalized * speed;
                dir.y = 0;
                transform.forward = dir.normalized;
                if (_skeletonAnim) _skeletonAnim.Move();
            }
            else
            {
                //Atacar
                Attack();
            }
        }
        else
        {
            GetTarget();
        }

        _attackCooldown -= Time.deltaTime;
    }

    //Funcion que agarra al player más cercano y lo persigue
    void GetTarget()
    {
        bool gotTarget = false;
        float previousDist = 10000000000;
        foreach (var player in _playerList)
        {
            var dist = Vector3.Distance(transform.position, player.transform.position);

            if (dist < previousDist)
            {
                gotTarget = true;
                _target = player.transform;
                _targetScript = player;
                previousDist = dist;
            }
        }

        if (!gotTarget)
        {
            //Game Over
        }
    }

    //Funcion que realiza daño
    public void GetDamaged(float damage)
    {
        if (PhotonNetwork.IsMasterClient) _health.ChangeLife(damage);
    }

    //Funcion que se ejecuta al morir
    void Death()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _gameManager.EnemyList.Remove(gameObject);
            PhotonNetwork.Destroy(gameObject);
        }
    }

    void Attack()
    {
        if (_attackCooldown <= 0)
        {
            if (_skeletonAnim) _skeletonAnim.Attack();

            _targetScript.GetDamaged(damage);
            _attackCooldown = _maxAttackCooldown;

            if (_targetScript.IsDead) GetTarget();

            //if (_skeletonAnim) _skeletonAnim.ResetAttack();
        }
    }
}