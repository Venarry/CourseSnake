using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerSpawnInitiator : MonoBehaviour
{
    [SerializeField] private GameObject _menu;
    [SerializeField] private TMP_InputField _name;
    [SerializeField] private Button _startGameButton;
    [SerializeField] private Image[] _colorImages;

    private Color _snakeColor = new(1, 1, 1);
    private Vector2 _spawnArea;

    public event Action<Vector3, string, Color> PlayerSpawnInited;
    public event Action<Vector3, string, Color> BotSpawnInited;

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

    public void InitBots(int count)
    {
        for (int i = 0; i < count; i++)
        {
            InitBot();
        }
    }

    public void InitBot()
    {
        Vector3 spawnPosition = CreateSpawnPosition();

        string name = BotNameDataSource.GetRandomName();
        Color color = _colorImages[UnityEngine.Random.Range(0, _colorImages.Length)].color;

        BotSpawnInited?.Invoke(spawnPosition, name, color);
    }

    private void InitPlayer()
    {
        Vector3 spawnPosition = CreateSpawnPosition();

        string name = _name.text;

        if (name == "")
            name = "player";

        SetMenuState(false);

        PlayerSpawnInited?.Invoke(spawnPosition, name, _snakeColor);
    }

    private Vector3 CreateSpawnPosition()
    {
        float targetWidthPosition = UnityEngine.Random.Range(-_spawnArea.x, _spawnArea.x);
        float targetHeightPosition = UnityEngine.Random.Range(-_spawnArea.y, _spawnArea.y);
        return new Vector3(targetWidthPosition, 0, targetHeightPosition);
    }
}
