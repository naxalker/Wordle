using System;
using YG;
using Zenject;

public class AdController : IInitializable, IDisposable
{
    private Board _board;
    private MessagePanel _messagePanel;
    private HintButton _hintButton;

    public AdController(Board board, MessagePanel messagePanel, HintButton hintButton)
    {
        _board = board;
        _messagePanel = messagePanel;
        _hintButton = hintButton;
    }

    public void Initialize()
    {
        _hintButton.Setup(ShowRewAd);
        _board.OnNewGameStarted += NewGameStartedHandler;
    }

    public void Dispose()
    {
        _board.OnNewGameStarted -= NewGameStartedHandler;
    }

    private void ShowRewAd()
    {
        YG2.RewardedAdvShow("giveHint", () =>
        {
            string[] positions = { "Первая", "Вторая", "Третья", "Четвертая", "Пятая" };

            for (int i = 0; i < _board.GuessedLettersInWord.Length; i++)
            {
                if (_board.GuessedLettersInWord[i] == '\0')
                {
                    _messagePanel.ShowMessage($"{positions[i]} буква - <color=#538D4E><b>{char.ToUpper(_board.Word[i])}</b></color>");
                    break;
                }
            }

            _hintButton.Disable();
        });
    }

    private void NewGameStartedHandler()
    {
        _hintButton.Enable();
        YG2.InterstitialAdvShow();
    }
}
