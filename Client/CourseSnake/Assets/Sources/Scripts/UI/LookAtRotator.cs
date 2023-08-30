using UnityEngine;

public class LookAtRotator : MonoBehaviour
{
    private Transform _target;
    private int _lookDirection = 1;

    public void SetTarget(Transform target, bool isInvert)
    {
        _target = target;

        if(isInvert == true)
        {
            _lookDirection = -1;
        }
        else
        {
            _lookDirection = 1;
        }
    }

    private void LateUpdate()
    {
        if (_target == null)
            return;

        transform.forward = _target.transform.forward * _lookDirection;
    }
}
