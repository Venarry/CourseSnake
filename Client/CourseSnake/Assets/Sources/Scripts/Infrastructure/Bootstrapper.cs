using UnityEngine;

public class Bootstrapper : MonoBehaviour
{
    [SerializeField] private PlayerSpawnInitiator _playerSpawnInitiator;
    [SerializeField] private SnakeDieReaction _snakeDieReaction;
    [SerializeField] private CameraMovement _camera;
    [SerializeField] private MapInfo _mapInfo;

    private async void Awake()
    {
        StateHandlerRoom stateHandlerRoom = StateHandlerRoom.Instance;
        LobbyRoomHandler lobbyRoomHandler = LobbyRoomHandler.Instance;

        _mapInfo.Init(lobbyRoomHandler);

        AppleFactory appleFactory = new();

        SnakeFactory snakeFactory = new();
        snakeFactory.Init(_camera, appleFactory);

        _playerSpawnInitiator.SetArea(10, 10);

        try
        {
            if (await stateHandlerRoom.JoinOrCreate() == false)
                return;

            snakeFactory.InitStateHandlerRoom(stateHandlerRoom);

            MapMultiplayerHandler mapMultiplayerHandler = new GameObject("MapMultiplayerHandler").AddComponent<MapMultiplayerHandler>();
            MultiplayerUserHandler multiplayerUserHandler = new GameObject("MultiplayerUserHandler").AddComponent<MultiplayerUserHandler>();
            multiplayerUserHandler.Init(mapMultiplayerHandler, stateHandlerRoom, _playerSpawnInitiator, snakeFactory);
            _snakeDieReaction.Init(multiplayerUserHandler);
            Debug.Log("Start Multiplayer");
        }
        catch
        {
            Debug.Log("Start Singlplayer");
            SinglPlayerUserHandler singlPlayerUserHandler = new(_playerSpawnInitiator, snakeFactory);
            _snakeDieReaction.Init(singlPlayerUserHandler);
        }

        _playerSpawnInitiator.SetMenuState(true);
    }
}
