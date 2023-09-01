using System;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBoardView : MonoBehaviour
{
    [SerializeField] private Transform _parent;

    private ISnakeSpawnHandler _spawnHandler;
    private LeaderBoardPlayerDataFactory _playerDataFactory;
    private readonly Dictionary<string, LeaderBoardPlayer> _players = new();
    private bool _isInitialized;

    public void Init(ISnakeSpawnHandler snakeSpawnHandler, LeaderBoardPlayerDataFactory playerDataFactory)
    {
        gameObject.SetActive(false);

        _spawnHandler = snakeSpawnHandler;
        _playerDataFactory = playerDataFactory;
        _isInitialized = true;

        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        if (_isInitialized == false)
            return;

        _spawnHandler.SnakeSpawned += OnSnakeSpawn;
        _spawnHandler.SnakeRemoved += OnSnakeRemoved;
    }

    private void OnSnakeSpawn(SnakeView snake)
    {
        LeaderBoardPlayer leaderBoardPlayer = _playerDataFactory.Create(_parent, snake.Login);
        _players.Add(snake.Id, leaderBoardPlayer);

        snake.ScoreChanged += OnScoreChange;
        snake.Destroyed += () => snake.ScoreChanged -= OnScoreChange;
    }

    private void OnSnakeRemoved(string key)
    {
        Destroy(_players[key].gameObject);
        _players.Remove(key);
    }

    private void OnScoreChange(string key, float value)
    {
        _players[key].UpdateScore(value);
    }

}
