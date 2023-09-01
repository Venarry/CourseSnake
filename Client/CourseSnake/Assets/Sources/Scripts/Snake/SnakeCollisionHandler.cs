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
                float myAngle = Vector3.Angle(body.transform.position - transform.position, transform.forward);
                float bodyAngle = Vector3.Angle(transform.position - body.transform.position, body.transform.forward);

                if (myAngle < bodyAngle)
                    _snakeBodyParts.Destroy();
            }
        }

        if (other.TryGetComponent(out DeathBarrier _))
        {
            _snakeView.Destroy();
        }
    }
}
