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

    [Header("Progress Text")]
    [SerializeField] private TMP_Text _progressText;

    [Header("Theme")]
    [SerializeField] private Button _themeButton;

    [Header("Sound")]
    [SerializeField] private Button _soundButton;
    [SerializeField] private Sprite _soundSprite;
    [SerializeField] private Sprite _muteSprite;

    [Header("Other References")]
    [SerializeField] private Board _board;
    [SerializeField] private AudioController _audioController;

    private WordsController _playerProgress;
    private ThemeController _themeController;

    [Inject]
    private void Construct(WordsController playerProgress, ThemeController themeController)
    {
        _playerProgress = playerProgress;
        _themeController = themeController;
    }

    private void Start()
    {
        _progressText.text = $"Слов отгадано\n{_playerProgress.GuessedWordsAmount + 1}/1000";

        _board.OnNewGameStarted += NewGameStartedHandler;

        _helpButton.onClick.AddListener(() => _helpPanel.Show());
        _themeButton.onClick.AddListener(() => _themeController.ChangeColors());
        _soundButton.onClick.AddListener(() => SoundButtonPressedHandler());
    }

    private void OnDestroy()
    {
        _board.OnNewGameStarted -= NewGameStartedHandler;
    }

    private void NewGameStartedHandler()
    {
        _progressText.text = $"Слов отгадано\n{_playerProgress.GuessedWordsAmount + 1}/1000";
    }

    private void SoundButtonPressedHandler()
    {
        _audioController.ToggleSound();

        _soundButton.GetComponent<Image>().sprite = _audioController.IsMuted ? _muteSprite : _soundSprite;
    }
}
