using System;
using UnityEngine;

public class BestScoreHandler
{
    private const string BestScore = "BestScore";

    private ISnakeHandler _snakeHandler;
    private float _bestScore;

    public event Action<float> BestScoreChanged; 

    public void Init(ISnakeHandler snakeHandler)
    {
        if(_snakeHandler != null)
        {
            _snakeHandler.PlayerSpawned -= OnPlayerSpawn;
        }

        _snakeHandler = snakeHandler;
        _snakeHandler.PlayerSpawned += OnPlayerSpawn;
    }

    public void LoadScore()
    {
        _bestScore = PlayerPrefs.GetFloat(BestScore);
        BestScoreChanged?.Invoke(_bestScore);
    }

    private void OnPlayerSpawn(SnakeView snake)
    {
        snake.ScoreChanged += OnScoreChange;
        snake.Destroyed += OnSnakeDestroy;
    }

    private void OnSnakeDestroy(SnakeView snake)
    {
        snake.ScoreChanged -= OnScoreChange;
        snake.Destroyed -= OnSnakeDestroy;
    }

    private void OnScoreChange(SnakeView snake, float score)
    {
        if(score > _bestScore)
        {
            _bestScore = score;
            PlayerPrefs.SetFloat(BestScore, _bestScore);
            BestScoreChanged?.Invoke(_bestScore);
        }
    }
}
