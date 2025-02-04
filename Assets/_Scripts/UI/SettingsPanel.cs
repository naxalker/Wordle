using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class SettingsPanel : MonoBehaviour
{
    [SerializeField] private Button _exitButton;
    [SerializeField] private Button _resetButton;
    [SerializeField] private ToggleSwitch _toggleSounds;
    [SerializeField] private ToggleSwitch _toggleTheme;
    [SerializeField] private AudioController _audioController;
    [SerializeField] private Board _board;

    private ThemeController _themeController;
    private PlayerStatistic _playerStatistic;
    private WordsController _wordsController;

    [Inject]
    private void Construct(ThemeController themeController, PlayerStatistic playerStatistic, WordsController wordsController)
    {
        _themeController = themeController;
        _playerStatistic = playerStatistic;
        _wordsController = wordsController;
    }

    private void Awake()
    {
        _exitButton.onClick.AddListener(() => gameObject.SetActive(false));
        _resetButton.onClick.AddListener(() =>
        {
            _playerStatistic.ResetProgress();
            _wordsController.ResetProgress();
            _board.StartNewGame();
        });

        _toggleSounds.OnToggled += ToggledSoundsHandler;
        _toggleTheme.OnToggled += ToggledThemeHandler;
    }

    private void OnDestroy()
    {
        _toggleSounds.OnToggled -= ToggledSoundsHandler;
        _toggleTheme.OnToggled -= ToggledThemeHandler;
    }

    private void ToggledSoundsHandler(bool isOn)
    {
        _audioController.ToggleSound(isOn);
    }

    private void ToggledThemeHandler(bool isOn)
    {
        _themeController.ChangeColors();
    }
}
