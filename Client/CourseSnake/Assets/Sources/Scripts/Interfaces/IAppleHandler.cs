using System;

public interface IAppleHandler
{
    public event Action<Apple> AppleAdded;
    public bool TryGetRandomApple(out Apple apple);
}
