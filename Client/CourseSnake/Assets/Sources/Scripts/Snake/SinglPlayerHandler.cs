using UnityEngine;

public class SinglPlayerHandler : MonoBehaviour
{
    private MouseClickHandler _mouseClickHandler;
    private SnakeRotation _snakeRotation;
    private bool _isInitialized;

    public void Init(MouseClickHandler mouseClickHandler, SnakeRotation snakeRotation)
    {
        gameObject.SetActive(false);

        _mouseClickHandler = mouseClickHandler;
        _snakeRotation = snakeRotation;
        _isInitialized = true;

        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        if (_isInitialized == false)
            return;

        _mouseClickHandler.DirectionSet += OnDirectionSet;
    }

    private void OnDisable()
    {
        if (_isInitialized == false)
            return;

        _mouseClickHandler.DirectionSet -= OnDirectionSet;
    }

    private void OnDirectionSet(Vector3 direction)
    {
        _snakeRotation.SetRotateDirection(direction);
    }
}
