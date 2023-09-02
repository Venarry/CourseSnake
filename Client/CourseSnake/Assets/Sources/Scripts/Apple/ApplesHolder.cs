using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplesHolder
{
    private readonly List<Apple> _apples = new();
    private readonly int _maxCount;

    public ApplesHolder(int maxCount)
    {
        _maxCount = maxCount;
    }

    public bool CanSpawn => _apples.Count < _maxCount;

    public void AddApple(Apple apple)
    {
        _apples.Add(apple);
        apple.Destroyed += OnAppleDestroy;
    }

    public void OnAppleDestroy(Apple apple)
    {
        apple.Destroyed -= OnAppleDestroy;
        _apples.Remove(apple);
    }
}
