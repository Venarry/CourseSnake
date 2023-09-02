using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LeaderBoardView : MonoBehaviour
{
    [SerializeField] private Transform _parent;

    private ISnakeHandler _spawnHandler;
    private LeaderBoardPlayerDataFactory _playerDataFactory;
    private readonly Dictionary<string, LeaderBoardPlayer> _leaders = new();
    private List<LeaderBoardPlayer> _leadersInBoard = new();
    private bool _isInitialized;

    public void Init(ISnakeHandler snakeSpawnHandler, LeaderBoardPlayerDataFactory playerDataFactory)
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
        _leaders.Add(snake.Id, leaderBoardPlayer);
        _leadersInBoard.Add(leaderBoardPlayer);

        snake.ScoreChanged += OnScoreChange;
        RefreshLeaderBoard();
    }

    private void OnSnakeRemoved(SnakeView snake)
    {
        string snakeId = snake.Id;
        LeaderBoardPlayer currentPlayer = _leaders[snakeId];

        _leaders.Remove(snakeId);
        _leadersInBoard.Remove(currentPlayer);

        snake.ScoreChanged -= OnScoreChange;
        Destroy(currentPlayer.gameObject);
        RefreshLeaderBoard();
    }

    private void OnScoreChange(SnakeView snake, float value)
    {
        _leaders[snake.Id].UpdateScore(value);
        RefreshLeaderBoard();
    }

    private void RefreshLeaderBoard()
    {
        _leadersInBoard = _leadersInBoard.OrderByDescending(currentLeader => currentLeader.Score).ToList();

        for (int i = 0; i < _leadersInBoard.Count; i++)
        {
            _leadersInBoard[i].UpdateLeaderPosition(i + 1);
            _leadersInBoard[i].transform.SetSiblingIndex(i);
        }
    }
}
