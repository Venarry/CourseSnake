using System;

public interface ISnakeSpawnHandler
{
    public event Action<SnakeView> PlayerSpawned;
    public event Action<SnakeView> SnakeSpawned;
    public event Action<SnakeView> SnakeRemoved;
}
