using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MultiplayerAppleSpawnHandler : IAppleHandler
{
    private readonly StateHandlerRoom _stateHandlerRoom;
    private readonly AppleFactory _appleFactory;

    private readonly Dictionary<string, Apple> _apples = new();

    public event Action<Apple> AppleAdded;

    public MultiplayerAppleSpawnHandler(StateHandlerRoom stateHandlerRoom,
        AppleSpawnInitiator appleSpawnInitiator,
        AppleFactory appleFactory)
    {
        _stateHandlerRoom = stateHandlerRoom;
        _appleFactory = appleFactory;

        stateHandlerRoom.Room.State.Apples.OnAdd += OnAppleAdd;
        stateHandlerRoom.Room.State.Apples.OnRemove += OnAppleRemove;
        appleSpawnInitiator.Inited += OnAppleInited;
    }

    public bool TryGetRandomApple(out Apple apple)
    {
        apple = null;

        if (_apples.Count == 0)
            return false;

        int targetIndex = UnityEngine.Random.Range(0, _apples.Count);
        apple = _apples.ElementAt(targetIndex).Value;

        return true;
    }

    private void OnAppleRemove(string key, ServerApple value)
    {
        if(_apples[key] != null)
        {
            _apples[key].Destroyed -= OnAppleDestroy;
            _apples[key].RemoveApple();
        }

        _apples.Remove(key);
    }

    private void OnAppleAdd(string key, ServerApple value)
    {
        Vector3 spawnPosition = new(value.Position.x, value.Position.y, value.Position.z);
        Apple apple = _appleFactory.Create(spawnPosition, value.Reward);
        _apples.Add(key, apple);

        apple.Destroyed += OnAppleDestroy;

        AppleAdded?.Invoke(apple);
    }

    private void OnAppleDestroy(Apple apple)
    {
        string key = "";

        foreach (KeyValuePair<string, Apple> currentApple in _apples)
        {
            if(currentApple.Value == apple)
            {
                key = currentApple.Key;
            }
        }

        if (key == "")
            return;

        apple.Destroyed -= OnAppleDestroy;
        _stateHandlerRoom.SendPlayerData("RemoveApple", key);
    }

    private void OnAppleInited(Vector3 spawnPosition, float reward, bool overLimit)
    {
        if (_apples.Count >= GameConfig.MaxApples && overLimit == false)
            return;

        MyVector3 position = new(spawnPosition);

        Dictionary<string, object> data = new()
        {
            { "Position", position },
            { "Reward", reward },
        };

        _stateHandlerRoom.SendPlayerData("CreateApple", data);
    }
}
