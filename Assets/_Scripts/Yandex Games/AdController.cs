using System;
using YG;
using Zenject;

public class AdController : IInitializable, IDisposable
{
    private Board _board;

    public AdController(Board board)
    {
        _board = board;
    }

    public void Initialize()
    {
        _board.OnNewGameStarted += NewGameStartedHandler;
    }

    public void Dispose()
    {
        _board.OnNewGameStarted -= NewGameStartedHandler;
    }

    private void NewGameStartedHandler()
    {
        YG2.InterstitialAdvShow();
    }
}
