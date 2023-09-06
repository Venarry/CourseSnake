using System;
using UnityEngine;
using YG;

public class SnakeDieReaction : MonoBehaviour
{
    [SerializeField] private PlayerSpawnInitiator _spawnInitiator;

    private ISnakeHandler _spawnHandler;
    private YandexGame _yandexGame;
    private SnakeView _currentSnake;
    private bool _isInitialized;

    public void Init(ISnakeHandler snakeSpawnHandler, YandexGame yandexGame)
    {
        gameObject.SetActive(false);

        _spawnHandler = snakeSpawnHandler;
        _yandexGame = yandexGame;
        _isInitialized = true;

        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        if (_isInitialized == false)
            return;

        _spawnHandler.PlayerSpawned += OnSnakeSpawn;
        _spawnHandler.BotSpawned += OnBotSpawned;
    }

    private void OnDisable()
    {
        if (_isInitialized == false)
            return;

        _spawnHandler.PlayerSpawned -= OnSnakeSpawn;
        _spawnHandler.BotSpawned -= OnBotSpawned;
    }

    private void OnBotSpawned(SnakeView snake)
    {
        snake.Destroyed += OnBotDestroyed;
    }

    private void OnBotDestroyed(SnakeView snake)
    {
        snake.Destroyed -= OnBotDestroyed;

        if (_spawnHandler.CanSpawnBots == false)
            return;

        _spawnInitiator.InitBot();
    }

    private void OnSnakeSpawn(SnakeView snake)
    {
        if (_currentSnake != null)
            throw new ArgumentException();

        _currentSnake = snake;
        snake.Destroyed += OnPlayerDestroy;
    }

    private void OnPlayerDestroy(SnakeView snake)
    {
        _currentSnake.Destroyed -= OnPlayerDestroy;
        _spawnInitiator.SetMenuState(true);
        _yandexGame._FullscreenShow();
        _currentSnake = null;
    }
}
