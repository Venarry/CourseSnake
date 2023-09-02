using UnityEngine;
using System;

public class PlayerClickHandler : MonoBehaviour
{
    private SnakeMovement _snakeMovement;
    private Plane _plane;
    private Camera _camera;

    public event Action<Vector3> DirectionSet;
    public event Action<bool> BoostStateChanged;

    private void Awake()
    {
        _plane = new Plane(Vector3.up, Vector3.zero);
        _snakeMovement = GetComponent<SnakeMovement>();
    }

    public void Init(Camera camera)
    {
        _camera = camera;
    }

    private void Update()
    {
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
            Vector3 direction = point - transform.position;
            direction.y = 0;
            //_snakeRotation.SetRotateDirection();
            DirectionSet?.Invoke(direction);
        }
    }

    private void SetBoostState(bool state)
    {
        //_snakeMovement.SetBoostState(state);
        BoostStateChanged?.Invoke(state);
    }
}
