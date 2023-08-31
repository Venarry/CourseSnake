using UnityEngine;

public class SnakeBody : MonoBehaviour
{
    private SnakeBodyParts _owner;
    private AppleFactory _appleFactory;

    public void Init(SnakeBodyParts owner, AppleFactory appleFactory)
    {
        _owner = owner;
        _appleFactory = appleFactory;
    }

    public void Destroy()
    {
        float reward = transform.localScale.x;
        _appleFactory.Create(reward, transform.position);

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out SnakeBodyParts snake))
        {
            if (snake == _owner)
                return;

            snake.Destroy();
        }
    }
}
