using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Vector3 _offset;
    private Transform _target;

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    private void LateUpdate()
    {
        if (_target == null)
            return;

        transform.position = _target.position + _offset;
    }
}
