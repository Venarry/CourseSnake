using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerAppleSpawnHandler
{
    private StateHandlerRoom _stateHandlerRoom;
    private AppleFactory _appleFactory;

    private readonly List<Apple> _apples = new();

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

    private void OnAppleRemove(int key, ServerApple value)
    {
        if(_apples[key] != null)
        {
            Object.Destroy(_apples[key].gameObject);
        }

        _apples.RemoveAt(key);
    }

    private void OnAppleAdd(int key, ServerApple value)
    {
        Vector3 spawnPosition = new(value.Position.x, value.Position.y, value.Position.z);
        Apple apple = _appleFactory.Create(value.Reward, spawnPosition);
        _apples.Add(apple);
    }

    private void OnAppleInited(Vector3 spawnPosition, float reward)
    {
        MyVector3 position = new(spawnPosition);

        Dictionary<string, object> data = new()
        {
            { "Position", position },
            { "Reward", reward },
        };

        _stateHandlerRoom.SendMessage("CreateApple", data);
    }
}
