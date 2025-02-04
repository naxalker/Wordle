using System;
using UnityEngine.UI;
using YG;
using Zenject;

public class AdController : IInitializable, IDisposable
{
    private Board _board;
    private MessagePanel _messagePanel;
    private Button _hintButton;

    public AdController(Board board, MessagePanel messagePanel, Button hintButton)
    {
        _board = board;
        _messagePanel = messagePanel;
        _hintButton = hintButton;
    }

    public void Initialize()
    {
        _hintButton.onClick.AddListener(ShowRewAd);
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

            _hintButton.interactable = false;
        });
    }

    private void NewGameStartedHandler()
    {
        _hintButton.interactable = true;
        YG2.InterstitialAdvShow();
    }
}
