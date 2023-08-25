using UnityEngine;

public class Apple : MonoBehaviour
{
    [SerializeField] private float _reward;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out SnakeView snakeView))
        {
            snakeView.AddScore(_reward);
            Destroy(gameObject);
        }
    }
}
