using UnityEngine;

public class SnakeView : MonoBehaviour
{
    [SerializeField] private MeshRenderer _headMesh;
    private SnakeScorePresenter _snakeScorePresenter;
    private bool _isInitialized;

    public void Init(SnakeScorePresenter snakeScorePresenter, Color color)
    {
        gameObject.SetActive(false);

        _snakeScorePresenter = snakeScorePresenter;
        _headMesh.material.color = color;
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

    public void SetScore(float value)
    {
        _snakeScorePresenter.SetScore(value);
    }
}
