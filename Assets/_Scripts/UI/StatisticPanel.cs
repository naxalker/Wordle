using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class StatisticPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text _totalGamesPlayedText;
    [SerializeField] private TMP_Text _totalWinsText;
    [SerializeField] private TMP_Text _currentWinStreakText;
    [SerializeField] private TMP_Text _bestWinStreakText;
    [SerializeField] private TMP_Text _averageAttemptsText;
    [SerializeField] private TMP_Text _fastestSolveTimeText;
    [SerializeField] private TMP_Text _totalTimeText;

    [SerializeField] private Button _returnButton;

    private PlayerStatistic _playerStatistic;

    [Inject]
    private void Construct(PlayerStatistic playerStatistic)
    {
        _playerStatistic = playerStatistic;
    }

    private void OnEnable()
    {
        _playerStatistic.OnTotalTimeValueChanged += TotalTimeValueChangedHandler;   
    }

    private void Start()
    {
        _returnButton.onClick.AddListener(() => Hide());
    }

    private void OnDisable()
    {
        _playerStatistic.OnTotalTimeValueChanged -= TotalTimeValueChangedHandler;
    }

    public void Show()
    {
        gameObject.SetActive(true);

        _totalGamesPlayedText.text = GetPlayedGamesText(_playerStatistic.TotalGamesPlayed);
        _totalWinsText.text = _playerStatistic.TotalWins.ToString();
        _currentWinStreakText.text = _playerStatistic.CurrentWinStreak.ToString();
        _bestWinStreakText.text = _playerStatistic.BestWinStreak.ToString();

        if (_playerStatistic.TotalWins > 0)
        {
            _averageAttemptsText.text = Mathf.RoundToInt(_playerStatistic.TotalAttempts / _playerStatistic.TotalWins).ToString();
        }
        else
        {
            _averageAttemptsText.text = "-";
        }

        if (_playerStatistic.FastestSolveTime != Mathf.Infinity)
        {
            _fastestSolveTimeText.text = GetFormattedTime(_playerStatistic.FastestSolveTime);
        }
        else
        {
            _fastestSolveTimeText.text = "-";
        }
        
        _totalWinsText.text = _playerStatistic.TotalGamesPlayed.ToString();

        _totalTimeText.text = GetFormattedTime(_playerStatistic.TotalTimePlayed);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void TotalTimeValueChangedHandler(float time)
    {
        _totalTimeText.text = GetFormattedTime(time);
    }

    private string GetFormattedTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);

        return $"{minutes:00}:{seconds:00}";
    }

    private string GetPlayedGamesText(int totalGames)
    {
        string gameWord;

        if (totalGames % 100 >= 11 && totalGames % 100 <= 19)
        {
            gameWord = "игр";
        }
        else
        {
            int lastDigit = totalGames % 10;

            if (lastDigit == 1)
                gameWord = "игру";
            else if (lastDigit >= 2 && lastDigit <= 4)
                gameWord = "игры";
            else
                gameWord = "игр";
        }

        return $"Ты сыграл уже - <b>{totalGames} {gameWord}</b>";
    }
}
