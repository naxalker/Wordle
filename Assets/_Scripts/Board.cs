using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class Board : MonoBehaviour
{
    public event Action OnNewGameStarted;
    public event Action OnLetterPlaced;
    public event Action OnLetterRemoved;
    public event Action OnInvalidWord;
    public event Action OnValidWordEntered;
    public event Action<bool, string> OnGameOver;

    private Row[] _rows;

    private string _word;

    private int _rowIndex;
    private int _columnIndex;

    private PlayerInput _playerInput;
    private WordsController _playerProgress;

    [Inject]
    private void Construct(PlayerInput playerInput, WordsController playerProgress)
    {
        _playerInput = playerInput;
        _playerProgress = playerProgress;
    }

    private List<string> UnguessedWords => _playerProgress.UnguessedWords;
    private string[] ValidWords => _playerProgress.ValidWords;

    private void Awake()
    {
        _rows = GetComponentsInChildren<Row>();
    }

    private void Start()
    {
        StartNewGame();
    }

    private void OnEnable()
    {
        _playerInput.OnKeyPressed += KeyPressedHandler;
    }

    private void OnDisable()
    {
        _playerInput.OnKeyPressed -= KeyPressedHandler;
    }

    public void StartNewGame()
    {
        ClearBoard();
        SetRandomWord();

        enabled = true;

        OnNewGameStarted?.Invoke();
    }

    public void PlaceLetter(char letter)
    {
        if (!enabled)
            return;

        Row currentRow = _rows[_rowIndex];

        if (_columnIndex >= currentRow.Tiles.Length)
            return;

        currentRow.Tiles[_columnIndex].SetLetter(letter);
        currentRow.Tiles[_columnIndex].SetState(TileState.OccupiedState);
        _columnIndex++;

        OnLetterPlaced?.Invoke();
    }

    public void SubmitWord()
    {
        if (!enabled)
            return;

        Row currentRow = _rows[_rowIndex];

        if (_columnIndex >= currentRow.Tiles.Length)
        {
            StartCoroutine(SubmitRow(currentRow));
        }
    }

    public void RemoveLetter()
    {
        if (!enabled)
            return;

        Row currentRow = _rows[_rowIndex];

        _columnIndex = Mathf.Max(_columnIndex - 1, 0);
        currentRow.Tiles[_columnIndex].SetLetter('\0');
        currentRow.Tiles[_columnIndex].SetState(TileState.EmptyState);

        OnLetterRemoved?.Invoke();
    }

    private void SetRandomWord()
    {
        _word = UnguessedWords[Random.Range(0, UnguessedWords.Count)];
        _word = _word.ToLower().Trim();

        Debug.Log(_word);
    }

    private IEnumerator SubmitRow(Row row)
    {
        if (!IsValidWord(row.Word))
        {
            row.Shake();

            OnInvalidWord?.Invoke();
            yield break;
        }

        OnValidWordEntered?.Invoke();

        string remaining = _word;

        for (int i = 0; i < row.Tiles.Length; i++)
        {
            Tile tile = row.Tiles[i];

            if (tile.Letter == _word[i])
            {
                tile.SetState(TileState.CorrectState);

                remaining = remaining.Remove(i, 1);
                remaining = remaining.Insert(i, " ");
            }
            else if (!_word.Contains(tile.Letter))
            {
                tile.SetState(TileState.IncorrectState);
            }
        }

        for (int i = 0; i < row.Tiles.Length; i++)
        {
            Tile tile = row.Tiles[i];

            if (tile.State != TileState.CorrectState && tile.State != TileState.IncorrectState)
            {
                if (remaining.Contains(tile.Letter))
                {
                    tile.SetState(TileState.WrongSpotState);

                    int index = remaining.IndexOf(tile.Letter);
                    remaining = remaining.Remove(index, 1);
                    remaining = remaining.Insert(index, " ");
                }
                else
                {
                    tile.SetState(TileState.IncorrectState);
                }
            }
        }

        foreach(Tile tile in row.Tiles)
        {
            yield return StartCoroutine(tile.Flip());
        }

        if (HasWon(row))
        {
            OnGameOver?.Invoke(true, row.Word);
            enabled = false;
        }

        _rowIndex++;
        _columnIndex = 0;

        if (_rowIndex >= _rows.Length)
        {
            OnGameOver?.Invoke(false, row.Word);
            enabled = false;
        }
    }

    private void ClearBoard()
    {
        for (int row = 0; row < _rows.Length; row++)
        {
            for (int col = 0; col < _rows[row].Tiles.Length; col++)
            {
                _rows[row].Tiles[col].SetLetter('\0');
                _rows[row].Tiles[col].SetState(TileState.EmptyState);
            }
        }

        _rowIndex = 0;
        _columnIndex = 0;
    }

    private void KeyPressedHandler(KeyCode keyCode)
    {
        if (keyCode >= KeyCode.A && keyCode <= KeyCode.Z)
        {
            PlaceLetter(PlayerInput.ENGLISH_TO_RUSSIAN_MAP[keyCode]);
        }
        else if (keyCode == KeyCode.Return)
        {
            SubmitWord();
        }
        else if (keyCode == KeyCode.Backspace)
        {
            RemoveLetter();
        }
    }

    private bool IsValidWord(string word) => ValidWords.Contains(word);

    private bool HasWon(Row row) => row.Tiles.All(tile => tile.State == TileState.CorrectState);
}
