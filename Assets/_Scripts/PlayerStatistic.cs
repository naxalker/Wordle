using TMPro;
using UnityEngine;

public class PlayerStatistic : MonoBehaviour
{
    private const float TIME_COUNTER_DURATION = 1f;

    private const string TOTAL_GAMES_PLAYED_KEY = "totalGamesPlayed";
    private const string TOTAL_WINS_KEY = "totalWins";
    private const string CURRENT_WIN_STREAK_KEY = "currentWinStreak";
    private const string BEST_WIN_STREAK_KEY = "bestWinStreak";
    private const string TOTAL_ATTEMPTS_KEY = "totalAttempts";
    private const string FASTEST_SOLVE_TIME_KEY = "fastestSolveTime";
    private const string TOTAL_TIME_PLAYED_KEY = "totalTimePlayed";

    [SerializeField] private TMP_Text _totalGamesPlayedText;
    [SerializeField] private TMP_Text _totalWinsText;
    [SerializeField] private TMP_Text _currentWinStreakText;
    [SerializeField] private TMP_Text _bestWinStreakText;
    [SerializeField] private TMP_Text _averageAttemptsText;
    [SerializeField] private TMP_Text _fastestSolveTimeText;
    [SerializeField] private TMP_Text _totalTimeText;

    [SerializeField] private Board _board;

    private int _totalGamesPlayed;
    private int _totalWins;
    private int _currentWinStreak;
    private int _bestWinStreak;
    private int _totalAttempts;
    private float _fastestSolveTime;
    private float _totalTimePlayed;

    private int _currentAttempts = 0;
    private float _currentSessionTime = 0f;
    private float _totalTimeSavingTimer;

    private void Start()
    {
        _totalGamesPlayed = PlayerPrefs.GetInt(TOTAL_GAMES_PLAYED_KEY, 0);
        _totalWins = PlayerPrefs.GetInt(TOTAL_WINS_KEY, 0);
        _currentWinStreak = PlayerPrefs.GetInt(CURRENT_WIN_STREAK_KEY, 0);
        _bestWinStreak = PlayerPrefs.GetInt(BEST_WIN_STREAK_KEY, 0);
        _totalAttempts = PlayerPrefs.GetInt(TOTAL_ATTEMPTS_KEY, 0);
        _fastestSolveTime = PlayerPrefs.GetFloat(FASTEST_SOLVE_TIME_KEY, 0f);
        _totalTimePlayed = PlayerPrefs.GetFloat(TOTAL_TIME_PLAYED_KEY, 0f);

        Show();

        _totalTimeSavingTimer = TIME_COUNTER_DURATION;

        _board.OnGameOver -= GameOverHandler;
        _board.OnValidWordEntered -= ValidWordEnteredHandler;
        _board.OnNewGameStarted -= NewGameStartedHandler;
    }

    private void Update()
    {
        _totalTimeSavingTimer -= Time.deltaTime;
        _currentSessionTime += Time.deltaTime;

        if (_totalTimeSavingTimer <= 0f)
        {
            _totalTimePlayed += TIME_COUNTER_DURATION;

            int minutes = Mathf.FloorToInt(_totalTimePlayed / 60);
            int seconds = Mathf.FloorToInt(_totalTimePlayed % 60);

            _totalTimeText.text = $"{minutes:00}:{seconds:00}";
            PlayerPrefs.SetFloat(TOTAL_TIME_PLAYED_KEY, _totalTimePlayed);

            _totalTimeSavingTimer = TIME_COUNTER_DURATION;
        }
    }

    private void OnDestroy()
    {
        _board.OnGameOver -= GameOverHandler;
        _board.OnValidWordEntered -= ValidWordEnteredHandler;
        _board.OnNewGameStarted -= NewGameStartedHandler;
    }

    public void Show()
    {
        gameObject.SetActive(true);

        _totalGamesPlayedText.text = $"Ты сыграл уже - <b>{_totalGamesPlayed} игр</b>";
        _totalWinsText.text = _totalWins.ToString();
        _currentWinStreakText.text = _currentWinStreak.ToString();
        _bestWinStreakText.text = _bestWinStreak.ToString();

        if (_totalWins > 0)
        {
            _averageAttemptsText.text = Mathf.RoundToInt(_totalAttempts / _totalWins).ToString();
        }
        else
        {
            _averageAttemptsText.text = "-";
        }
        
        _fastestSolveTimeText.text = _fastestSolveTime.ToString();
        _totalWinsText.text = _totalGamesPlayed.ToString();
    }

    public void Hide()
    {

    }

    private void GameOverHandler(bool hasWon, string word)
    {
        _totalGamesPlayed++;

        if (hasWon)
        {
            _totalWins++;
            _currentWinStreak++;

            if (_currentWinStreak > _bestWinStreak)
            {
                _bestWinStreak = _currentWinStreak;
            }

            _totalAttempts += _currentAttempts;

            if (_currentSessionTime < _fastestSolveTime)
            {
                _fastestSolveTime = _currentSessionTime;
            }
        }
        else
        {
            _currentWinStreak = 0;
        }

        SaveStatistic();
    }

    private void ValidWordEnteredHandler()
    {
        _currentAttempts++;
    }

    private void NewGameStartedHandler()
    {
        _currentSessionTime = 0f;
        _currentAttempts = 0;
    }

    private void SaveStatistic()
    {
        PlayerPrefs.SetInt(TOTAL_GAMES_PLAYED_KEY, _totalGamesPlayed);
        PlayerPrefs.SetInt(TOTAL_WINS_KEY, _totalWins);
        PlayerPrefs.SetInt(CURRENT_WIN_STREAK_KEY, _currentWinStreak);
        PlayerPrefs.SetInt(TOTAL_ATTEMPTS_KEY, _totalAttempts);
        PlayerPrefs.SetFloat(FASTEST_SOLVE_TIME_KEY, _fastestSolveTime);
        PlayerPrefs.SetFloat(TOTAL_TIME_PLAYED_KEY, _totalTimePlayed);
    }
}
