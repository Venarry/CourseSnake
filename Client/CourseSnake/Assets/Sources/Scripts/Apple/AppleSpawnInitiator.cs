using System;
using UnityEngine;

public class AppleSpawnInitiator : MonoBehaviour
{
    [SerializeField] private float _spawnTime = 2f;
    [SerializeField] private Vector2 _spawnRange;
    private float _currentTime;

    public event Action<Vector3, float> Inited;

    private void Update()
    {
        _currentTime += Time.deltaTime;

        if(_currentTime >= _spawnTime)
        {
            _currentTime = 0;

            InitRandomApple();
        }
    }

    public void InitRandomApples(int count)
    {
        for (int i = 0; i < count; i++)
        {
            InitRandomApple();
        }
    }

    public void SetSpawnRange(Vector2 spawnRange)
    {
        _spawnRange = spawnRange;
    }

    public void InitSpawn(Vector3 position, float reward)
    {
        Inited?.Invoke(position, reward);
    }

    private void InitRandomApple()
    {
        Vector3 spawnPosition = CreatePointBySquad();
        float reward = UnityEngine.Random.Range(0.3f, 1f);
        Inited?.Invoke(spawnPosition, reward);
    }

    private Vector3 CreatePointByRound()
    {
        Vector3 multipli = new Vector3(UnityEngine.Random.Range(-1, 1), 0, UnityEngine.Random.Range(-1, 1)).normalized;

        Vector3 spawnPosition = new(_spawnRange.x * multipli.x, 0, _spawnRange.y * multipli.y);
        return spawnPosition;
    }

    private Vector3 CreatePointBySquad()
    {
        float x = UnityEngine.Random.Range(-_spawnRange.x, _spawnRange.x);
        float y = UnityEngine.Random.Range(-_spawnRange.y, _spawnRange.y);

        Vector3 spawnPosition = new(x, 0, y);
        return spawnPosition;
    }
}
