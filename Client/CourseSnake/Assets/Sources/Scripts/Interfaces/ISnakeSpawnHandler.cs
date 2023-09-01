using System;

public interface ISnakeSpawnHandler
{
    public event Action<SnakeView> SnakeSpawned;
    public event Action<string> SnakeRemoved;
}
