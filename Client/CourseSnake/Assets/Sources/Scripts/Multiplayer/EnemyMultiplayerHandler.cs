using Colyseus.Schema;
using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMultiplayerHandler : MonoBehaviour
{
    private string _id;
    private StateHandlerRoom _stateHandlerRoom;
    private Player _thisPlayer;
    private SnakeRotation _snakeRotation;
    private SnakeMovement _snakeMovement;
    private SnakeScorePresenter _scorePresenter;
    private SnakeView _snakeView;
    private bool _isInitialized;

    public void Init(string id,
        StateHandlerRoom stateHandlerRoom,
        Player player, 
        SnakeRotation snakeRotation, 
        SnakeMovement snakeMovement, 
        SnakeScorePresenter scorePresenter,
        SnakeView snakeView)
    {
        gameObject.SetActive(false);

        _id = id;
        _stateHandlerRoom = stateHandlerRoom;
        _thisPlayer = player;
        _snakeRotation = snakeRotation;
        _snakeMovement = snakeMovement;
        _scorePresenter = scorePresenter;
        _snakeView = snakeView;
        _isInitialized = true;

        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        if (_isInitialized == false)
            return;

        _thisPlayer.Direction.OnChange += OnTargetPointChange;
        _thisPlayer.Position.OnChange += OnPositionChange;
        _thisPlayer.Rotation.OnChange += OnRotationChange;
        _thisPlayer.OnChange += OnDataChange;

        _snakeView.Destroyed += OnSnakeDestroy;
    }

    private void OnDisable()
    {
        if (_isInitialized == false)
            return;

        _thisPlayer.Direction.OnChange -= OnTargetPointChange;
        _thisPlayer.Position.OnChange -= OnPositionChange;
        _thisPlayer.Rotation.OnChange -= OnRotationChange;
        _thisPlayer.OnChange -= OnDataChange;

        _snakeView.Destroyed -= OnSnakeDestroy;
    }
    private void OnSnakeDestroy()
    {
        //_stateHandlerRoom.SendPlayerData("EnemyDestroyed", _id);
    }

    private void OnDataChange(List<DataChange> changes)
    {
        foreach (var change in changes)
        {
            switch (change.Field)
            {
                case "BoostState":
                    _snakeMovement.SetBoostState((bool)change.Value);
                    break;

                case "Score":
                    _scorePresenter.SetScore((float)change.Value);
                    break;
            }
        }
    }

    private void OnRotationChange(List<DataChange> changes)
    {
        Vector3 rotation = transform.rotation.eulerAngles;
        rotation = ApplyVectorChange(changes, rotation);
        _snakeRotation.SetRotation(rotation);
    }

    private void OnPositionChange(List<DataChange> changes)
    {
        Vector3 position = transform.position;
        position = ApplyVectorChange(changes, position);
        _snakeMovement.SetLerpPosition(position);
    }

    private void OnTargetPointChange(List<DataChange> changes)
    {
        Vector3 point = _snakeRotation.TargetPoint;
        point = ApplyVectorChange(changes, point);
        _snakeRotation.SetTargetPoint(point);
    }

    private Vector3 ApplyVectorChange(List<DataChange> changes, Vector3 startVector)
    {
        foreach (var change in changes)
        {
            switch (change.Field)
            {
                case "x":
                    startVector.x = (float)change.Value;
                    break;

                case "y":
                    startVector.y = (float)change.Value;
                    break;

                case "z":
                    startVector.z = (float)change.Value;
                    break;
            }
        }

        return startVector;
    }
}
