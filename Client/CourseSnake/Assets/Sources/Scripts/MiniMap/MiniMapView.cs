using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapView : MonoBehaviour
{
    [SerializeField] private Image _playerIconTemplate;
    [SerializeField] private RectTransform _parent;

    private readonly Dictionary<SnakeView, Image> _snakes = new();
    private ISnakeHandler _snakeHandler;
    private Vector2 _mapSize;
    private Vector2 _mapSizeMultiplier;
    private bool _isInitialized;

    public void Init(ISnakeHandler snakeHandler, Vector2 mapSize)
    {
        gameObject.SetActive(false);

        _snakeHandler = snakeHandler;
        _mapSize = mapSize;

        float mapOffset = 10f;

        _mapSizeMultiplier.x = _parent.rect.width / 2 / (_mapSize.x + mapOffset);
        _mapSizeMultiplier.y = _parent.rect.height / 2 / (_mapSize.y + mapOffset);
        _isInitialized = true;

        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        if (_isInitialized == false)
            return;

        _snakeHandler.PlayerSpawned += OnPlayerSpawn;
        _snakeHandler.SnakeSpawned += OnSnakeSpawn;
    }

    private void OnDisable()
    {
        if (_isInitialized == false)
            return;

        _snakeHandler.SnakeSpawned -= OnSnakeSpawn;
    }

    private void Update()
    {
        UpdateSnakesPosition();
    }

    private void UpdateSnakesPosition()
    {
        foreach (var snake in _snakes)
        {
            snake.Value.transform.localPosition = new(
                snake.Key.transform.position.x * _mapSizeMultiplier.x, 
                snake.Key.transform.position.z * _mapSizeMultiplier.y);
            
        }
    }

    private void OnPlayerSpawn(SnakeView snake)
    {
        SpawnSnakeIcon(snake, Color.green);
    }

    private void OnSnakeSpawn(SnakeView snake)
    {
        if(_snakes.ContainsKey(snake))
        {
            return;
        }

        SpawnSnakeIcon(snake, Color.red);
    }

    private void SpawnSnakeIcon(SnakeView snake, Color color)
    {
        Image icon = Instantiate(_playerIconTemplate, _parent);
        icon.color = color;
        _snakes.Add(snake, icon);

        snake.Destroyed += OnSnakeDestroy;
    }

    private void OnSnakeDestroy(SnakeView snake)
    {
        Destroy(_snakes[snake].gameObject);
        _snakes.Remove(snake);
    }
}
