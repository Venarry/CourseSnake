using System;
using UnityEngine;

public class SnakeRotation : MonoBehaviour
{
    private float _rotateSpeed = 60;
    private SnakeMovement _snakeMovement;
    private Vector3 _targetPoint;

    public event Action<Vector3> RotationChanged;
    public event Action<Vector3> TargetPointSet;

    private float MovementSpeed => _snakeMovement.CurrentSpeed;
    public Vector3 TargetPoint => _targetPoint;

    private void Awake()
    {
        _snakeMovement = GetComponent<SnakeMovement>();
    }

    public void Update()
    {
        Rotate();
    }

    public void SetRotateDirection(Vector3 point)
    {
        _targetPoint = point;
        _targetPoint = new(_targetPoint.x, 0, _targetPoint.z);
        TargetPointSet?.Invoke(_targetPoint);
    }

    public void SetRotation(Vector3 rotation)
    {
        transform.rotation = Quaternion.Euler(rotation);
    }

    private void Rotate()
    {
        Quaternion previousRotation = transform.rotation;

        Vector3 rotateDirection = _targetPoint.normalized;
        Quaternion targetRotation;

        if (rotateDirection != Vector3.zero)
            targetRotation = Quaternion.LookRotation(rotateDirection);
        else
            targetRotation = Quaternion.LookRotation(transform.forward);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotateSpeed * MovementSpeed * Time.deltaTime);

        if (previousRotation != transform.rotation)
            RotationChanged?.Invoke(transform.rotation.eulerAngles);
    }
}
