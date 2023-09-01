using System;
using UnityEngine;

public class SnakeDieReaction : MonoBehaviour
{
    [SerializeField] private PlayerSpawnInitiator _spawnInitiator;

    private ISnakeSpawnHandler _spawnHandler;
    private SnakeView _currentSnake;
    private bool _isInitialized;

    public void Init(ISnakeSpawnHandler snakeSpawnHandler)
    {
        gameObject.SetActive(false);

        _spawnHandler = snakeSpawnHandler;
        _isInitialized = true;

        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        if (_isInitialized == false)
            return;

        _spawnHandler.PlayerSpawned += OnSnakeSpawn;
    }

    private void OnDisable()
    {
        if (_isInitialized == false)
            return;

        _spawnHandler.PlayerSpawned -= OnSnakeSpawn;
    }

    private void OnSnakeSpawn(SnakeView snake)
    {
        if (_currentSnake != null)
            throw new ArgumentException();

        _currentSnake = snake;
        snake.Destroyed += OnSnakeDestroy;
    }

    private void OnSnakeDestroy()
    {
        _spawnInitiator.SetMenuState(true);
        _currentSnake = null;
    }
}
