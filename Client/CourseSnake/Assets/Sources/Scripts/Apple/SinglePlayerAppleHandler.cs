using System;
using System.Collections.Generic;
using UnityEngine;

public class SinglePlayerAppleHandler : IAppleHandler
{
    private readonly AppleSpawnInitiator _spawnInitiator;
    private readonly AppleFactory _appleFactory;

    private readonly List<Apple> _apples = new();

    public SinglePlayerAppleHandler(AppleSpawnInitiator spawnInitiator, AppleFactory appleFactory)
    {
        _spawnInitiator = spawnInitiator;
        _appleFactory = appleFactory;

        _spawnInitiator.Inited += OnAppleSpawnInit;
    }

    public event Action<Apple> AppleAdded;

    public bool TryGetRandomApple(out Apple apple)
    {
        apple = null;

        if (_apples.Count == 0)
            return false;

        apple = _apples[UnityEngine.Random.Range(0, _apples.Count)];
        return true;
    }

    private void OnAppleSpawnInit(Vector3 position, float reward, bool overLimit)
    {
        if (_apples.Count >= GameConfig.MaxApples && overLimit == false)
            return;

        Apple apple = _appleFactory.Create(position, reward);
        _apples.Add(apple);
        AppleAdded?.Invoke(apple);

        apple.Destroyed += OnAppleDestroy;
    }

    private void OnAppleDestroy(Apple apple)
    {
        apple.Destroyed -= OnAppleDestroy;
        _apples.Remove(apple);
    }
}
