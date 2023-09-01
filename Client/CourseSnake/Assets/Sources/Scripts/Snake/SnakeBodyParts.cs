using System;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBodyParts : MonoBehaviour
{
    [SerializeField] private float _distanceForSave = 1;

    private List<SnakeBody> _snakeParts = new();
    private List<Vector3> _positionHistory;
    private List<Quaternion> _rotationHistory;
    private SnakeFactory _snakeFactory;
    private Color _color;
    private bool _isInitialized;

    public event Action Destroyed;

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
    }

    public void Init(SnakeFactory snakeFactory, Color color)
    {
        _snakeFactory = snakeFactory;
        _color = color;
        _isInitialized = true;

        SnakeBody tail = _snakeFactory.CreateTail(PartSpawnPosition, _color, this);
        Add(tail);
    }

    private void Update()
    {
        TransformBody();
    }

    public void Destroy()
    {
        foreach (var part in _snakeParts)
        {
            part.Destroy();
        }

        Destroyed?.Invoke();
        Destroy(gameObject);
    }

    public void SetBodyPart(int value)
    {
        int partsCount = _snakeParts.Count - 1;

        if (partsCount == value)
            return;

        if (partsCount < value)
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

    private void AddPart()
    {
        if (_isInitialized == false)
            return;

        SnakeBody bodyPart = _snakeFactory.CreateBody(PartSpawnPosition, _color, this);
        Add(bodyPart);
    }

    private void Add(SnakeBody part)
    {
        int targetSlotIndex;

        if (_snakeParts.Count == 0)
            targetSlotIndex = 0;
        else
            targetSlotIndex = _snakeParts.Count - 1;

        _snakeParts.Insert(targetSlotIndex, part);
        _positionHistory.Add(part.transform.position);
        _rotationHistory.Add(part.transform.rotation);

        Resize();
    }

    private void RemovePart()
    {
        if (_snakeParts.Count < 2)
            return;

        SnakeBody targetSlot = _snakeParts[_snakeParts.Count - 2];
        Destroy(targetSlot.gameObject);
        _snakeParts.Remove(targetSlot);
        _positionHistory.RemoveAt(_positionHistory.Count - 1);
        _rotationHistory.RemoveAt(_rotationHistory.Count - 1);

        Resize();
    }

    private void Resize()
    {
        float minSize = 0.7f;

        for (int i = 0; i < _snakeParts.Count; i++)
        {
            float size = Math.Clamp((float)(_snakeParts.Count - i) / _snakeParts.Count, minSize, 1);
            _snakeParts[i].transform.localScale = new Vector3(size, size, size);
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

            _snakeParts[i].transform.position = Vector3.Lerp(_positionHistory[i + 1], _positionHistory[i], progress);
            _snakeParts[i].transform.rotation = Quaternion.Lerp(_rotationHistory[i + 1], _rotationHistory[i], progress);
        }
    }
}
