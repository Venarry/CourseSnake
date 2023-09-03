using System;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerUsersHandler : MonoBehaviour, ISnakeHandler
{
    private readonly Dictionary<string, SnakeView> _snakes = new();
    private readonly Dictionary<string, SnakeView> _bots = new();
    private MapMultiplayerHandler _mapMultiplayerHandler;
    private StateHandlerRoom _stateHandlerRoom;
    private PlayerSpawnInitiator _playerSpawner;
    private SnakeFactory _snakeFactory;
    private bool _isInitialized;
    private int _botId;

    public event Action<SnakeView> PlayerSpawned;
    public event Action<SnakeView> BotSpawned;
    public event Action<SnakeView> BotRemoved;
    public event Action<SnakeView> SnakeSpawned;
    public event Action<SnakeView> SnakeRemoved;

    public int SnakeCount => _snakes.Count;
    public int BotsCount => _bots.Count;

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
        _playerSpawner.BotSpawnInited += OnBotInit;
        _mapMultiplayerHandler.PlayerJoined += OnPlayerJoin;
        _mapMultiplayerHandler.EnemyJoined += OnEnemyJoin;
        _mapMultiplayerHandler.UserLeaved += OnUserLeave;
        _mapMultiplayerHandler.EnemyDead += OnUserLeave;
    }

    private void OnDisable()
    {
        if (_isInitialized == false)
            return;

        _playerSpawner.PlayerSpawnInited -= OnPlayerInit;
        _playerSpawner.BotSpawnInited -= OnBotInit;
        _mapMultiplayerHandler.PlayerJoined -= OnPlayerJoin;
        _mapMultiplayerHandler.EnemyJoined -= OnEnemyJoin;
        _mapMultiplayerHandler.UserLeaved -= OnUserLeave;
    }

    private void OnBotInit(Vector3 position, string name, Color color)
    {
        string currentBotId = _botId++.ToString();
        SnakeView snake = _snakeFactory.CreateBot(position, color, currentBotId, name);
        _bots.Add(currentBotId, snake);
        snake.Destroyed += OnBotDestroyed;

        BotSpawned?.Invoke(snake);
        SnakeSpawned?.Invoke(snake);
    }

    private void OnBotDestroyed(SnakeView snake)
    {
        snake.Destroyed -= OnBotDestroyed;
        _bots.Remove(snake.Id);
        SnakeRemoved?.Invoke(snake);
    }

    private void OnPlayerJoin(string key, Player player)
    {
        Vector3 spawnPosition = new(player.Position.x, player.Position.y, player.Position.z);
        Color snakeColor = new(player.Color.x, player.Color.y, player.Color.z);
        SnakeView snake = _snakeFactory.CreatePlayer(spawnPosition, player.Name, snakeColor, key, true, player);
        _snakes.Add(key, snake);

        PlayerSpawned?.Invoke(snake);
        SnakeSpawned?.Invoke(snake);
    }

    private void OnEnemyJoin(string key, Player player)
    {
        SnakeView enemy = _snakeFactory.CreateEnemy(player, key);
        _snakes.Add(key, enemy);

        foreach (KeyValuePair<string, SnakeView> bot in _bots)
        {
            bot.Value.Destroyed -= OnBotDestroyed;
            bot.Value.Destroy();
        }

        _bots.Clear();

        SnakeSpawned?.Invoke(enemy);
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

    private void OnUserLeave(string key)
    {
        if (_snakes.ContainsKey(key) == false)
            return;

        if(_snakes[key] != null)
            _snakes[key].Destroy();

        SnakeView removedSnake = _snakes[key];

        _snakes.Remove(key);
        SnakeRemoved?.Invoke(removedSnake);
    }
}
