using TMPro;
using UnityEngine;
using Zenject;

public class Header : MonoBehaviour
{
    [SerializeField] private TMP_Text _progressText;
    [SerializeField] private Board _board;

    private PlayerProgressController _playerProgress;

    [Inject]
    private void Construct(PlayerProgressController playerProgress)
    {
        _playerProgress = playerProgress;
    }

    private void Start()
    {
        _progressText.text = $"Уровень\n{_playerProgress.GuessedWordsAmount + 1}/1000";

        _board.OnNewGameStarted += NewGameStartedHandler;
    }

    private void OnDestroy()
    {
        _board.OnNewGameStarted -= NewGameStartedHandler;
    }

    private void NewGameStartedHandler()
    {
        _progressText.text = $"Уровень\n{_playerProgress.GuessedWordsAmount + 1}/1000";
    }
}
