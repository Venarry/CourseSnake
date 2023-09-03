using UnityEngine;

public class BotNameDataSource
{
    private static string[] _names = new string[]
    {
        "Матье",
        "Владос",
        "dronoff12",
        "s2",
        "Venarry",
        "killer",
        "viktuzz",
        "player",
        "sssssss",
        "Маша",
        "Сэргей",
        "сын маминой подруги",
        "я не бот",
        "я бот разраба",
        "я не знал что ей 13",
    };

    public static string GetRandomName() =>
        _names[Random.Range(0, _names.Length)];
}
