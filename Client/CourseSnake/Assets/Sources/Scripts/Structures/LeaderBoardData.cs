public struct LeaderBoardData
{
    public string Login;
    public int Score;

    public LeaderBoardData(string login, int score)
    {
        Login = login;
        Score = score;
    }

    public void SetScore(int value)
    {
        if(value < 0)
            value = 0;

        Score = value;
    }
}
