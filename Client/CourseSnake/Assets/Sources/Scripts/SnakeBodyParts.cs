using System;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBodyParts : MonoBehaviour
{
    [SerializeField] private float _distanceForSave = 1;
    [SerializeField] private List<Vector3> _history;
    [SerializeField] private List<Transform> _snakeParts;
    [SerializeField] private Transform _head;

    private void Awake()
    {
        _history.Add(_head.position);

        for (int i = 0; i < _snakeParts.Count; i++)
        {
            _history.Add(_snakeParts[i].position);
        }
    }

    private void Update()
    {
        TransformBody();

        if (Input.GetKeyDown(KeyCode.E))
        {
            Add();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Remove();
        }
    }

    public void Add()
    {
        Transform bodyPartPrefab = Resources.Load<Transform>(ResourcesPath.SnakeBodyPart);
        Transform bodyPart = Instantiate(bodyPartPrefab, _history[_history.Count - 1], Quaternion.identity);

        int lastAvailableSlotIndex = _snakeParts.Count - 1;
        _snakeParts.Insert(lastAvailableSlotIndex, bodyPart);
        _history.Add(bodyPart.position);
    }

    public void Remove()
    {
        if (_snakeParts.Count < 2)
            return;

        Transform lastAvailableSlot = _snakeParts[_snakeParts.Count - 2];
        Destroy(lastAvailableSlot.gameObject);
        _snakeParts.Remove(lastAvailableSlot);
        _history.RemoveAt(_history.Count - 1);
    }

    private void TransformBody()
    {
        float deltaDistance = (_head.position - _history[0]).magnitude;

        if (deltaDistance > _distanceForSave)
        {
            Vector3 direction = (_head.position - _history[0]).normalized;

            _history.Insert(0, _history[0] + direction * _distanceForSave);
            _history.RemoveAt(_history.Count - 1);

            deltaDistance -= _distanceForSave;
        }

        for (int i = 0; i < _snakeParts.Count; i++)
        {
            _snakeParts[i].position = Vector3.Lerp(_history[i + 1], _history[i], deltaDistance / _distanceForSave);
        }
    }
}
