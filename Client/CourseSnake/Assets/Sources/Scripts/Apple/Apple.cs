using System;
using UnityEngine;

public class Apple : MonoBehaviour
{
    [SerializeField] private float _reward = 1;

    public event Action<Apple> Destroyed;

    private void Awake()
    {
        SetReward(_reward);
    }

    public void SetReward(float reward)
    {
        float minReward = 0.1f;

        if(reward < minReward)
            reward = minReward;

        _reward = reward;
        transform.localScale = new Vector3(_reward, _reward, _reward);
    }

    public void RemoveApple()
    {
        Destroyed?.Invoke(this);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out SnakeView snakeView))
        {
            snakeView.AddScore(_reward);
            RemoveApple();
        }
    }
}
