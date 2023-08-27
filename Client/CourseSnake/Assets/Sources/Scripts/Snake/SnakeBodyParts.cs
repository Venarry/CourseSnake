using System;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBodyParts : MonoBehaviour
{
    [SerializeField] private float _distanceForSave = 1;
    [SerializeField] private List<Transform> _snakeParts;

    private List<Vector3> _positionHistory;
    private List<Quaternion> _rotationHistory;
    private SnakeFactory _snakeFactory;
    private bool _isInitialized;

    private Vector3 PartSpawnPosition => _positionHistory[_positionHistory.Count - 1];

    private void Awake()
    {
        _positionHistory = new()
        {
            transform.position
        };

        _rotationHistory = new()
        {
            transform.rotation
        };

        for (int i = 0; i < _snakeParts.Count; i++)
        {
            _positionHistory.Add(_snakeParts[i].position);
            _rotationHistory.Add(_snakeParts[i].rotation);
        }
    }

    public void Init(SnakeFactory snakeFactory)
    {
        _snakeFactory = snakeFactory;
        _isInitialized = true;

        Transform tail = _snakeFactory.CreateTail(PartSpawnPosition);
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
        if (_isInitialized == false)
            return;

        Transform bodyPart = _snakeFactory.CreateBody(_positionHistory[_positionHistory.Count - 1]);
        Add(bodyPart);
    }

    public void SetBodyPart(int value)
    {
        int partsCount = _snakeParts.Count;

        if (partsCount == value)
            return;

        if(_snakeParts.Count < value)
        {
            for (int i = 0; i < value - partsCount; i++)
            {
                AddPart();
            }
        }
        else
        {
            for (int i = 0; i < partsCount - value; i++)
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
        _positionHistory.Add(part.position);
        _rotationHistory.Add(part.rotation);

        Resize();
    }

    public void RemovePart()
    {
        if (_snakeParts.Count < 2)
            return;

        Transform targetSlot = _snakeParts[_snakeParts.Count - 2];
        Destroy(targetSlot.gameObject);
        _snakeParts.Remove(targetSlot);
        _positionHistory.RemoveAt(_positionHistory.Count - 1);
        _rotationHistory.RemoveAt(_rotationHistory.Count - 1);

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
        float deltaDistance = (transform.position - _positionHistory[0]).magnitude;

        if (deltaDistance > _distanceForSave)
        {
            Vector3 direction = (transform.position - _positionHistory[0]).normalized;

            _positionHistory.Insert(0, _positionHistory[0] + direction * _distanceForSave);
            _positionHistory.RemoveAt(_positionHistory.Count - 1);

            _rotationHistory.Insert(0, transform.rotation);
            _rotationHistory.RemoveAt(_rotationHistory.Count - 1);

            deltaDistance -= _distanceForSave;
        }

        for (int i = 0; i < _snakeParts.Count; i++)
        {
            float progress = deltaDistance / _distanceForSave;
            _snakeParts[i].position = Vector3.Lerp(_positionHistory[i + 1], _positionHistory[i], progress);
            _snakeParts[i].rotation = Quaternion.Lerp(_rotationHistory[i + 1], _rotationHistory[i], progress);
        }
    }
}
