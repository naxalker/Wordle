using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MessagePanel : MonoBehaviour
{
    private static float FADE_DURATION = .5f;
    private static string[] VICTORY_MESSAGES = new string[]
    {
        "Поздравляем! Все верно!",
        "Хорошая работа!",
        "Молодец! Верное слово!",
        "Ты справился! Было тяжело?",
        "Ура! Слово отгадано!"
    };

    //private static string[] LOSE_MESSAGES = new string[]
    //{
    //    "В следующий раз получится!",
    //    "Попробуй снова, ты справишься!",
    //    "Отличная попытка, попробуй ещё раз!",
    //    "Упс! Не в этот раз.",
    //    "Неудачи — это часть пути к победе."
    //};

    [Header("References")]
    [SerializeField] private Board _board;

    [Space]
    [SerializeField] private TMP_Text _messageText;
    [SerializeField] private FadeableButton _nextWordButton;

    private void Awake()
    {
        _messageText.gameObject.SetActive(false);
        MakeMessageTransparent();

        _nextWordButton.Hide(0f);

        _nextWordButton.GetComponent<Button>().onClick.AddListener(() => _board.StartNewGame());
    }

    private void Start()
    {
        _board.OnLetterRemoved += LetterRemovedHandler;
        _board.OnInvalidWord += InvalidWordHandler;
        _board.OnGameOver += GameOverHandler;
        _board.OnNewGameStarted += NewGameStartedHandler;
    }

    private void OnDestroy()
    {
        _board.OnLetterRemoved -= LetterRemovedHandler;
        _board.OnInvalidWord -= InvalidWordHandler;
        _board.OnGameOver -= GameOverHandler;
        _board.OnNewGameStarted -= NewGameStartedHandler;
    }

    public void ShowMessage(string message)
    {
        _messageText.gameObject.SetActive(true);
        _messageText.text = message;
        _messageText.DOFade(1f, FADE_DURATION).From(0f).SetEase(Ease.Linear);
    }

    private void InvalidWordHandler()
    {
        ShowMessage("Слово не найдено в словаре");
    }

    private void LetterRemovedHandler()
    {
        HideMessage();
    }

    private void GameOverHandler(bool isVictory, string word)
    {
        string gameOverMessage = isVictory ?
            VICTORY_MESSAGES[Random.Range(0, VICTORY_MESSAGES.Length)] :
            $"Загаданное слово - <color=#538D4E><b>{word.ToUpper()}</b></color>";

        ShowMessage(gameOverMessage);
        _nextWordButton.Show(FADE_DURATION);
    }

    private void NewGameStartedHandler()
    {
        HideMessage();
        _nextWordButton.Hide(FADE_DURATION);
    }

    private void HideMessage()
    {
        MakeMessageTransparent();
        _messageText.gameObject.SetActive(false);
    }

    private void MakeMessageTransparent()
    {
        Color color = _messageText.color;
        color.a = 0f;
        _messageText.color = color;
    }
}
