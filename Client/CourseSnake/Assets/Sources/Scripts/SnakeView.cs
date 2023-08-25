using UnityEngine;

public class SnakeView : MonoBehaviour
{
    private SnakeScorePresenter _snakeScorePresenter;
    private bool _isInitialized;

    public void Init(SnakeScorePresenter snakeScorePresenter)
    {
        gameObject.SetActive(false);

        _snakeScorePresenter = snakeScorePresenter;
        _isInitialized = true;

        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        if (_isInitialized == false)
            return;

        _snakeScorePresenter.Enable();
    }

    private void OnDisable()
    {
        if (_isInitialized == false)
            return;

        _snakeScorePresenter.Disable();
    }

    public void AddScore(float value)
    {
        _snakeScorePresenter.AddScore(value);
    }
}
