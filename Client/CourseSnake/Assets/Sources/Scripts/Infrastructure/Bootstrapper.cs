using UnityEngine;

public class Bootstrapper : MonoBehaviour
{
    [SerializeField] private PlayerSpawnInitiator _playerSpawnInitiator;
    [SerializeField] private SnakeDieReaction _snakeDieReaction;
    [SerializeField] private CameraMovement _camera;
    [SerializeField] private MapInfo _mapInfo;
    [SerializeField] private LeaderBoardView _leaderBoardView;
    [SerializeField] private AppleSpawnInitiator _appleSpawnInitiator;

    private async void Awake()
    {
        StateHandlerRoom stateHandlerRoom = StateHandlerRoom.Instance;
        LobbyRoomHandler lobbyRoomHandler = LobbyRoomHandler.Instance;

        _mapInfo.Init(lobbyRoomHandler);

        AppleFactory appleFactory = new();
        LeaderBoardPlayerDataFactory leaderBoardPlayerDataFactory = new();

        SnakeFactory snakeFactory = new();
        snakeFactory.Init(_camera, _appleSpawnInitiator);

        Vector2 activeArea = new(20, 20);

        _playerSpawnInitiator.SetArea(activeArea);
        _appleSpawnInitiator.SetSpawnRange(activeArea);

        ISnakeSpawnHandler snakeSpawnHandler;

        try
        {
            if (await stateHandlerRoom.JoinOrCreateAny() == false)
                return;

            snakeFactory.InitStateHandlerRoom(stateHandlerRoom);

            MapMultiplayerHandler mapMultiplayerHandler = new GameObject("MapMultiplayerHandler").AddComponent<MapMultiplayerHandler>();
            MultiplayerUserHandler multiplayerUserHandler = new GameObject("MultiplayerUserHandler").AddComponent<MultiplayerUserHandler>();
            multiplayerUserHandler.Init(mapMultiplayerHandler, stateHandlerRoom, _playerSpawnInitiator, snakeFactory);
            _snakeDieReaction.Init(multiplayerUserHandler);

            MultiplayerAppleSpawnHandler appleSpawnHandler = new(
                stateHandlerRoom, 
                _appleSpawnInitiator, 
                appleFactory);

            snakeSpawnHandler = multiplayerUserHandler;
            Debug.Log("Start Multiplayer");
        }
        catch
        {
            SinglPlayerUserHandler singlPlayerUserHandler = new(_playerSpawnInitiator, snakeFactory);
            _snakeDieReaction.Init(singlPlayerUserHandler);

            snakeSpawnHandler = singlPlayerUserHandler;
            Debug.Log("Start Singlplayer");
        }

        _appleSpawnInitiator.InitRandomApples(20);
        _leaderBoardView.Init(snakeSpawnHandler, leaderBoardPlayerDataFactory);
        _playerSpawnInitiator.SetMenuState(true);
    }
}
