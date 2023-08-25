using System;
using UnityEngine;

public class SnakeScorePresenter
{
    private readonly SnakeScoreModel _snakeScoreModel;
    private readonly SnakeBodyParts _snakeBodyParts;

    public SnakeScorePresenter(SnakeScoreModel snakeScoreModel, SnakeBodyParts snakeBodyParts)
    {
        _snakeScoreModel = snakeScoreModel;
        _snakeBodyParts = snakeBodyParts;
    }

    public void AddScore(float value)
    {
        _snakeScoreModel.AddScore(value);
    }

    public void Enable()
    {
        _snakeScoreModel.ScoreChanged += OnScoreChanged;
    }

    public void Disable()
    {
        _snakeScoreModel.ScoreChanged -= OnScoreChanged;
    }

    private void OnScoreChanged(float score)
    {
        Debug.Log(score);
        int bodyCounts = (int)Math.Floor(score);
        _snakeBodyParts.SetBodyPart(bodyCounts);
    }
}
