using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 6;
    [SerializeField] private float _boostSpeedMultiplayer = 2.5f;

    public float Speed => _speed;

    private void Update()
    {
        float targetSpeed = _speed;

        if (Input.GetMouseButton(0))
        {
            targetSpeed = _speed * _boostSpeedMultiplayer;
        }

        transform.position += targetSpeed * Time.deltaTime * transform.forward;
    }
}
