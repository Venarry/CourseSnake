using System;
using System.Collections.Generic;
using UnityEngine;

public class SinglePlayerUsersHandler : ISnakeHandler
{
    private PlayerSpawnInitiator _playerSpawnInitiator;
    private readonly SnakeFactory _snakeFactory;

    private readonly List<SnakeView> _bots = new();

    private int _botId = 1;

    public SinglePlayerUsersHandler(PlayerSpawnInitiator playerSpawnInitiator, SnakeFactory snakeFactory)
    {
        _playerSpawnInitiator = playerSpawnInitiator;
        _snakeFactory = snakeFactory;

        _playerSpawnInitiator.PlayerSpawnInited += OnPlayerInited;
        _playerSpawnInitiator.BotSpawnInited += OnBotInited;
    }

    ~SinglePlayerUsersHandler()
    {
        _playerSpawnInitiator.PlayerSpawnInited -= OnPlayerInited;
        _playerSpawnInitiator.BotSpawnInited -= OnBotInited;
    }

    public event Action<SnakeView> PlayerSpawned;
    public event Action<SnakeView> BotSpawned;
    public event Action<SnakeView> SnakeSpawned;
    public event Action<SnakeView> SnakeRemoved;

    public int SnakeCount => 1;
    public int BotsCount => _bots.Count;
    public bool CanSpawnBots => true;

    private void OnPlayerInited(Vector3 position, string name, Color color)
    {
        string id = "0";
        SnakeView snake = _snakeFactory.CreatePlayer(position, name, color, id, false);
        snake.Destroyed += OnPlayerDestroy;

        PlayerSpawned?.Invoke(snake);
        SnakeSpawned?.Invoke(snake);
    }

    private void OnPlayerDestroy(SnakeView snake)
    {
        SnakeRemoved?.Invoke(snake);
    }

    private void OnBotInited(Vector3 position, string name, Color color)
    {
        string id = _botId++.ToString();
        SnakeView snake = _snakeFactory.CreateBot(position, color, id, name);
        _bots.Add(snake);
        snake.Destroyed += OnBotDestroy;

        BotSpawned?.Invoke(snake);
        SnakeSpawned?.Invoke(snake);
    }

    private void OnBotDestroy(SnakeView snake)
    {
        snake.Destroyed -= OnBotDestroy;
        _bots.Remove(snake);

        SnakeRemoved?.Invoke(snake);
    }
}
