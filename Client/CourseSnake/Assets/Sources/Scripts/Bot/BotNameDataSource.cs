using UnityEngine;

public class BotNameDataSource
{
    private static string[] _names = new string[]
    {
        "�����",
        "������",
        "dronoff12",
        "s2",
        "Venarry",
        "killer",
        "viktuzz",
        "player",
        "sssssss",
        "����",
        "������",
        "��� ������� �������",
        "� �� ���",
        "� ��� �������",
        "� �� ���� ��� �� 13",
        "�����",
        "1000-7",
        "0_o",
        "��������",
        "����� ����",
        "������",
        "spidermen",
        "425234",
        "asd",
        "�� �������� ����",
    };

    public static string GetRandomName() =>
        _names[Random.Range(0, _names.Length)];
}
