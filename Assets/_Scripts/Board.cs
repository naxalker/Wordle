using System;
using System.Linq;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class Board : MonoBehaviour
{
    public event Action OnInvalidWord;
    public event Action<bool> OnGameOver;

    [Header("States")]
    [SerializeField] private Tile.TileState _emptyState;
    [SerializeField] private Tile.TileState _occupiedState;
    [SerializeField] private Tile.TileState _correctState;
    [SerializeField] private Tile.TileState _wrongSpotState;
    [SerializeField] private Tile.TileState _incorrectState;

    private Row[] _rows;

    private string[] _solutions;
    private string[] _validWords;
    private string _word;

    private int _rowIndex;
    private int _columnIndex;

    private PlayerInput _playerInput;

    [Inject]
    private void Construct(PlayerInput playerInput)
    {
        _playerInput = playerInput;
    }

    private void Awake()
    {
        _rows = GetComponentsInChildren<Row>();
    }

    private void Start()
    {
        LoadData();
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
    }

    public void PlaceLetter(char letter)
    {
        Row currentRow = _rows[_rowIndex];

        if (_columnIndex >= currentRow.Tiles.Length)
            return;

        currentRow.Tiles[_columnIndex].SetLetter(letter);
        currentRow.Tiles[_columnIndex].SetState(_occupiedState);
        _columnIndex++;
    }

    public void SubmitWord()
    {
        Row currentRow = _rows[_rowIndex];

        if (_columnIndex >= currentRow.Tiles.Length)
        {
            SubmitRow(currentRow);
        }
    }

    public void RemoveLetter()
    {
        Row currentRow = _rows[_rowIndex];

        _columnIndex = Mathf.Max(_columnIndex - 1, 0);
        currentRow.Tiles[_columnIndex].SetLetter('\0');
        currentRow.Tiles[_columnIndex].SetState(_emptyState);
    }

    private void LoadData()
    {
        TextAsset textFile = Resources.Load("words") as TextAsset;

        _validWords = textFile.text
            .Split('\n')
            .Select(word => word.Trim())
            .ToArray();

        _solutions = _validWords.Take(1000).ToArray();
    }

    private void SetRandomWord()
    {
        _word = _solutions[Random.Range(0, _solutions.Length)];
        _word = _word.ToLower().Trim();
    }

    private void SubmitRow(Row row)
    {
        if (!IsValidWord(row.Word))
        {
            OnInvalidWord?.Invoke();
            return;
        }

        string remaining = _word;

        for (int i = 0; i < row.Tiles.Length; i++)
        {
            Tile tile = row.Tiles[i];

            if (tile.Letter == _word[i])
            {
                tile.SetState(_correctState);

                remaining = remaining.Remove(i, 1);
                remaining = remaining.Insert(i, " ");
            }
            else if (!_word.Contains(tile.Letter))
            {
                tile.SetState(_incorrectState);
            }
        }

        for (int i = 0; i < row.Tiles.Length; i++)
        {
            Tile tile = row.Tiles[i];

            if (tile.State != _correctState && tile.State != _incorrectState)
            {
                if (remaining.Contains(tile.Letter))
                {
                    tile.SetState(_wrongSpotState);

                    int index = remaining.IndexOf(tile.Letter);
                    remaining = remaining.Remove(index, 1);
                    remaining = remaining.Insert(index, " ");
                }
                else
                {
                    tile.SetState(_incorrectState);
                }
            }
        }

        if (HasWon(row))
        {
            OnGameOver?.Invoke(true);
            enabled = false;
        }

        _rowIndex++;
        _columnIndex = 0;

        if (_rowIndex >= _rows.Length)
        {
            OnGameOver?.Invoke(false);
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
                _rows[row].Tiles[col].SetState(_emptyState);
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

    private bool IsValidWord(string word) => _validWords.Contains(word);

    private bool HasWon(Row row) => row.Tiles.All(tile => tile.State == _correctState);
}
