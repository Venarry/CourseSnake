using System;
using System.Collections.Generic;

public class LeaderBoardModel
{
    private readonly Dictionary<string, LeaderBoardData> _leaderBoard = new();

    public event Action<string, LeaderBoardData> PlayerAdded;
    public event Action<string, int> PlayerUpdated;
    public event Action<string> PlayerRemoved;

    public void AddPlayer(string key, LeaderBoardData data)
    {
        if (_leaderBoard.ContainsKey(key))
            return;

        _leaderBoard.Add(key, data);
        PlayerAdded?.Invoke(key, data);
    }

    public void UpdatePlayerScore(string key, int score)
    {
        if (_leaderBoard.ContainsKey(key) == false)
            return;

        _leaderBoard[key].SetScore(score);
        PlayerUpdated?.Invoke(key, score);
    }

    public void RemovePlayer(string key)
    {
        if (_leaderBoard.ContainsKey(key) == false)
            return;

        _leaderBoard.Remove(key);
        PlayerRemoved?.Invoke(key);
    }
}
