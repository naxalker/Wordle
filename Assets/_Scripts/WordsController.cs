using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class WordsController : IInitializable, IDisposable
{
    private const int WORDS_TO_GUESS_AMOUNT = 1000;
    private const string UNGUESSED_WORDS_KEY = "UnguessedWords";

    private List<string> _unguessedWords;
    private string[] _validWords;

    private Board _board;

    public WordsController(Board board)
    {
        _board = board;
    }

    public List<string> UnguessedWords => _unguessedWords;
    public string[] ValidWords => _validWords;
    public int GuessedWordsAmount => WORDS_TO_GUESS_AMOUNT - _unguessedWords.Count;

    public void Initialize()
    {
        LoadValidWords();

        if (PlayerPrefs.HasKey(UNGUESSED_WORDS_KEY))
        {
            _unguessedWords = LoadProgress();
        }
        else
        {
            _unguessedWords = _validWords.Take(WORDS_TO_GUESS_AMOUNT).ToList();
        }

        _board.OnGameOver += GameOverHandler;
    }

    public void Dispose()
    {
        _board.OnGameOver -= GameOverHandler;
    }

    private void GameOverHandler(bool isVictory, string word)
    {
        if (isVictory)
        {
            _unguessedWords.Remove(word);
            SaveProgress();
        }
    }

    private void LoadValidWords()
    {
        TextAsset textFile = Resources.Load("words") as TextAsset;

        _validWords = textFile.text
            .Split('\n')
            .Select(word => word.Trim())
            .ToArray();
    }

    private void SaveProgress()
    {
        string joinedString = string.Join(",", _unguessedWords.ToArray());
        PlayerPrefs.SetString(UNGUESSED_WORDS_KEY, joinedString);
    }

    private List<string> LoadProgress()
    {
        string savedString = PlayerPrefs.GetString(UNGUESSED_WORDS_KEY);

        return savedString.Split(',').ToList();
    }
}
