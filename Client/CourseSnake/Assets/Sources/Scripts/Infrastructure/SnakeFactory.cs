using Unity.VisualScripting;
using UnityEngine;

public class SnakeFactory
{
    private readonly SnakeView _prefabSnake = Resources.Load<SnakeView>(ResourcesPath.Snake);
    private readonly SnakeView _prefabEnemySnake = Resources.Load<SnakeView>(ResourcesPath.EnemySnake);
    private readonly Transform _prefabBody = Resources.Load<Transform>(ResourcesPath.SnakeBodyPart);
    private readonly Transform _prefabTail = Resources.Load<Transform>(ResourcesPath.SnakeTail);
    private CameraMovement _cameraMovement;
    private StateHandlerRoom _stateHandlerRoom;

    public void InitStateHandlerRoom(StateHandlerRoom stateHandlerRoom)
    {
        _stateHandlerRoom = stateHandlerRoom;
    }

    public void InitCamera(CameraMovement cameraMovement)
    {
        _cameraMovement = cameraMovement;
    }

    public SnakeView Create(Vector3 position, string name, Color color, bool isMultiplayer, Player player = null)
    {
        SnakeView snakeView = Object.Instantiate(_prefabSnake, position, Quaternion.identity);

        Camera camera = _cameraMovement.GetComponent<Camera>();

        SnakeBodyParts snakeBodyParts = snakeView.GetComponent<SnakeBodyParts>();
        SnakeMovement snakeMovement = snakeView.GetComponent<SnakeMovement>();
        SnakeNameView snakeNameView = snakeView.GetComponent<SnakeNameView>();
        snakeNameView.SetName(name);
        snakeNameView.SetLookAtTarget(_cameraMovement.transform);

        snakeBodyParts.Init(this, color);
        snakeView.GetComponent<MouseClickHandler>().Init(camera);

        SnakeScoreModel snakeScoreModel = new();
        SnakeScorePresenter snakeScorePresenter = new(
            snakeScoreModel, 
            snakeBodyParts, 
            snakeMovement);

        snakeView.Init(snakeScorePresenter, color);

        _cameraMovement.SetTarget(snakeView.transform);

        if (isMultiplayer == true)
        {
            SnakeRotation snakeRotation = snakeView.GetComponent<SnakeRotation>();
            snakeView.AddComponent<PlayerMultiplayerHandler>().Init(
                player, 
                _stateHandlerRoom, 
                snakeMovement, 
                snakeRotation, 
                snakeScorePresenter);
        }

        return snakeView;
    }

    public SnakeView CreateEnemy(Player player)
    {
        Color snakeColor = new(player.Color.x, player.Color.y, player.Color.z);
        string name = player.Name;

        Vector3 position = new(player.Position.x, player.Position.y, player.Position.z);
        Quaternion rotation = Quaternion.Euler(player.Rotation.x, player.Rotation.y, player.Rotation.z);
        SnakeView snakeView = Object.Instantiate(_prefabEnemySnake, position, rotation);

        SnakeBodyParts snakeBodyParts = snakeView.GetComponent<SnakeBodyParts>();
        snakeBodyParts.Init(this, snakeColor);

        SnakeMovement snakeMovement = snakeView.GetComponent<SnakeMovement>();
        SnakeRotation snakeRotation = snakeView.GetComponent<SnakeRotation>();
        SnakeNameView snakeNameView = snakeView.GetComponent<SnakeNameView>();
        snakeNameView.SetName(name);
        snakeNameView.SetLookAtTarget(_cameraMovement.transform);

        Vector3 targetPoint = new(player.Direction.x, player.Direction.y, player.Direction.z);
        snakeRotation.SetTargetPoint(targetPoint);

        SnakeScoreModel snakeScoreModel = new();
        SnakeScorePresenter snakeScorePresenter = new(snakeScoreModel, snakeBodyParts, snakeMovement);

        snakeView.Init(snakeScorePresenter, snakeColor);
        snakeView.GetComponent<EnemyMultiplayerHandler>().Init(player, snakeRotation, snakeMovement, snakeScorePresenter);

        return snakeView;
    }

    public Transform CreateBody(Vector3 position, Color color)
    {
        Transform body = Object.Instantiate(_prefabBody, position, Quaternion.identity);
        body.GetComponentInChildren<MeshRenderer>().material.color = color;
        return body;
    }

    public Transform CreateTail(Vector3 position, Color color)
    {
        Transform tail = Object.Instantiate(_prefabTail, position, Quaternion.identity);
        tail.GetComponentInChildren<MeshRenderer>().material.color = color;
        return tail;
    }
}
