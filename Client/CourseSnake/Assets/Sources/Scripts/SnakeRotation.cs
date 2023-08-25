using UnityEngine;

public class SnakeRotation : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed = 150;
    [SerializeField] private SnakeMovement _snakeMovement;

    private Vector3 _targetPosition;
    private Plane _plane;
    private Camera _camera;

    private float _movementSpeed => _snakeMovement.CurrentSpeed;

    private void Awake()
    {
        _plane = new Plane(Vector3.up, Vector3.zero);
    }

    public void Init(Camera camera)
    {
        _camera = camera;
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

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotateSpeed * _movementSpeed * Time.deltaTime);
    }
}
