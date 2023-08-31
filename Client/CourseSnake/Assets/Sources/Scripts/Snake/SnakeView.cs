using System;
using UnityEngine;

public class SnakeView : MonoBehaviour
{
    [SerializeField] private MeshRenderer _headMesh;
    private SnakeScorePresenter _snakeScorePresenter;
    private SnakeBodyParts _snakeBodyParts;
    private bool _isInitialized;

    public event Action Destroyed;

    public void Init(SnakeScorePresenter snakeScorePresenter, SnakeBodyParts snakeBodyParts, Color color)
    {
        gameObject.SetActive(false);

        _snakeScorePresenter = snakeScorePresenter;
        _snakeBodyParts = snakeBodyParts;
        _headMesh.material.color = color;
        _isInitialized = true;

        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        if (_isInitialized == false)
            return;

        _snakeScorePresenter.Enable();
        _snakeBodyParts.Destroyed += OnSnakeDestroyed;
    }

    private void OnDisable()
    {
        if (_isInitialized == false)
            return;

        _snakeScorePresenter.Disable();
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out DeathBarrier _))
        {
            Destroy();
        }
    }
}
