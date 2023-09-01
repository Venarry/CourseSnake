using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LeaderBoardView : MonoBehaviour
{
    [SerializeField] private Transform _parent;

    private ISnakeSpawnHandler _spawnHandler;
    private LeaderBoardPlayerDataFactory _playerDataFactory;
    private readonly Dictionary<string, LeaderBoardPlayer> _players = new();
    private List<LeaderBoardPlayer> _leaders = new();
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
        _leaders.Add(leaderBoardPlayer);

        snake.ScoreChanged += OnScoreChange;
        //snake.Destroyed += () => snake.ScoreChanged -= OnScoreChange;
        RefreshLeaderBoard();
    }

    private void OnSnakeRemoved(string key)
    {
        LeaderBoardPlayer currentPlayer = _players[key];

        _players.Remove(key);
        _leaders.Remove(currentPlayer);
        Destroy(currentPlayer.gameObject);
        RefreshLeaderBoard();
    }

    private void OnScoreChange(string key, float value)
    {
        _players[key].UpdateScore(value);
        RefreshLeaderBoard();
    }

    private void RefreshLeaderBoard()
    {
        _leaders = _leaders.OrderByDescending(currentLeader => currentLeader.Score).ToList();

        for (int i = 0; i < _leaders.Count; i++)
        {
            _leaders[i].UpdateLeaderPosition(i + 1);
            _leaders[i].transform.SetSiblingIndex(i);
        }
    }
}
