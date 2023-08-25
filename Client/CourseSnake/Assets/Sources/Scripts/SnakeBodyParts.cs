using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SnakeBodyParts : MonoBehaviour
{
    [SerializeField] private float _distanceForSave = 1;
    [SerializeField] private List<Transform> _snakeParts;
    [SerializeField] private Transform _head;

    private List<Vector3> _history;
    private SnakeFactory _snakeFactory;
    private Vector3 _partSpawnPosition => _history[_history.Count - 1];

    private void Awake()
    {
        _history = new List<Vector3>();
        _history.Add(_head.position);

        for (int i = 0; i < _snakeParts.Count; i++)
        {
            _history.Add(_snakeParts[i].position);
        }
    }

    public void Init(SnakeFactory snakeFactory)
    {
        _snakeFactory = snakeFactory;

        Transform tail = _snakeFactory.CreateTail(_partSpawnPosition);
        Add(tail);
    }

    private void Update()
    {
        TransformBody();

        if (Input.GetKeyDown(KeyCode.E))
        {
            AddPart();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            RemovePart();
        }
    }

    public void AddPart()
    {
        Transform bodyPart = _snakeFactory.CreateBody(_history[_history.Count - 1]);
        Add(bodyPart);
    }

    public void SetBodyPart(int value)
    {
        if (_snakeParts.Count == value)
            return;

        if(_snakeParts.Count < value)
        {
            for (int i = 0; i < value - _snakeParts.Count; i++)
            {
                AddPart();
            }
        }
        else
        {
            for (int i = 0; i < value - _snakeParts.Count; i++)
            {
                RemovePart();
            }
        }
    }

    private void Add(Transform part)
    {
        int targetSlotIndex;

        if (_snakeParts.Count == 0)
            targetSlotIndex = 0;
        else
            targetSlotIndex = _snakeParts.Count - 1;

        _snakeParts.Insert(targetSlotIndex, part);
        _history.Add(part.position);

        Resize();
    }

    public void RemovePart()
    {
        if (_snakeParts.Count < 2)
            return;

        Transform targetSlot = _snakeParts[_snakeParts.Count - 2];
        Destroy(targetSlot.gameObject);
        _snakeParts.Remove(targetSlot);
        _history.RemoveAt(_history.Count - 1);

        Resize();
    }

    private void Resize()
    {
        float minSize = 0.6f;

        for (int i = 0; i < _snakeParts.Count; i++)
        {
            float size = Math.Clamp((float)(_snakeParts.Count - i) / _snakeParts.Count, minSize, 1);
            _snakeParts[i].localScale = new Vector3(size, size, size);
        }
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

            Vector3 lookDirection = _history[i + 1] - _history[i];
            Quaternion targetRotation;
            if (lookDirection != Vector3.zero)
                targetRotation = Quaternion.LookRotation(lookDirection);
            else
                targetRotation = Quaternion.identity;

            _snakeParts[i].rotation = Quaternion.Lerp(_snakeParts[i].rotation, targetRotation, deltaDistance / _distanceForSave);
        }
    }
}
