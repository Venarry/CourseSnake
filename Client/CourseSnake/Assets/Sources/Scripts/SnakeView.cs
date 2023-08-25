using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SnakeBodyParts))]
[RequireComponent(typeof(SnakeMovement))]
[RequireComponent(typeof(SnakeRotation))]
public class SnakeView : MonoBehaviour
{
    private SnakeMovement _snakeMovement;
    private SnakeBodyParts _snakeBodyParts;

    private void Awake()
    {
        _snakeBodyParts = GetComponent<SnakeBodyParts>();
    }

    private void OnEnable()
    {
        
    }

    public void AddBodyPart()
    {
        _snakeBodyParts.AddBodyPart();
    }
}
