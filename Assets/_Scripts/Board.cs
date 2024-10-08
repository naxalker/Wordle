using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Board : MonoBehaviour
{
    [Header("States")]
    [SerializeField] private Tile.TileState _emptyState;
    [SerializeField] private Tile.TileState _occupiedState;
    [SerializeField] private Tile.TileState _correctState;
    [SerializeField] private Tile.TileState _wrongSpotState;
    [SerializeField] private Tile.TileState _incorrectState;

    [Header("UI")]
    [SerializeField] private TMP_Text _invalidWordText;
    [SerializeField] private Button _tryAgainButton;
    [SerializeField] private Button _newWordButton;

    private Row[] _rows;

    private string[] _solutions;
    private string[] _validWords;
    private string _word;

    private int _rowIndex;
    private int _columnIndex;

    private void Awake()
    {
        _rows = GetComponentsInChildren<Row>();

        _tryAgainButton.onClick.AddListener(() => TryAgain());
        _newWordButton.onClick.AddListener(() => NewGame());
    }

    private void OnEnable()
    {
        _tryAgainButton.gameObject.SetActive(false);
        _newWordButton.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        _tryAgainButton.gameObject.SetActive(true);
        _newWordButton.gameObject.SetActive(true);
    }

    private void Start()
    {
        LoadData();
        NewGame();
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

        _invalidWordText.gameObject.SetActive(false);
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
            _invalidWordText.gameObject.SetActive(true);
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
            enabled = false;
        }

        _rowIndex++;
        _columnIndex = 0;

        if (_rowIndex >= _rows.Length)
        {
            enabled = false;
        }
    }

    private void NewGame()
    {
        ClearBoard();
        SetRandomWord();

        enabled = true;
    }

    private void TryAgain()
    {
        ClearBoard();
        enabled = true;
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

    private bool IsValidWord(string word) => _validWords.Contains(word);

    private bool HasWon(Row row) => row.Tiles.All(tile => tile.State == _correctState);
}
