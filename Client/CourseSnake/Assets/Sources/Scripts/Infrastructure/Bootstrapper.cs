using UnityEngine;

public class Bootstrapper : MonoBehaviour
{
    private async void Awake()
    {
        StateHandlerRoom stateHandlerRoom = StateHandlerRoom.Instance;
        if (await stateHandlerRoom.JoinOrCreate() == false)
            return;

        SnakeFactory snakeFactory = new();
        snakeFactory.Init(stateHandlerRoom);

        PlayerSpawnPointInitiator playerSpawner = new(10, 10);

        MapMultiplayerHandler mapMultiplayerHandler = new GameObject("MapMultiplayerHandler").AddComponent<MapMultiplayerHandler>();
        MultiplayerUserHandler multiplayerUserHandler = new GameObject("MultiplayerUserHandler").AddComponent<MultiplayerUserHandler>();
        multiplayerUserHandler.Init(mapMultiplayerHandler, stateHandlerRoom, playerSpawner, snakeFactory);

        playerSpawner.InitPlayerSpawnPoint();
    }
}
