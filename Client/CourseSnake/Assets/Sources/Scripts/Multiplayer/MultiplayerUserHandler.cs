using System;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerUserHandler : MonoBehaviour
{
    private readonly Dictionary<string, SnakeView> _enemys = new();
    private MapMultiplayerHandler _mapMultiplayerHandler;
    private StateHandlerRoom _stateHandlerRoom;
    private PlayerSpawnPointInitiator _playerSpawner;
    private SnakeFactory _snakeFactory;
    private CameraMovement _mainCamera;
    private bool _isInitialized;

    public void Init(MapMultiplayerHandler mapMultiplayerHandler,
        StateHandlerRoom stateHandlerRoom,
        PlayerSpawnPointInitiator playerSpawner,
        SnakeFactory snakeFactory,
        CameraMovement mainCamera)
    {
        gameObject.SetActive(false);

        _mapMultiplayerHandler = mapMultiplayerHandler;
        _stateHandlerRoom = stateHandlerRoom;
        _playerSpawner = playerSpawner;
        _snakeFactory = snakeFactory;
        _mainCamera = mainCamera;
        _isInitialized = true;

        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        if (_isInitialized == false)
            return;

        _playerSpawner.PlayerPointInited += OnPlayerSpawnPointInit;
        _mapMultiplayerHandler.PlayerJoined += OnPlayerJoin;
        _mapMultiplayerHandler.EnemyJoined += OnEnemyJoin;
        _mapMultiplayerHandler.EnemyLeaved += OnEnemyLeave;
    }

    private void OnDisable()
    {
        if (_isInitialized == false)
            return;

        _playerSpawner.PlayerPointInited -= OnPlayerSpawnPointInit;
        _mapMultiplayerHandler.PlayerJoined -= OnPlayerJoin;
        _mapMultiplayerHandler.EnemyJoined -= OnEnemyJoin;
        _mapMultiplayerHandler.EnemyLeaved -= OnEnemyLeave;
    }

    private void OnPlayerJoin(string key, Player player)
    {
        Vector3 spawnPosition = new(player.Position.x, player.Position.y, player.Position.z);
        _snakeFactory.Create(spawnPosition, _mainCamera, true, player);
    }

    private void OnEnemyJoin(string key, Player player)
    {
        SnakeView enemy = _snakeFactory.CreateEnemy(player);
        _enemys.Add(key, enemy);
    }

    private void OnPlayerSpawnPointInit(Vector3 position)
    {
        MyVector3 myVector3 = new(position);
        _stateHandlerRoom.SendPlayerData("PlayerSpawned", myVector3);
    }

    private void OnEnemyLeave(string key)
    {
        if (_enemys.ContainsKey(key) == false)
            return;

        Destroy(_enemys[key].gameObject);
        _enemys.Remove(key);
    }
}
