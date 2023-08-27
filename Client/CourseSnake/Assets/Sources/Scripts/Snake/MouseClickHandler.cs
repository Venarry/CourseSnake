using UnityEngine;
using System;

public class MouseClickHandler : MonoBehaviour
{
    private SnakeRotation _snakeRotation;
    private SnakeMovement _snakeMovement;
    private Plane _plane;
    private Camera _camera;

    private void Awake()
    {
        _plane = new Plane(Vector3.up, Vector3.zero);
        _snakeRotation = GetComponent<SnakeRotation>();
        _snakeMovement = GetComponent<SnakeMovement>();
    }

    public void Init(Camera camera)
    {
        _camera = camera;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            
        }
        HandleMouse();

        if (Input.GetMouseButtonDown(0))
        {
            SetBoostState(true);
        }

        if (Input.GetMouseButtonUp(0))
        {
            SetBoostState(false);
        }
    }

    private void HandleMouse()
    {
        Vector3 point;
        _plane.Translate(transform.position);
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (_plane.Raycast(ray, out float distance))
        {
            point = ray.GetPoint(distance);
            _snakeRotation.SetTargetPoint(point);
        }
    }

    private void SetBoostState(bool state)
    {
        _snakeMovement.SetBoostState(state);
    }
}
