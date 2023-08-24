using UnityEngine;

public class SnakeRotation : MonoBehaviour
{
    [SerializeField] private Transform _targetPositionPoint;
    [SerializeField] private float _rotateSpeed = 150;

    private Vector3 _targetPosition;
    private Plane _plane;
    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
        _plane = new Plane(Vector3.up, Vector3.zero);
    }

    public void Update()
    {
        HandleMouse();
        Rotate();
    }

    private void HandleMouse()
    {
        _plane.Translate(transform.position);
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (_plane.Raycast(ray, out float distance))
        {
            _targetPosition = ray.GetPoint(distance);
            _targetPositionPoint.position = _targetPosition;
        }
    }

    private void Rotate()
    {
        Vector3 moveDirection = _targetPosition - transform.position;
        Quaternion targetRotation;

        if (moveDirection != Vector3.zero)
            targetRotation = Quaternion.LookRotation(moveDirection);
        else
            targetRotation = Quaternion.LookRotation(transform.forward);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotateSpeed * Time.deltaTime);
    }
}
