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

        _playerSpawnInitiator.PlayerPointInited += OnPlayerInited;
    }

    ~SinglPlayerUserHandler()
    {
        _playerSpawnInitiator.PlayerPointInited -= OnPlayerInited;
    }

    private void OnPlayerInited(Vector3 position)
    {
        _snakeFactory.Create(position, false);
    }
}
