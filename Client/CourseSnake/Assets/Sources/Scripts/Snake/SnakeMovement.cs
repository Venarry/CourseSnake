using System;
using UnityEngine;

public class SnakeMovement : MonoBehaviour
{
    [SerializeField] private float _defaultSpeed = 8;
    [SerializeField] private float _boostSpeedMultiplayer = 2.5f;

    private bool _boosted;

    public event Action<float> BoostUsed;
    public event Action<bool> BoostStateChanged;
    public event Action<Vector3> PositionChanged;

    public float CurrentSpeed { get; private set; }

    private void Update()
    {
        CurrentSpeed = _defaultSpeed;

        if (_boosted)
        {
            CurrentSpeed = _defaultSpeed * _boostSpeedMultiplayer;
            BoostUsed?.Invoke(CurrentSpeed * Time.deltaTime);
        }

        Vector3 moveForce = CurrentSpeed * Time.deltaTime * transform.forward;
        transform.position += moveForce;

        PositionChanged?.Invoke(transform.position);
    }

    public void SetLerpPosition(Vector3 position)
    {
        Vector3 targetPosition = Vector3.Lerp(transform.position, position, 0.25f);
        transform.position = targetPosition;
    }

    public void SetBoostState(bool state)
    {
        _boosted = state;
        BoostStateChanged?.Invoke(state);
    }
}
