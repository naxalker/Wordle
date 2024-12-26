using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PlayerInput : IInitializable, ITickable, IDisposable
{
    public static readonly Dictionary<KeyCode, char> ENGLISH_TO_RUSSIAN_MAP = new Dictionary<KeyCode, char>()
    {
        { KeyCode.A, '�' }, { KeyCode.B, '�' }, { KeyCode.C, '�' },
        { KeyCode.D, '�' }, { KeyCode.E, '�' }, { KeyCode.F, '�' },
        { KeyCode.G, '�' }, { KeyCode.H, '�' }, { KeyCode.I, '�' },
        { KeyCode.J, '�' }, { KeyCode.K, '�' }, { KeyCode.L, '�' },
        { KeyCode.M, '�' }, { KeyCode.N, '�' }, { KeyCode.O, '�' },
        { KeyCode.P, '�' }, { KeyCode.Q, '�' }, { KeyCode.R, '�' },
        { KeyCode.S, '�' }, { KeyCode.T, '�' }, { KeyCode.U, '�' },
        { KeyCode.V, '�' }, { KeyCode.W, '�' }, { KeyCode.X, '�' },
        { KeyCode.Y, '�' }, { KeyCode.Z, '�' }, { KeyCode.LeftBracket, '�' },
        { KeyCode.RightBracket, '�' }, { KeyCode.Semicolon, '�' },
        { KeyCode.Quote, '�' }, { KeyCode.Comma, '�' }, { KeyCode.Period, '�' }
    };

    private static readonly KeyCode[] SUPPORTED_KEYS = new KeyCode[]
    {
        KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D, KeyCode.E, KeyCode.F,
        KeyCode.G, KeyCode.H, KeyCode.I, KeyCode.J, KeyCode.K, KeyCode.L,
        KeyCode.M, KeyCode.N, KeyCode.O, KeyCode.P, KeyCode.Q, KeyCode.R,
        KeyCode.S, KeyCode.T, KeyCode.U, KeyCode.V, KeyCode.W, KeyCode.X,
        KeyCode.Y, KeyCode.Z, KeyCode.LeftBracket, KeyCode.RightBracket, 
        KeyCode.Semicolon, KeyCode.Quote, KeyCode.Comma, KeyCode.Period,
        KeyCode.Backspace, KeyCode.Return
    };

    public event Action<KeyCode> OnKeyPressed;
    public event Action OnButtonPressed;

    private Board _board;
    private Button[] _letterButtons;
    private Button _clearButton;
    private Button _submitButton;

    private Dictionary<char, Button> _letterToButton = new Dictionary<char, Button>();

    public PlayerInput(Board board, Button[] letterButtons, Button clearButton, Button submitButton)
    {
        _board = board;
        _letterButtons = letterButtons;
        _clearButton = clearButton;
        _submitButton = submitButton;
    }

    public void Initialize()
    {
        foreach (Button button in _letterButtons)
        {
            char letter = button.GetComponentInChildren<TMP_Text>().text[0];
            _letterToButton[letter] = button;
            button.onClick.AddListener(() =>
            {
                _board.PlaceLetter(letter);
                OnButtonPressed?.Invoke();
            });
        }

        _clearButton.onClick.AddListener(() => _board.RemoveLetter());
        _submitButton.onClick.AddListener(() => _board.SubmitWord());
        Tile.OnTileChangedState += TileChangedStateHandler;
        _board.OnNewGameStarted += NewGameStartedHandler;
    }

    public void Dispose()
    {
        Tile.OnTileChangedState -= TileChangedStateHandler;
        _board.OnNewGameStarted -= NewGameStartedHandler;
    }

    public void Tick()
    {
        foreach (KeyCode keyCode in SUPPORTED_KEYS)
        {
            if (Input.GetKeyDown(keyCode))
            {
                OnKeyPressed?.Invoke(keyCode);
            }
        }
    }

    private void TileChangedStateHandler(Tile tile)
    {
        if (tile.State == TileState.EmptyState || tile.State == TileState.OccupiedState) { return; }
        
        Button letterButton = _letterToButton[tile.Letter];

        if (letterButton.TryGetComponent(out KeyboardButton keyboardButton))
        {
            if (tile.State == TileState.CorrectState || keyboardButton.ColorHasChanged == false)
            {
                keyboardButton.ChangeColor(tile.FillColor);
            }
        }
    }

    private void NewGameStartedHandler()
    {
        foreach (Button button in _letterButtons)
        {
            if (button.TryGetComponent(out KeyboardButton keyboardButton))
            {
                keyboardButton.ResetButton();
            }
        }
    }
}
