using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PlayerInput : IInitializable, ITickable
{
    public static readonly Dictionary<KeyCode, char> ENGLISH_TO_RUSSIAN_MAP = new Dictionary<KeyCode, char>()
    {
        { KeyCode.A, 'ô' }, { KeyCode.B, 'è' }, { KeyCode.C, 'ñ' },
        { KeyCode.D, 'â' }, { KeyCode.E, 'ó' }, { KeyCode.F, 'à' },
        { KeyCode.G, 'ï' }, { KeyCode.H, 'ð' }, { KeyCode.I, 'ø' },
        { KeyCode.J, 'î' }, { KeyCode.K, 'ë' }, { KeyCode.L, 'ä' },
        { KeyCode.M, 'ü' }, { KeyCode.N, 'ò' }, { KeyCode.O, 'ù' },
        { KeyCode.P, 'ç' }, { KeyCode.Q, 'é' }, { KeyCode.R, 'ê' },
        { KeyCode.S, 'û' }, { KeyCode.T, 'å' }, { KeyCode.U, 'ã' },
        { KeyCode.V, 'ì' }, { KeyCode.W, 'ö' }, { KeyCode.X, '÷' },
        { KeyCode.Y, 'í' }, { KeyCode.Z, 'ÿ' }
    };

    private static readonly KeyCode[] SUPPORTED_KEYS = new KeyCode[]
    {
        KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D, KeyCode.E, KeyCode.F,
        KeyCode.G, KeyCode.H, KeyCode.I, KeyCode.J, KeyCode.K, KeyCode.L,
        KeyCode.M, KeyCode.N, KeyCode.O, KeyCode.P, KeyCode.Q, KeyCode.R,
        KeyCode.S, KeyCode.T, KeyCode.U, KeyCode.V, KeyCode.W, KeyCode.X,
        KeyCode.Y, KeyCode.Z, KeyCode.Backspace, KeyCode.Return
    };

    public event Action<KeyCode> OnKeyPressed;

    private Board _board;
    private Button[] _letterButtons;
    private Button _clearButton;
    private Button _submitButton;

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
            button.onClick.AddListener(() =>
                _board.PlaceLetter(button.GetComponentInChildren<TMP_Text>().text[0]));
        }

        _clearButton.onClick.AddListener(() => _board.RemoveLetter());
        _submitButton.onClick.AddListener(() => _board.SubmitWord());
    }

    public void Tick()
    {
        foreach(KeyCode keyCode in SUPPORTED_KEYS)
        {
            if (Input.GetKeyDown(keyCode))
            {
                OnKeyPressed?.Invoke(keyCode);
            }
        }
    }
}
