using System;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerUserHandler : MonoBehaviour
{
    private readonly Dictionary<string, SnakeView> _enemys = new();
    private MapMultiplayerHandler _mapMultiplayerHandler;
    private StateHandlerRoom _stateHandlerRoom;
    private PlayerSpawnInitiator _playerSpawner;
    private SnakeFactory _snakeFactory;
    private bool _isInitialized;

    public void Init(MapMultiplayerHandler mapMultiplayerHandler,
        StateHandlerRoom stateHandlerRoom,
        PlayerSpawnInitiator playerSpawner,
        SnakeFactory snakeFactory)
    {
        gameObject.SetActive(false);

        _mapMultiplayerHandler = mapMultiplayerHandler;
        _stateHandlerRoom = stateHandlerRoom;
        _playerSpawner = playerSpawner;
        _snakeFactory = snakeFactory;
        _isInitialized = true;

        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        if (_isInitialized == false)
            return;

        _playerSpawner.PlayerSpawnInited += OnPlayerInit;
        _mapMultiplayerHandler.PlayerJoined += OnPlayerJoin;
        _mapMultiplayerHandler.EnemyJoined += OnEnemyJoin;
        _mapMultiplayerHandler.EnemyLeaved += OnEnemyLeave;
    }

    private void OnDisable()
    {
        if (_isInitialized == false)
            return;

        _playerSpawner.PlayerSpawnInited -= OnPlayerInit;
        _mapMultiplayerHandler.PlayerJoined -= OnPlayerJoin;
        _mapMultiplayerHandler.EnemyJoined -= OnEnemyJoin;
        _mapMultiplayerHandler.EnemyLeaved -= OnEnemyLeave;
    }

    private void OnPlayerJoin(string key, Player player)
    {
        Vector3 spawnPosition = new(player.Position.x, player.Position.y, player.Position.z);
        Color snakeColor = new(player.Color.x, player.Color.y, player.Color.z);
        _snakeFactory.Create(spawnPosition, player.Name, snakeColor, true, player);
    }

    private void OnEnemyJoin(string key, Player player)
    {
        SnakeView enemy = _snakeFactory.CreateEnemy(player);
        _enemys.Add(key, enemy);
    }

    private void OnPlayerInit(Vector3 position, string name, Color color)
    {
        MyVector3 myVector3 = new(position);
        MyVector3 snakeColor = new(color.r, color.g, color.b);

        Dictionary<string, object> data = new()
        {
            { "Position", myVector3 },
            { "Name", name },
            { "Color", snakeColor },
        };

        _stateHandlerRoom.SendPlayerData("PlayerSpawned", data);
    }

    private void OnEnemyLeave(string key)
    {
        if (_enemys.ContainsKey(key) == false)
            return;

        Destroy(_enemys[key].gameObject);
        _enemys.Remove(key);
    }
}
