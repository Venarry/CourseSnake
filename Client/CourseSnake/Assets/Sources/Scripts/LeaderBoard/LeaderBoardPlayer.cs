using UnityEngine;
using TMPro;

public class LeaderBoardPlayer : MonoBehaviour
{
    [SerializeField] private TMP_Text _leaderPosition;
    [SerializeField] private TMP_Text _login;
    [SerializeField] private TMP_Text _score;

    public int LeaderPosition { get; private set; }
    public float Score { get; private set; }

    public void Init(string login)
    {
        _login.text = login;
        _score.text = "0";
    }

    public void UpdateScore(float value)
    {
        _score.text = value.ToString();
        Score = value;
    }

    public void UpdateLeaderPosition(int value)
    {
        _leaderPosition.text = value.ToString();
        LeaderPosition = value;
    }
}
