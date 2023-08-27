using System;
using UnityEngine;

public class PlayerSpawnPointInitiator
{
    private readonly float _widthSpawn;
    private readonly float _heightSpawn;

    public event Action<Vector3> PlayerPointInited;

    public PlayerSpawnPointInitiator(
        float widthSpawn,
        float heightSpawn)
    {
        _widthSpawn = widthSpawn;
        _heightSpawn = heightSpawn;
    }

    public void InitPlayerSpawnPoint()
    {
        float targetWidthPosition = UnityEngine.Random.Range(-_widthSpawn, _widthSpawn);
        float targetHeightPosition = UnityEngine.Random.Range(-_heightSpawn, _heightSpawn);
        Vector3 spawnPosition = new(targetWidthPosition, 0, targetHeightPosition);

        PlayerPointInited?.Invoke(spawnPosition);
    }
}
