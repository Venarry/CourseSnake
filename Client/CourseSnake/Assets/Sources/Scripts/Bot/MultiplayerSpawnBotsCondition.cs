public class MultiplayerSpawnBotsCondition
{
    private readonly LobbyRoomHandler _lobbyRoomHandler;
    private readonly PlayerSpawnInitiator _spawnInitiator;
    private readonly MultiplayerUsersHandler _multiplayerUsersHandler;

    public MultiplayerSpawnBotsCondition(LobbyRoomHandler lobbyRoomHandler,
        PlayerSpawnInitiator playerSpawnInitiator,
        MultiplayerUsersHandler multiplayerUsersHandler)
    {
        _lobbyRoomHandler = lobbyRoomHandler;
        _spawnInitiator = playerSpawnInitiator;
        _multiplayerUsersHandler = multiplayerUsersHandler;
        _multiplayerUsersHandler.SetSpawnBotsState(false);

        _lobbyRoomHandler.PlayersCountChanged += OnPlayersCountChange;
    }

    private void OnPlayersCountChange(int count)
    {
        if (count > 1)
        {
            _multiplayerUsersHandler.SetSpawnBotsState(false);
            _multiplayerUsersHandler.DestroyBots();
        }
        else if (_multiplayerUsersHandler.CanSpawnBots == false)
        {
            _multiplayerUsersHandler.SetSpawnBotsState(true);
            _spawnInitiator.InitBots(GameConfig.BotsCount);
        }
    }
}
