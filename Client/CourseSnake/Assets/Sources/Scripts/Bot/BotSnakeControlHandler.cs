using UnityEngine;

public class BotSnakeControlHandler : MonoBehaviour
{
    private IAppleHandler _appleHandler;
    private SnakeRotation _snakeRotation;
    private Apple _currentApple;
    private bool _isInitialized;

    public void Init(IAppleHandler appleHandler, SnakeRotation snakeRotation)
    {
        gameObject.SetActive(false);

        _appleHandler = appleHandler;
        _snakeRotation = snakeRotation;
        _isInitialized = true;

        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        if (_isInitialized == false)
            return;

        _appleHandler.AppleAdded += OnAppleAdd;
    }

    private void OnDisable()
    {
        if (_isInitialized == false)
            return;

        _appleHandler.AppleAdded -= OnAppleAdd;

        if(_currentApple != null)
            _currentApple.Destroyed -= OnAppleDestroyed;
    }

    private void OnAppleAdd(Apple apple)
    {
        if (_currentApple != null)
            return;

        _currentApple = apple;
        Debug.Log(_currentApple.transform.position);
        _snakeRotation.SetRotateDirection(_currentApple.transform.position - transform.position);
        _snakeRotation.SetRotateDirection(_currentApple.transform.position);
        _currentApple.Destroyed += OnAppleDestroyed;
    }

    private void OnAppleDestroyed(Apple apple)
    {
        _currentApple.Destroyed -= OnAppleDestroyed;
        _currentApple = null;
        
        TryFindNewApple();
    }

    private void TryFindNewApple()
    {
        if (_appleHandler.TryGetRandomApple(out _currentApple))
        {
            _snakeRotation.SetRotateDirection(_currentApple.transform.position - transform.position);
            _snakeRotation.SetRotateDirection(_currentApple.transform.position);
            _currentApple.Destroyed += OnAppleDestroyed;
            Debug.Log(_currentApple.transform.position);
        }
    }
}
