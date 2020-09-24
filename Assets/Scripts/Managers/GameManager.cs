using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks
{
    //Constantes
    const int _minPlayersToStartGame = 2;
    const int _minEnemiesToStartWave = 1;
    const int _maxWaves = 4;

    //Variables
    [SerializeField] List<PlayerScript> _playerList;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] GameObject loseText;
    [SerializeField] GameObject winText;
    bool _gameStart;
    float _timer;
    List<GameObject> _enemyList;
    int _actualWave;
    float _pickUpTimer;

    //Referencias a la ubicación de los enemigos
    const string _slimePrefab = "Prefabs/EnemyPrefabs/Slime";
    const string _gremlingPrefab = "Prefabs/EnemyPrefabs/Gremling";
    const string _ghostPrefab = "Prefabs/EnemyPrefabs/Ghost";
    const string _skeletonPrefab = "Prefabs/EnemyPrefabs/Skeleton";

    //Referencia a la ubicacion del Pickup
    const string _pickUpPrefab = "Prefabs/GameplayPrefabs/PickUp";


    private void Awake()
    {
        //Si no es masterclient, chau
        if (!PhotonNetwork.IsMasterClient) return;

        //inicializo las variables
        _gameStart = false;
        _timer = 10f;
        _enemyList = new List<GameObject>();
        _playerList = new List<PlayerScript>();
        _actualWave = 0;
        _pickUpTimer = 30f;
    }

    private void Update()
    {
        //Si no es masterclient, chau
        if (!PhotonNetwork.IsMasterClient) return;

        //Si arranco el juego, hago el control de Waves, en caso contrario espero a que se conecten mas jugadores
        if (!_gameStart)
        {
            CheckPlayerForStart();
        } 
        else
        {
            WaveControl();
            PickUpSpawn();
        }
    }

    void PickUpSpawn()
    {
        if (_pickUpTimer > 0) _pickUpTimer -= Time.deltaTime;
        else
        {
            PhotonNetwork.Instantiate(_pickUpPrefab, Vector3.zero, Quaternion.identity);
            _pickUpTimer = 30f;
        }
    }

    //Espera a que se conecten usuarios
    void CheckPlayerForStart()
    {
        //Si no es el MasterClient, no va a tener control de las waves ni los enemigos
        if (!PhotonNetwork.IsMasterClient) return;

        //Agarro la cantidad de jugadores en la sala
        int players = PhotonNetwork.CurrentRoom.PlayerCount;

        //Si hay mas jugadores que el mínimo, arranca el juego
        if (players >= _minPlayersToStartGame)
        {
            _gameStart = true;
        }
    }

    //Espera unos 10 segundos y arranca el juego
    void WaveControl()
    {
        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
        } 
        else
        {
            //inicializa las waves
            if (_enemyList.Count < _minEnemiesToStartWave && _actualWave < _maxWaves)
            {
                SpawnWave();
                _actualWave++;
            }

            if (_actualWave == _maxWaves && _enemyList.Count == 0) photonView.RPC("WinGame", RpcTarget.All);

            //termino el juego si mataron a todos los enemigos en todas las rondas
            //if (_actualWave == _maxWaves && _enemyList.Count == 0) EndGame();
        }
    }

    //Funcion que agrega al jugador en la lista de jugadores
    public void AddPlayerToList(PlayerScript player)
    {
        _playerList.Add(player);
    }

    //Devuelve la lista de jugadores
    public List<PlayerScript> GetPlayerList()
    {
        return _playerList;
    }

    //Acceso a la lista de enemigos
    public List<GameObject> EnemyList
    {
        get { return _enemyList; }
    }

    //Funcion que spawnea la wave
    void SpawnWave()
    {
        //Genero un array de todos los bichos
        string[] enemyType = new string[4];
        enemyType[0] = _slimePrefab;
        enemyType[1] = _gremlingPrefab;
        enemyType[2] = _ghostPrefab;
        enemyType[3] = _skeletonPrefab;

        var selectedEnemy = enemyType[Random.Range(0, enemyType.Length)];
        var enemy = PhotonNetwork.Instantiate(selectedEnemy, spawnPoints[0].position, Quaternion.identity);
        _enemyList.Add(enemy);
        selectedEnemy = enemyType[Random.Range(0, enemyType.Length)];
        enemy = PhotonNetwork.Instantiate(selectedEnemy, spawnPoints[1].position, Quaternion.identity);
        _enemyList.Add(enemy);
        selectedEnemy = enemyType[Random.Range(0, enemyType.Length)];
        enemy = PhotonNetwork.Instantiate(selectedEnemy, spawnPoints[2].position, Quaternion.identity);
        _enemyList.Add(enemy);
        selectedEnemy = enemyType[Random.Range(0, enemyType.Length)];
        enemy = PhotonNetwork.Instantiate(selectedEnemy, spawnPoints[3].position, Quaternion.identity);
        _enemyList.Add(enemy);
        selectedEnemy = enemyType[Random.Range(0, enemyType.Length)];
        enemy = PhotonNetwork.Instantiate(selectedEnemy, spawnPoints[4].position, Quaternion.identity);
        _enemyList.Add(enemy);
        selectedEnemy = enemyType[Random.Range(0, enemyType.Length)];
        enemy = PhotonNetwork.Instantiate(selectedEnemy, spawnPoints[5].position, Quaternion.identity);
        _enemyList.Add(enemy);
        selectedEnemy = enemyType[Random.Range(0, enemyType.Length)];
        enemy = PhotonNetwork.Instantiate(selectedEnemy, spawnPoints[6].position, Quaternion.identity);
        _enemyList.Add(enemy);
        selectedEnemy = enemyType[Random.Range(0, enemyType.Length)];
        enemy = PhotonNetwork.Instantiate(selectedEnemy, spawnPoints[7].position, Quaternion.identity);
        _enemyList.Add(enemy);
    }

    //Funcion que chequea si se terminó el juego
    public void CheckEndgame()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        bool isSomebodyAlive = false;

        foreach (var p in _playerList)
        {
            if (!p.IsDead) isSomebodyAlive = true;
        }

        if (!isSomebodyAlive) photonView.RPC("EndGame", RpcTarget.All);
    }

    [PunRPC]
    void EndGame()
    {
        loseText.SetActive(true);
    }

    [PunRPC]
    void WinGame()
    {
        winText.SetActive(true);
    }
}