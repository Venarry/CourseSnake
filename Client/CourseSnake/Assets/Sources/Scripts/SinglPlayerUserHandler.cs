using System;
using UnityEngine;

public class SinglPlayerUserHandler
{
    private PlayerSpawnInitiator _playerSpawnInitiator;
    private readonly SnakeFactory _snakeFactory;

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
        _snakeFactory.Create(position, name, color, false);
    }
}
