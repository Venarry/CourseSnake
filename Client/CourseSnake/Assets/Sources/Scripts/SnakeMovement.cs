using System;
using UnityEngine;

public class SnakeMovement : MonoBehaviour
{
    [SerializeField] private float _defaultSpeed = 8;
    [SerializeField] private float _boostSpeedMultiplayer = 2.5f;

    public event Action BoostUsed;

    public float CurrentSpeed { get; private set; }

    private void Update()
    {
        CurrentSpeed = _defaultSpeed;

        if (Input.GetMouseButton(0))
        {
            CurrentSpeed = _defaultSpeed * _boostSpeedMultiplayer;
        }

        transform.position += CurrentSpeed * Time.deltaTime * transform.forward;
    }
}
