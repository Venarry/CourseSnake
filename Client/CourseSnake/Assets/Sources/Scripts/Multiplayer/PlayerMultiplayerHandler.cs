using Colyseus.Schema;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMultiplayerHandler : MonoBehaviour
{
    private Player _thisPlayer;
    private StateHandlerRoom _stateHandlerRoom;
    private SnakeView _snakeView;
    private SnakeMovement _snakeMovement;
    private SnakeRotation _snakeRotation;
    private SnakeScorePresenter _snakeScorePresenter;
    private PlayerClickHandler _mouseClickHandler;
    private bool _isInitialized;

    public void Init(Player player,
        StateHandlerRoom stateHandlerRoom, 
        SnakeView snakeView,
        SnakeMovement snakeMovement, 
        SnakeRotation snakeRotation, 
        SnakeScorePresenter snakeScorePresenter,
        PlayerClickHandler mouseClickHandler)
    {
        gameObject.SetActive(false);

        _thisPlayer = player;
        _stateHandlerRoom = stateHandlerRoom;
        _snakeView = snakeView;
        _snakeMovement = snakeMovement;
        _snakeRotation = snakeRotation;
        _snakeScorePresenter = snakeScorePresenter; 
        _mouseClickHandler = mouseClickHandler;
        _isInitialized = true;

        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        if (_isInitialized == false)
            return;

        _thisPlayer.Position.OnChange += OnPositionChange;
        _thisPlayer.Direction.OnChange += OnDirectionChange;
        _thisPlayer.OnChange += OnDataChange;

        _snakeView.Destroyed += OnSnakeDestroy;
        _mouseClickHandler.DirectionSet += OnDirectionSet;
        _mouseClickHandler.BoostStateChanged += OnBoostStateChange;
        _snakeMovement.PositionChanged += OnPositionChange;
        _snakeRotation.RotationChanged += OnRotationChanged;
        _snakeScorePresenter.ScoreChanged += OnScoreChanged;
    }

    private void OnDisable()
    {
        if (_isInitialized == false)
            return;

        _thisPlayer.Position.OnChange -= OnPositionChange;
        _thisPlayer.Direction.OnChange -= OnDirectionChange;
        _thisPlayer.OnChange -= OnDataChange;

        _snakeView.Destroyed -= OnSnakeDestroy;
        _mouseClickHandler.DirectionSet -= OnDirectionSet;
        _mouseClickHandler.BoostStateChanged -= OnBoostStateChange;
        _snakeMovement.PositionChanged -= OnPositionChange;
        _snakeRotation.RotationChanged -= OnRotationChanged;
        _snakeScorePresenter.ScoreChanged -= OnScoreChanged;
    }

    private void OnDataChange(List<DataChange> changes)
    {
        foreach (DataChange change in changes)
        {
            switch (change.Field)
            {
                case "BoostState":
                    _snakeMovement.SetBoostState((bool)change.Value);
                    break;
            }
        }
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

        _snakeRotation.SetRotateDirection(point);
    }

    private void OnSnakeDestroy()
    {
        _stateHandlerRoom.SendPlayerData("PlayerDestroyed");
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

    private void OnBoostStateChange(bool state)
    {
        _stateHandlerRoom.SendPlayerData("BoostState", state);
    }

    private void OnDirectionSet(Vector3 point)
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
