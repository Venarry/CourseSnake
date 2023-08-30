using Colyseus.Schema;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMultiplayerHandler : MonoBehaviour
{
    private Player _thisPlayer;
    private StateHandlerRoom _stateHandlerRoom;
    private SnakeMovement _snakeMovement;
    private SnakeRotation _snakeRotation;
    private SnakeScorePresenter _snakeScorePresenter;
    private bool _isInitialized;

    public void Init(Player player,
        StateHandlerRoom stateHandlerRoom, 
        SnakeMovement snakeMovement, 
        SnakeRotation snakeRotation, 
        SnakeScorePresenter snakeScorePresenter)
    {
        gameObject.SetActive(false);

        _thisPlayer = player;
        _stateHandlerRoom = stateHandlerRoom;
        _snakeMovement = snakeMovement;
        _snakeRotation = snakeRotation;
        _snakeScorePresenter = snakeScorePresenter; 
        _isInitialized = true;

        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        if (_isInitialized == false)
            return;

        _thisPlayer.Position.OnChange += OnPositionChange;
        _thisPlayer.Direction.OnChange += OnDirectionChange;

        _snakeMovement.PositionChanged += OnPositionChange;
        _snakeMovement.BoostStateChanged += OnBoostStateChange;
        _snakeRotation.RotationChanged += OnRotationChanged;
        _snakeRotation.TargetPointSet += OnTargetPointSet;
        _snakeScorePresenter.ScoreChanged += OnScoreChanged;
    }

    private void OnDirectionChange(List<DataChange> changes)
    {
        Vector3 point = _snakeRotation.TargetPoint;

        foreach (var change in changes)
        {
            switch (change.Field)
            {
                case "x":
                    point.x = (float)change.Value;
                    break;

                case "y":
                    point.y = (float)change.Value;
                    break;

                case "z":
                    point.z = (float)change.Value;
                    break;
            }
        }

        //_snakeRotation.SetTargetPoint(point);
    }

    private void OnPositionChange(List<DataChange> changes)
    {
        Vector3 currentPosition = transform.position;

        foreach (var change in changes)
        {
            switch (change.Field)
            {
                case "x":
                    currentPosition.x = (float)change.Value;
                    break;

                case "y":
                    currentPosition.y = 0;
                    break;

                case "z":
                    currentPosition.z = (float)change.Value;
                    break;
            }
        }
        
        _snakeMovement.SetLerpPosition(currentPosition);
    }

    private void OnDisable()
    {
        if (_isInitialized == false)
            return;

        _thisPlayer.Position.OnChange -= OnPositionChange;
        _thisPlayer.Direction.OnChange -= OnDirectionChange;

        _snakeMovement.PositionChanged -= OnPositionChange;
        _snakeMovement.BoostStateChanged -= OnBoostStateChange;
        _snakeRotation.RotationChanged -= OnRotationChanged;
        _snakeRotation.TargetPointSet -= OnTargetPointSet;
        _snakeScorePresenter.ScoreChanged -= OnScoreChanged;
    }

    private void OnBoostStateChange(bool state)
    {
        _stateHandlerRoom.SendPlayerData("BoostState", state);
    }

    private void OnTargetPointSet(Vector3 point)
    {
        MyVector3 myVector3 = new(point);
        _stateHandlerRoom.SendPlayerData("Direction", myVector3);
    }

    private void OnScoreChanged(float score)
    {
        _stateHandlerRoom.SendPlayerData("Score", score);
    }

    private void OnRotationChanged(Vector3 rotation)
    {
        _stateHandlerRoom.SendPlayerData("Rotation", rotation);
    }

    private void OnPositionChange(Vector3 position)
    {
        MyVector3 myVector3 = new(position);
        _stateHandlerRoom.SendPlayerData("Position", myVector3);
    }
}
