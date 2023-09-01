using UnityEngine;

public class LeaderBoardPlayerDataFactory
{
    private readonly LeaderBoardPlayer _prefab = Resources.Load<LeaderBoardPlayer>(ResourcesPath.LeaderBoardPlayer);

    public LeaderBoardPlayer Create(Transform parent, string login)
    {
        LeaderBoardPlayer leaderBoardPlayer = Object.Instantiate(_prefab);
        leaderBoardPlayer.Init(login);
        leaderBoardPlayer.transform.SetParent(parent);
        leaderBoardPlayer.transform.localScale = Vector3.one;

        return leaderBoardPlayer;
    }
}
