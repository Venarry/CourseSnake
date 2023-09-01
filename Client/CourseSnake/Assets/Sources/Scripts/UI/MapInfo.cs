using UnityEngine;
using TMPro;

public class MapInfo : MonoBehaviour
{
    [SerializeField] private TMP_Text _onlinePlayers;

    private LobbyRoomHandler _lobbyRoomHandler;
    private bool _isInitialized;

    public void Init(LobbyRoomHandler lobbyRoomHandler)
    {
        gameObject.SetActive(false);

        _lobbyRoomHandler = lobbyRoomHandler;
        _isInitialized = true;

        gameObject.SetActive(true);
    }

    public void OnEnable()
    {
        if (_isInitialized == false)
            return;

        _lobbyRoomHandler.PlayersCountChanged += OnPlayersCountChange;
    }

    public void OnDisable()
    {
        if (_isInitialized == false)
            return;

        _lobbyRoomHandler.PlayersCountChanged -= OnPlayersCountChange;
    }

    private void OnPlayersCountChange(int value)
    {
        _onlinePlayers.text = $"Игроков онлайн: {value}";
    }
}
