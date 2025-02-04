using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class Header : MonoBehaviour
{
    [Header("Help")]
    [SerializeField] private Button _helpButton;
    [SerializeField] private HelpPanel _helpPanel;

    [Header("Statistic")]
    [SerializeField] private Button _statButton;
    [SerializeField] private StatisticPanel _statisticPanel;

    [Header("Progress Text")]
    [SerializeField] private TMP_Text _progressText;

    [Header("Settings")]
    [SerializeField] private Button _settingsButton;
    [SerializeField] private SettingsPanel _settingsPanel;

    [Header("Other References")]
    [SerializeField] private Board _board;

    private WordsController _playerProgress;

    [Inject]
    private void Construct(WordsController playerProgress)
    {
        _playerProgress = playerProgress;
    }

    private void Start()
    {
        _board.OnNewGameStarted += NewGameStartedHandler;

        _helpButton.onClick.AddListener(() => _helpPanel.Show());
        _statButton.onClick.AddListener(() => _statisticPanel.Show());
        _settingsButton.onClick.AddListener(() => _settingsPanel.gameObject.SetActive(true));
    }

    private void OnDestroy()
    {
        _board.OnNewGameStarted -= NewGameStartedHandler;
    }

    private void NewGameStartedHandler()
    {
        _progressText.text = $"Слов отгадано\n{_playerProgress.GuessedWordsAmount}/{WordsController.WORDS_TO_GUESS_AMOUNT}";
    }
}
