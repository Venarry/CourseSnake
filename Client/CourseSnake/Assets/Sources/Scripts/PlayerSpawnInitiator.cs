using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerSpawnInitiator : MonoBehaviour
{
    [SerializeField] private GameObject _menu;
    [SerializeField] private TMP_InputField _name;
    [SerializeField] private Button _startGameButton;

    private Color _snakeColor = new(1, 1, 1);
    private Vector2 _spawnArea;

    public event Action<Vector3, string, Color> PlayerSpawnInited;

    private void OnEnable()
    {
        _startGameButton.onClick.AddListener(InitPlayer);
    }

    private void OnDisable()
    {
        _startGameButton.onClick.RemoveListener(InitPlayer);
    }

    public void SetMenuState(bool state)
    {
        _menu.SetActive(state);
    }

    public void SetColor(Image image)
    {
        _snakeColor = image.color;
    }

    public void SetArea(Vector2 area)
    {
        _spawnArea = area;
    }

    private void InitPlayer()
    {
        float targetWidthPosition = UnityEngine.Random.Range(-_spawnArea.x, _spawnArea.x);
        float targetHeightPosition = UnityEngine.Random.Range(-_spawnArea.y, _spawnArea.y);
        Vector3 spawnPosition = new(targetWidthPosition, 0, targetHeightPosition);

        string name = _name.text;

        if (name == "")
            name = "player";

        SetMenuState(false);

        PlayerSpawnInited?.Invoke(spawnPosition, name, _snakeColor);
    }
}
