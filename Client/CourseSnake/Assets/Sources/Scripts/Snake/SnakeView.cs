using System;
using UnityEngine;

public class SnakeView : MonoBehaviour
{
    [SerializeField] private MeshRenderer _headMesh;
    private SnakeScorePresenter _snakeScorePresenter;
    private SnakeBodyParts _snakeBodyParts;
    private SnakeNameView _snakeNameView;
    private bool _isInitialized;

    public event Action<SnakeView, float> ScoreChanged;
    public event Action Destroyed;

    public string Id { get; private set; }
    public string Login => _snakeNameView.Login;

    public void Init(SnakeScorePresenter snakeScorePresenter, 
        SnakeBodyParts snakeBodyParts, 
        SnakeNameView snakeNameView,
        Color color, 
        string id)
    {
        gameObject.SetActive(false);

        _snakeScorePresenter = snakeScorePresenter;
        _snakeBodyParts = snakeBodyParts;
        _snakeNameView = snakeNameView;
        _headMesh.material.color = color;
        Id = id;
        _isInitialized = true;

        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        if (_isInitialized == false)
            return;

        _snakeScorePresenter.Enable();
        _snakeScorePresenter.ScoreChanged += OnScoreChanged;
        _snakeBodyParts.Destroyed += OnSnakeDestroyed;
    }

    private void OnDisable()
    {
        if (_isInitialized == false)
            return;

        _snakeScorePresenter.Disable();
        _snakeScorePresenter.ScoreChanged -= OnScoreChanged;
        _snakeBodyParts.Destroyed -= OnSnakeDestroyed;
    }

    public void Destroy()
    {
        _snakeBodyParts.Destroy();
    }

    public void AddScore(float value)
    {
        _snakeScorePresenter.AddScore(value);
    }

    public void SetScore(float value)
    {
        _snakeScorePresenter.SetScore(value);
    }

    private void OnSnakeDestroyed()
    {
        Destroyed?.Invoke();
    }

    private void OnScoreChanged(float score)
    {
        ScoreChanged?.Invoke(this, score);
    }
}
