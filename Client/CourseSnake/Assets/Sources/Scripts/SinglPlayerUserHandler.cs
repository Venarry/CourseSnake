using System;
using UnityEngine;

public class SinglPlayerUserHandler : ISnakeSpawnHandler
{
    private PlayerSpawnInitiator _playerSpawnInitiator;
    private readonly SnakeFactory _snakeFactory;

    public event Action<SnakeView> SnakeSpawned;

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
        SnakeView snake = _snakeFactory.CreatePlayer(position, name, color, false);

        SnakeSpawned?.Invoke(snake);
    }
}
