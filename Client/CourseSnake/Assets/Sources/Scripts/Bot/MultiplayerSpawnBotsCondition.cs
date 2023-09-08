using System;
using UnityEngine;
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
        //OnPlayersCountChange(_lobbyRoomHandler.PlayersCount);

        //_lobbyRoomHandler.PlayersCountChanged += OnPlayersCountChange;
        _multiplayerUsersHandler.SnakeSpawned += OnSnakesChange;
        _multiplayerUsersHandler.SnakeRemoved += OnSnakesChange;
    }

    private void OnSnakesChange(SnakeView snake)
    {
        if (_multiplayerUsersHandler.PlayerHasSpawn == false)
        {
            if(_multiplayerUsersHandler.CanSpawnBots == true)
            {
                _multiplayerUsersHandler.SetSpawnBotsState(false);
                _multiplayerUsersHandler.DestroyBots();
            }

            return;
        }

        int snakeCount = _multiplayerUsersHandler.SnakeCount;

        if (snakeCount > 1)
        {
            Debug.Log("dB");
            _multiplayerUsersHandler.SetSpawnBotsState(false);
            _multiplayerUsersHandler.DestroyBots();
        }
        else if (_multiplayerUsersHandler.CanSpawnBots == false)
        {
            _multiplayerUsersHandler.SetSpawnBotsState(true);
            _spawnInitiator.InitBots(GameConfig.BotsCount);
        }
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
