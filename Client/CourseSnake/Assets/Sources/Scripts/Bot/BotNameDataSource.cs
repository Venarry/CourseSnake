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
        "ОБЭМЭ",
        "1000-7",
        "0_o",
        "Креветка",
        "Люблю колу",
        "амогус",
        "spidermen",
        "425234",
        "asd",
        "не трогайте меня",
    };

    public static string GetRandomName() =>
        _names[Random.Range(0, _names.Length)];
}
