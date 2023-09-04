using System;
using UnityEngine;

public class SinglePlayerSnakeHandler : MonoBehaviour
{
    private PlayerClickHandler _mouseClickHandler;
    private SnakeRotation _snakeRotation;
    private SnakeMovement _snakeMovement;
    private bool _isInitialized;

    public void Init(
        PlayerClickHandler mouseClickHandler,
        SnakeRotation snakeRotation, 
        SnakeMovement snakeMovement)
    {
        gameObject.SetActive(false);

        _mouseClickHandler = mouseClickHandler;
        _snakeRotation = snakeRotation;
        _snakeMovement = snakeMovement;
        _isInitialized = true;

        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        if (_isInitialized == false)
            return;

        _mouseClickHandler.PointSet += OnDirectionSet;
        _mouseClickHandler.BoostStateChanged += OnBoostStateChange;
    }

    private void OnDisable()
    {
        if (_isInitialized == false)
            return;

        _mouseClickHandler.PointSet -= OnDirectionSet;
        _mouseClickHandler.BoostStateChanged -= OnBoostStateChange;
    }
    private void OnBoostStateChange(bool state)
    {
        _snakeMovement.SetBoostState(state);
    }

    private void OnDirectionSet(Vector3 direction)
    {
        _snakeRotation.SetRotateDirection(direction);
    }
}
