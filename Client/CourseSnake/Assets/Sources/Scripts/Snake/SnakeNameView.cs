using UnityEngine;
using TMPro;

public class SnakeNameView : MonoBehaviour
{
    [SerializeField] private TMP_Text _name;
    [SerializeField] private LookAtRotator _lookAtRotator;

    public string Login => _name.text;

    public void SetName(string name)
    {
        _name.text = name;
    }

    public void LookAtTarget(Transform target)
    {
        _lookAtRotator.SetTarget(target, false);
    }
}
