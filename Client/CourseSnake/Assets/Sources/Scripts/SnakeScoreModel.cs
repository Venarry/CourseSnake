using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeScoreModel
{
    private float _score;

    public event Action<float> ScoreChanged;

    public void AddScore(float value)
    {
        if (value <= 0)
            return;

        _score += value;
        ScoreChanged?.Invoke(_score);
    }

    public void RemoveScore(float value)
    {
        if (value >= 0)
            return;

        if(_score <= 0)
            return;

        _score -= value;

        if(_score < 0)
            _score = 0;
    }

    public void SetScore(float value)
    {
        if (value <= 0)
            return;

        _score = value;
        ScoreChanged?.Invoke(_score);
    }


}
