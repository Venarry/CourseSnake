using System;

public interface ISnakeHandler
{
    public int SnakeCount { get; }
    public event Action<SnakeView> PlayerSpawned;
    public event Action<SnakeView> SnakeSpawned;
    public event Action<SnakeView> SnakeRemoved;
}
