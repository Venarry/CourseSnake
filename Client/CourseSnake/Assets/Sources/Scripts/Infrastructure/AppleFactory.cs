using UnityEngine;

public class AppleFactory
{
    private readonly Apple _prefab = Resources.Load<Apple>(ResourcesPath.Apple);

    public Apple Create(Vector3 position, float reward)
    {
        Apple apple = Object.Instantiate(_prefab, position, Quaternion.identity);
        apple.SetReward(reward);

        return apple;
    }
}
