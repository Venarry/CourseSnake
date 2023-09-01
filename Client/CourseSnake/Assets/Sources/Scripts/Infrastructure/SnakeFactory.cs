using Unity.VisualScripting;
using UnityEngine;

public class SnakeFactory
{
    private readonly SnakeView _prefabSnake = Resources.Load<SnakeView>(ResourcesPath.Snake);
    private readonly SnakeBody _prefabBody = Resources.Load<SnakeBody>(ResourcesPath.SnakeBodyPart);
    private readonly SnakeBody _prefabTail = Resources.Load<SnakeBody>(ResourcesPath.SnakeTail);
    private CameraMovement _cameraMovement;
    private AppleFactory _appleFactory;
    private StateHandlerRoom _stateHandlerRoom;

    public void InitStateHandlerRoom(StateHandlerRoom stateHandlerRoom)
    {
        _stateHandlerRoom = stateHandlerRoom;
    }

    public void Init(CameraMovement cameraMovement, AppleFactory appleFactory)
    {
        _cameraMovement = cameraMovement;
        _appleFactory = appleFactory;
    }

    public SnakeView CreatePlayer(Vector3 position, string name, Color color, string id, bool isMultiplayer, Player player = null)
    {
        SnakeView snakeView = Object.Instantiate(_prefabSnake, position, Quaternion.identity);

        Camera camera = _cameraMovement.GetComponent<Camera>();
        snakeView.AddComponent<MouseClickHandler>().Init(camera);

        SnakeBodyParts snakeBodyParts = snakeView.GetComponent<SnakeBodyParts>();
        SnakeMovement snakeMovement = snakeView.GetComponent<SnakeMovement>();
        SnakeNameView snakeNameView = snakeView.GetComponent<SnakeNameView>();
        snakeNameView.SetName(name);
        snakeNameView.SetLookAtTarget(_cameraMovement.transform);

        //SnakeBody snakeBody = snakeView.GetComponent<SnakeBody>();
        //snakeBody.Init(snakeBodyParts, _appleFactory);
        snakeView.AddComponent<SnakeCollisionHandler>();

        snakeBodyParts.Init(this, color);

        SnakeScoreModel snakeScoreModel = new();
        SnakeScorePresenter snakeScorePresenter = new(
            snakeScoreModel, 
            snakeBodyParts, 
            snakeMovement);

        snakeView.Init(snakeScorePresenter, snakeBodyParts, color, id);

        _cameraMovement.SetTarget(snakeView.transform);

        if (isMultiplayer == true)
        {
            SnakeRotation snakeRotation = snakeView.GetComponent<SnakeRotation>();
            snakeView.AddComponent<PlayerMultiplayerHandler>().Init(
                player, 
                _stateHandlerRoom, 
                snakeView,
                snakeMovement, 
                snakeRotation, 
                snakeScorePresenter);
        }

        return snakeView;
    }

    public SnakeView CreateEnemy(Player player, string id)
    {
        Color snakeColor = new(player.Color.x, player.Color.y, player.Color.z);
        string name = player.Name;

        Vector3 position = new(player.Position.x, player.Position.y, player.Position.z);
        Quaternion rotation = Quaternion.Euler(player.Rotation.x, player.Rotation.y, player.Rotation.z);
        SnakeView snakeView = Object.Instantiate(_prefabSnake, position, rotation);

        SnakeBodyParts snakeBodyParts = snakeView.GetComponent<SnakeBodyParts>();
        snakeBodyParts.Init(this, snakeColor);

        SnakeMovement snakeMovement = snakeView.GetComponent<SnakeMovement>();
        SnakeRotation snakeRotation = snakeView.GetComponent<SnakeRotation>();
        SnakeNameView snakeNameView = snakeView.GetComponent<SnakeNameView>();
        snakeNameView.SetName(name);
        snakeNameView.SetLookAtTarget(_cameraMovement.transform);

        SnakeBody snakeBody = snakeView.AddComponent<SnakeBody>();
        snakeBody.Init(snakeBodyParts, _appleFactory);

        Vector3 targetPoint = new(player.Direction.x, player.Direction.y, player.Direction.z);
        snakeRotation.SetTargetPoint(targetPoint);

        SnakeScoreModel snakeScoreModel = new();
        SnakeScorePresenter snakeScorePresenter = new(snakeScoreModel, snakeBodyParts, snakeMovement);

        snakeView.Init(snakeScorePresenter, snakeBodyParts, snakeColor, id);
        snakeView.AddComponent<EnemyMultiplayerHandler>().Init(
            id,
            _stateHandlerRoom,
            player,
            snakeRotation, 
            snakeMovement, 
            snakeScorePresenter,
            snakeView);

        return snakeView;
    }

    public SnakeBody CreateBody(Vector3 position, Color color, SnakeBodyParts owner) =>
        CreateSnakePart(_prefabBody, position, color, owner);

    public SnakeBody CreateTail(Vector3 position, Color color, SnakeBodyParts owner)=>
        CreateSnakePart(_prefabTail, position, color, owner);

    private SnakeBody CreateSnakePart(SnakeBody prefab, Vector3 position, Color color, SnakeBodyParts owner)
    {
        SnakeBody part = Object.Instantiate(prefab, position, Quaternion.identity);
        part.Init(owner, _appleFactory);
        part.GetComponentInChildren<MeshRenderer>().material.color = color;

        return part;
    }
}
