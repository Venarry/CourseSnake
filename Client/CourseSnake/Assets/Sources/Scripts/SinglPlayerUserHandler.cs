using System;
using UnityEngine;

public class SinglPlayerUserHandler : ISnakeSpawnHandler
{
    private PlayerSpawnInitiator _playerSpawnInitiator;
    private readonly SnakeFactory _snakeFactory;

    public event Action<SnakeView> SnakeSpawned;
    public event Action<string> SnakeRemoved;

    public SinglPlayerUserHandler(PlayerSpawnInitiator playerSpawnInitiator, SnakeFactory snakeFactory)
    {
        _playerSpawnInitiator = playerSpawnInitiator;
        _snakeFactory = snakeFactory;

        _playerSpawnInitiator.PlayerSpawnInited += OnPlayerInited;
    }

    ~SinglPlayerUserHandler()
    {
        _playerSpawnInitiator.PlayerSpawnInited -= OnPlayerInited;
    }

    private void OnPlayerInited(Vector3 position, string name, Color color)
    {
        string id = "0";
        SnakeView snake = _snakeFactory.CreatePlayer(position, name, color, id, false);

        SnakeSpawned?.Invoke(snake);
    }
}
