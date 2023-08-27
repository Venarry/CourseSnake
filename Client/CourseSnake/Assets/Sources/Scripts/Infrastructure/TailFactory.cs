using UnityEngine;

public class TailFactory
{
    private Transform _prefab = Resources.Load<Transform>(ResourcesPath.SnakeTail);

    public Transform Create(Vector3 position)
    {
        Transform tail = Object.Instantiate(_prefab, position, Quaternion.identity);
        return tail;
    }
}
