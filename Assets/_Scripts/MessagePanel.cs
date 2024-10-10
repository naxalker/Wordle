using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MessagePanel : MonoBehaviour
{
    private static float FADE_DURATION = .5f;

    [Header("References")]
    [SerializeField] private Board _board;

    [Space]
    [SerializeField] private TMP_Text _messageText;
    [SerializeField] private Button _nextWordButton;

    private PlayerInput _playerInput;

    [Inject]
    private void Construct(PlayerInput playerInput)
    {
        _playerInput = playerInput;
    }

    private void Awake()
    {
        _messageText.gameObject.SetActive(false);
        Color color = _messageText.color;
        color.a = 0f;
        _messageText.color = color;

        _nextWordButton.gameObject.SetActive(false);
        color = _nextWordButton.GetComponent<Image>().color;
        color.a = 0f;
        _nextWordButton.GetComponent<Image>().color = color;

        _nextWordButton.onClick.AddListener(() => _board.StartNewGame());
    }

    private void Start()
    {
        _board.OnInvalidWord += InvalidWordHandler;
        _playerInput.OnKeyPressed += KeyPressedHandler;
    }

    private void OnDestroy()
    {
        _board.OnInvalidWord -= InvalidWordHandler;
        _playerInput.OnKeyPressed -= KeyPressedHandler;
    }

    private void InvalidWordHandler()
    {
        ShowMessage("Слово не найдено в словаре");
    }

    private void KeyPressedHandler(KeyCode code)
    {
        if (code == KeyCode.Backspace)
        {
            HideMessage();
        }
    }

    private void ShowMessage(string message)
    {
        if (_messageText.gameObject.activeInHierarchy)
            return;

        _messageText.gameObject.SetActive(true);
        _messageText.text = message;
        _messageText.DOFade(1f, FADE_DURATION);
    }

    private void HideMessage()
    {
        if (!_messageText.gameObject.activeInHierarchy)
            return;

        _messageText.DOFade(0f, FADE_DURATION)
            .OnComplete(() => _messageText.gameObject.SetActive(false));
    }
}
