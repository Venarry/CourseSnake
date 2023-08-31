using System;
using UnityEngine;

public class SnakeDieReaction : MonoBehaviour
{
    [SerializeField] private PlayerSpawnInitiator _spawnInitiator;

    private ISnakeSpawnHandler _spawnHandler;
    private SnakeView _currentSnake;
    private bool _isInitialised;

    public void Init(ISnakeSpawnHandler snakeSpawnHandler)
    {
        gameObject.SetActive(false);

        _spawnHandler = snakeSpawnHandler;
        _isInitialised = true;

        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        if (_isInitialised == false)
            return;

        _spawnHandler.SnakeSpawned += OnSnakeSpawn;
    }

    private void OnDisable()
    {
        if (_isInitialised == false)
            return;

        _spawnHandler.SnakeSpawned -= OnSnakeSpawn;
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
