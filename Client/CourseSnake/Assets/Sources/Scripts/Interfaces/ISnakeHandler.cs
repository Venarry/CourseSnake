using System;

public interface ISnakeHandler
{
    public int SnakeCount { get; }
    public int BotsCount { get; }
    public event Action<SnakeView> PlayerSpawned;
    public event Action<SnakeView> BotSpawned;
    public event Action<SnakeView> BotRemoved;
    public event Action<SnakeView> SnakeSpawned;
    public event Action<SnakeView> SnakeRemoved;
}
