using UnityEngine;
using TMPro;

public class LeaderBoardPlayer : MonoBehaviour
{
    [SerializeField] private TMP_Text _leaderPosition;
    [SerializeField] private TMP_Text _login;
    [SerializeField] private TMP_Text _score;

    public void Init(string login)
    {
        _login.text = login;
        _score.text = "0";
    }

    public void UpdateScore(float value)
    {
        _score.text = value.ToString();
    }

    public void UpdateLeaderPosition(int value)
    {
        _leaderPosition.text = value.ToString();
    }
}
