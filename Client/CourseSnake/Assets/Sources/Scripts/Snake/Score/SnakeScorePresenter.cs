using System;
using UnityEngine;

public class SnakeScorePresenter
{
    private readonly SnakeScoreModel _snakeScoreModel;
    private readonly SnakeBodyParts _snakeBodyParts;
    private readonly SnakeMovement _snakeMovement;

    public event Action<float> ScoreChanged;

    public float Score => _snakeScoreModel.Score;

    public SnakeScorePresenter(
        SnakeScoreModel snakeScoreModel, 
        SnakeBodyParts snakeBodyParts, 
        SnakeMovement snakeMovement)
    {
        _snakeScoreModel = snakeScoreModel;
        _snakeBodyParts = snakeBodyParts;
        _snakeMovement = snakeMovement;
    }

    public void AddScore(float value)
    {
        _snakeScoreModel.AddScore(value);
    }

    public void SetScore(float value)
    {
        _snakeScoreModel.SetScore(value);
    }

    public void Enable()
    {
        _snakeScoreModel.ScoreChanged += OnScoreChanged;
        _snakeMovement.BoostUsed += OnBoostUsed;
    }

    public void Disable()
    {
        _snakeScoreModel.ScoreChanged -= OnScoreChanged;
        _snakeMovement.BoostUsed -= OnBoostUsed;
    }

    private void OnBoostUsed(float speed)
    {
        float scoreReduceMultiplier = 0.05f;
        _snakeScoreModel.RemoveScore(speed * scoreReduceMultiplier);
    }

    private void OnScoreChanged(float score)
    {
        int bodyCounts = (int)Math.Floor(score);
        _snakeBodyParts.SetBodyPart(bodyCounts);

        ScoreChanged?.Invoke(score);
    }
}
