using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeScoreModel
{
    public float Score { get; private set; }

    public event Action<float> ScoreChanged;

    public void AddScore(float value)
    {
        if (value <= 0)
            return;

        Score += value;
        ScoreChanged?.Invoke(Score);
    }

    public void RemoveScore(float value)
    {
        if (value <= 0)
            return;

        if(Score <= 0)
            return;

        Score -= value;

        if(Score < 0)
            Score = 0;

        ScoreChanged?.Invoke(Score);
    }

    public void SetScore(float value)
    {
        if (value <= 0)
            return;

        Score = value;
        ScoreChanged?.Invoke(Score);
    }


}
