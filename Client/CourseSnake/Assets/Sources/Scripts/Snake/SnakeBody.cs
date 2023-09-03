using UnityEngine;

public class SnakeBody : MonoBehaviour
{
    private SnakeBodyParts _owner;
    private AppleSpawnInitiator _appleSpawnInitiator;

    public void Init(SnakeBodyParts owner, AppleSpawnInitiator appleSpawnInitiator)
    {
        _owner = owner;
        _appleSpawnInitiator = appleSpawnInitiator;
    }

    public bool IsOwner(SnakeBodyParts snakeBodyParts) =>
        _owner == snakeBodyParts;

    public void Destroy()
    {
        float reward = transform.localScale.x;
        _appleSpawnInitiator.InitSpawn(transform.position, reward, overLimit: true);

        Destroy(gameObject);
    }
}
