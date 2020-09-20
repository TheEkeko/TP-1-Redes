using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks
{
    //Constantes
    const int _minPlayersToStartGame = 1;
    const int _minEnemiesToStartWave = 1;
    const int _maxWaves = 4;

    //Variables
    [SerializeField] List<GameObject> _playerList;
    [SerializeField] Transform[] spawnPoints;
    bool _gameStart;
    float _timer;
    List<GameObject> _enemyList;
    int _actualWave;

    //Referencias a la ubicación de los enemigos
    const string _slimePrefab = "Prefabs/EnemyPrefabs/Slime";
    const string _gremlingPrefab = "Prefabs/EnemyPrefabs/Gremling";
    const string _ghostPrefab = "Prefabs/EnemyPrefabs/Ghost";
    const string _skeletonPrefab = "Prefabs/EnemyPrefabs/Skeleton";

    private void Awake()
    {
        //Si no es masterclient, chau
        if (!PhotonNetwork.IsMasterClient) return;

        //inicializo las variables
        _gameStart = false;
        _timer = 10f;
        _enemyList = new List<GameObject>();
        _playerList = new List<GameObject>();
        _actualWave = 0;
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
            if (_enemyList.Count < _minEnemiesToStartWave)
            {
                var enemy = PhotonNetwork.Instantiate(_slimePrefab, spawnPoints[0].position, Quaternion.identity);
                _enemyList.Add(enemy);
                enemy = PhotonNetwork.Instantiate(_gremlingPrefab, spawnPoints[1].position, Quaternion.identity);
                _enemyList.Add(enemy);
                enemy = PhotonNetwork.Instantiate(_ghostPrefab, spawnPoints[2].position, Quaternion.identity);
                _enemyList.Add(enemy);
                enemy = PhotonNetwork.Instantiate(_skeletonPrefab, spawnPoints[3].position, Quaternion.identity);
                _enemyList.Add(enemy);
                _actualWave++;
            }

            //termino el juego si mataron a todos los enemigos en todas las rondas
            if (_actualWave == _maxWaves && _enemyList.Count == 0) EndGame();
        }
    }

    //Funcion que agrega al jugador en la lista de jugadores
    public void AddPlayerToList(GameObject player)
    {
        _playerList.Add(player);
    }

    //Devuelve la lista de jugadores
    public List<GameObject> GetPlayerList()
    {
        return _playerList;
    }

    //Acceso a la lista de enemigos
    public List<GameObject> EnemyList
    {
        get { return _enemyList; }
    }

    void EndGame()
    {

    }
}