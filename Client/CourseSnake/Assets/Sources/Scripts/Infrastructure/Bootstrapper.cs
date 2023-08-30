using UnityEngine;

public class Bootstrapper : MonoBehaviour
{
    private async void Awake()
    {
        StateHandlerRoom stateHandlerRoom = StateHandlerRoom.Instance;

        CameraFactory cameraFactory = new();
        CameraMovement cameraMovement = cameraFactory.Create();

        SnakeFactory snakeFactory = new();
        snakeFactory.InitCamera(cameraMovement);

        PlayerSpawnInitiator playerSpawner = new(10, 10);

        try
        {
            if (await stateHandlerRoom.JoinOrCreate() == false)
                return;

            snakeFactory.InitStateHandlerRoom(stateHandlerRoom);

            MapMultiplayerHandler mapMultiplayerHandler = new GameObject("MapMultiplayerHandler").AddComponent<MapMultiplayerHandler>();
            MultiplayerUserHandler multiplayerUserHandler = new GameObject("MultiplayerUserHandler").AddComponent<MultiplayerUserHandler>();
            multiplayerUserHandler.Init(mapMultiplayerHandler, stateHandlerRoom, playerSpawner, snakeFactory);
        }
        catch
        {
            Debug.Log("StartSinglpalyer");
            SinglPlayerUserHandler singlPlayerUserHandler = new(playerSpawner, snakeFactory);
        }
        
        playerSpawner.InitPlayerSpawnPoint();
    }
}
