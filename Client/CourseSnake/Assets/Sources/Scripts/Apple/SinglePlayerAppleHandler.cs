using System;
using System.Collections.Generic;

public class SinglePlayerAppleHandler : IAppleHandler
{
    private readonly StateHandlerRoom _stateHandlerRoom;
    private readonly AppleFactory _appleFactory;

    private readonly Dictionary<string, Apple> _apples = new();

    public event Action<Apple> AppleAdded;

    public bool TryGetRandomApple(out Apple apple)
    {
        throw new NotImplementedException();
    }
}
