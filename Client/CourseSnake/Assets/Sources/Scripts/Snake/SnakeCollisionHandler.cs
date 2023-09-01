using UnityEngine;

public class SnakeCollisionHandler : MonoBehaviour
{
    private SnakeView _snakeView;
    private SnakeBodyParts _snakeBodyParts;

    private void Awake()
    {
        _snakeView = GetComponent<SnakeView>();
        _snakeBodyParts = GetComponent<SnakeBodyParts>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out SnakeBody body))
        {
            if(body.IsOwner(_snakeBodyParts) == false)
            {
                _snakeBodyParts.Destroy();
            }
        }

        if (other.TryGetComponent(out SnakeView enemy))
        {
            float myAngle = Vector3.Angle(enemy.transform.position - transform.position, transform.forward);
            float enemyAngle = Vector3.Angle(transform.position - enemy.transform.position, enemy.transform.forward);

            if (myAngle < enemyAngle)
                _snakeBodyParts.Destroy();
        }

        if (other.TryGetComponent(out DeathBarrier _))
        {
            _snakeBodyParts.Destroy();
        }
    }
}
