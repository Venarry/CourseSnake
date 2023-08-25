using UnityEngine;

public class Bootstrapper : MonoBehaviour
{
    private void Awake()
    {
        SnakeFactory snakeFactory = new();

        SnakeView snakeView = snakeFactory.CreateSnake(Vector3.zero);
    }
}
