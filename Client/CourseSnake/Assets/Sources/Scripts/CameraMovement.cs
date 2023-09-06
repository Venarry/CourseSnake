using System;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Vector3 _offset;
    [SerializeField] private Vector3 _nontargetPosition;
    private SnakeScorePresenter _scorePresenter;
    private SnakeMovement _snakeMovement;
    private Transform _target;
    private float _scoreMultiplier;
    private float _targetScoreMultiplier;
    private bool _isInitialized;

    public void Init(SnakeScorePresenter scorePresenter, SnakeMovement snakeMovement)
    {
        gameObject.SetActive(false);

        _scorePresenter = scorePresenter;
        _snakeMovement = snakeMovement;
        OnScoreChange(_scorePresenter.Score);
        _isInitialized = true;

        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        if (_isInitialized == false)
            return;

        _scorePresenter.ScoreChanged += OnScoreChange;
        _snakeMovement.PositionChanged += OnPositionChange;
    }

    private void OnPositionChange(Vector3 position)
    {
        transform.position = position + _offset + (_offset * _targetScoreMultiplier);
    }

    private void OnDisable()
    {
        if (_isInitialized == false)
            return;

        _scorePresenter.ScoreChanged -= OnScoreChange;
        _snakeMovement.PositionChanged -= OnPositionChange;
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    public void OnScoreChange(float score)
    {
        if (score > GameConfig.MaxSnakeLength)
            score = GameConfig.MaxSnakeLength;

        float multiplier = 0.005f;
        _scoreMultiplier = score * multiplier;
    }

    private void LateUpdate()
    {
        _targetScoreMultiplier = Mathf.Lerp(_targetScoreMultiplier, _scoreMultiplier, 0.2f);

        if (_target == null)
        {
            transform.position = Vector3.Lerp(transform.position, _nontargetPosition, 0.01f);
            return;
        }

        //transform.position = _target.position + _offset + (_offset * _targetScoreMultiplier);
    }
}
