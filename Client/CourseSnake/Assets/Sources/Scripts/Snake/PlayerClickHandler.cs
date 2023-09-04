using UnityEngine;
using System;

public class PlayerClickHandler : MonoBehaviour
{
    private Plane _plane;
    private Camera _camera;

    public event Action<Vector3> PointSet;
    public event Action<bool> BoostStateChanged;

    private void Awake()
    {
        _plane = new Plane(Vector3.up, Vector3.zero);
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

            Vector3 targetPoint = point;
            float minPointDistance = 5f;

            if (Vector3.Distance(transform.position, targetPoint) < minPointDistance)
            {
                targetPoint += (targetPoint - transform.position).normalized * minPointDistance;
            }
            //Vector3 direction = point - transform.position;
            //direction.y = 0;

            PointSet?.Invoke(targetPoint);
        }
    }

    private void SetBoostState(bool state)
    {
        BoostStateChanged?.Invoke(state);
    }
}
