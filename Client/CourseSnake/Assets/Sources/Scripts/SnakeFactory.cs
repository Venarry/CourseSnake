using UnityEngine;

public class SnakeFactory
{
    private readonly SnakeView _prefabSnake = Resources.Load<SnakeView>(ResourcesPath.Snake);
    private readonly Transform _prefabBody = Resources.Load<Transform>(ResourcesPath.SnakeBodyPart);
    private readonly Transform _prefabTail = Resources.Load<Transform>(ResourcesPath.SnakeTail);
    private readonly CameraMovement _mainCameraPrefab = Resources.Load<CameraMovement>(ResourcesPath.MainCamera);

    public SnakeView CreateSnake(Vector3 position)
    {
        CameraMovement mainCamera = Object.Instantiate(_mainCameraPrefab);
        Camera camera = mainCamera.GetComponent<Camera>();

        SnakeView snakeView = Object.Instantiate(_prefabSnake, position, Quaternion.identity);
        snakeView.GetComponent<SnakeBodyParts>().Init(this);
        snakeView.GetComponent<SnakeRotation>().Init(camera);

        mainCamera.SetTarget(snakeView.transform);
        return snakeView;
    }

    public Transform CreateBody(Vector3 position)
    {
        Transform body = Object.Instantiate(_prefabBody, position, Quaternion.identity);
        return body;
    }

    public Transform CreateTail(Vector3 position)
    {
        Transform tail = Object.Instantiate(_prefabTail, position, Quaternion.identity);
        return tail;
    }
}
