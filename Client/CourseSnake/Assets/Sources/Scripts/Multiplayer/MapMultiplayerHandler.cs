using Colyseus;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMultiplayerHandler : MonoBehaviour
{
    private ColyseusRoom<State> _room;

    public event Action<string, Player> PlayerJoined;
    public event Action<string, Player> EnemyJoined;
    public event Action<string> EnemyLeaved;

    private void Awake()
    {
        _room = StateHandlerRoom.Instance.Room;
        Enable();
    }

    public void Enable()
    {
        _room.State.players.OnAdd += OnPlayerAdd;
        _room.State.players.OnRemove += OnPlayerRemove;
    }

    private void OnDestroy()
    {
        LeaveRoom();
    }

    private void OnPlayerAdd(string key, Player player)
    {
        if (key == _room.SessionId)
        {
            OnPlayerJoin(key, player);
        }
        else
        {
            OnEnemyJoin(key, player);
        }
    }

    private void OnEnemyJoin(string key, Player player)
    {
        EnemyJoined?.Invoke(key, player);
    }

    private void OnPlayerJoin(string key, Player player)
    {
        PlayerJoined?.Invoke(key, player);
    }

    private void OnPlayerRemove(string key, Player value)
    {
        EnemyLeaved?.Invoke(key);
    }

    public void LeaveRoom()
    {
        _room.Leave();

        _room.State.players.OnAdd -= OnPlayerAdd;
        _room.State.players.OnRemove -= OnPlayerRemove;
    }
}
