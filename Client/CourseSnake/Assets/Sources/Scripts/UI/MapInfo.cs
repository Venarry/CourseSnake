using UnityEngine;
using TMPro;
using System;

public class MapInfo : MonoBehaviour
{
    [SerializeField] private TMP_Text _onlinePlayers;
    [SerializeField] private TMP_Text _bestScore;

    private LobbyRoomHandler _lobbyRoomHandler;
    private BestScoreHandler _bestScoreHandler;
    private bool _isInitialized;

    public void Init(LobbyRoomHandler lobbyRoomHandler, BestScoreHandler bestScoreHandler)
    {
        gameObject.SetActive(false);

        _lobbyRoomHandler = lobbyRoomHandler;
        _bestScoreHandler = bestScoreHandler;
        _isInitialized = true;

        gameObject.SetActive(true);
    }

    public void OnEnable()
    {
        if (_isInitialized == false)
            return;

        _lobbyRoomHandler.PlayersCountChanged += OnPlayersCountChange;
        _bestScoreHandler.BestScoreChanged += OnBestScoreChange;
    }

    public void OnDisable()
    {
        if (_isInitialized == false)
            return;

        _lobbyRoomHandler.PlayersCountChanged -= OnPlayersCountChange;
        _bestScoreHandler.BestScoreChanged -= OnBestScoreChange;
    }

    private void OnBestScoreChange(float score)
    {
        _bestScore.text = $"Лучший результат:\n{Math.Round(score, 1)}";
    }

    private void OnPlayersCountChange(int value)
    {
        _onlinePlayers.text = $"Игроков онлайн: {value}";
    }
}
