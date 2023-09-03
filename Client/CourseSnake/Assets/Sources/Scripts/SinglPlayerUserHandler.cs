using System;
using UnityEngine;

public class SinglPlayerUserHandler : ISnakeHandler
{
    private PlayerSpawnInitiator _playerSpawnInitiator;
    private readonly SnakeFactory _snakeFactory;

    public event Action<SnakeView> PlayerSpawned;
    public event Action<SnakeView> BotSpawned;
    public event Action<SnakeView> BotRemoved;
    public event Action<SnakeView> SnakeSpawned;
    public event Action<SnakeView> SnakeRemoved;

    public int SnakeCount => throw new NotImplementedException();
    public int BotsCount => throw new NotImplementedException();

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

        PlayerSpawned?.Invoke(snake);
        SnakeSpawned?.Invoke(snake);
    }
}
