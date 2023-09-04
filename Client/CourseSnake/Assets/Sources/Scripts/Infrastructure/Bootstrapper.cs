using UnityEngine;

public class Bootstrapper : MonoBehaviour
{
    [SerializeField] private PlayerSpawnInitiator _playerSpawnInitiator;
    [SerializeField] private SnakeDieReaction _snakeDieReaction;
    [SerializeField] private CameraMovement _camera;
    [SerializeField] private MapInfo _mapInfo;
    [SerializeField] private LeaderBoardView _leaderBoardView;
    [SerializeField] private AppleSpawnInitiator _appleSpawnInitiator;
    [SerializeField] private MiniMapView _miniMapView;

    private async void Awake()
    {
        StateHandlerRoom stateHandlerRoom = StateHandlerRoom.Instance;
        LobbyRoomHandler lobbyRoomHandler = LobbyRoomHandler.Instance;

        _mapInfo.Init(lobbyRoomHandler);

        AppleFactory appleFactory = new();
        LeaderBoardPlayerDataFactory leaderBoardPlayerDataFactory = new();

        SnakeFactory snakeFactory = new();
        

        Vector2 mapSize = new(65, 65);

        _playerSpawnInitiator.SetArea(mapSize);
        _appleSpawnInitiator.SetSpawnRange(mapSize);

        try
        {
            if (await stateHandlerRoom.JoinOrCreateAny() == false)
                return;

            snakeFactory.InitStateHandlerRoom(stateHandlerRoom);

            MultiplayerAppleSpawnHandler appleSpawnHandler = new(
                stateHandlerRoom,
                _appleSpawnInitiator,
                appleFactory);

            snakeFactory.Init(_camera, _appleSpawnInitiator, appleSpawnHandler);

            MapMultiplayerHandler mapMultiplayerHandler = new GameObject("MapMultiplayerHandler").AddComponent<MapMultiplayerHandler>();
            MultiplayerUsersHandler multiplayerUsersHandler = new GameObject("MultiplayerUserHandler").AddComponent<MultiplayerUsersHandler>();

            multiplayerUsersHandler.Init(mapMultiplayerHandler, 
                stateHandlerRoom, 
                _playerSpawnInitiator, 
                snakeFactory,
                lobbyRoomHandler);

            _snakeDieReaction.Init(multiplayerUsersHandler);

            _leaderBoardView.Init(multiplayerUsersHandler, leaderBoardPlayerDataFactory);
            _miniMapView.Init(multiplayerUsersHandler, mapSize);

            _appleSpawnInitiator.Init(multiplayerUsersHandler);
            _appleSpawnInitiator.InitRandomApples(20);

            MultiplayerSpawnBotsCondition multiplayerSpawnBotsCondition = new(
                lobbyRoomHandler, 
                _playerSpawnInitiator, 
                multiplayerUsersHandler);

            Debug.Log("Start Multiplayer");
        }
        catch
        {
            SinglePlayerAppleHandler singlePlayerAppleHandler = new(_appleSpawnInitiator, appleFactory);

            snakeFactory.Init(_camera, _appleSpawnInitiator, singlePlayerAppleHandler);

            SinglePlayerUsersHandler singlPlayerUserHandler = new(_playerSpawnInitiator, snakeFactory);
            _snakeDieReaction.Init(singlPlayerUserHandler);

            _appleSpawnInitiator.Init(singlPlayerUserHandler);
            _leaderBoardView.Init(singlPlayerUserHandler, leaderBoardPlayerDataFactory);
            _miniMapView.Init(singlPlayerUserHandler, mapSize);

            _appleSpawnInitiator.InitRandomApples(20);
            _playerSpawnInitiator.InitBots(GameConfig.BotsCount);

            Debug.Log("Start Singlplayer");
        }

        _playerSpawnInitiator.SetMenuState(true);
    }
}
