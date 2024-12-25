using System;
using YG;
using Zenject;

public class LeaderboardController : IInitializable, IDisposable
{
    private PlayerStatistic _playerStatistic;

    public LeaderboardController(PlayerStatistic playerStatistic)
    {
        _playerStatistic = playerStatistic;
    }

    public void Initialize()
    {
        _playerStatistic.OnTotalWinsValueChanged += TotalWinsValueChangedHandler;
    }

    public void Dispose()
    {
        _playerStatistic.OnTotalWinsValueChanged -= TotalWinsValueChangedHandler;
    }

    private void TotalWinsValueChangedHandler(int winsAmount)
    {
        YG2.SetLeaderboard("totalWins", winsAmount);
    }
}
