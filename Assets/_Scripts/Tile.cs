using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class Tile : MonoBehaviour
{
    public static event Action<Tile> OnTileChangedState;

    public char Letter { get; private set; }
    public TileState State { get; private set; }

    private TMP_Text _text;
    private Image _fill;
    private Outline _outline;

    private TileColorsSO _tileColors;

    [Inject]
    private void Construct(TileColorsSO tileColors)
    {
        _tileColors = tileColors;
    }

    private void Awake()
    {
        _text = GetComponentInChildren<TMP_Text>();
        _fill = GetComponent<Image>();
        _outline = GetComponent<Outline>();
    }

    public void SetLetter(char letter)
    {
        Letter = letter;
        _text.text = letter.ToString();
    }

    public void SetState(TileState state)
    {
        State = state;

        TileColorConfig tileConfig = _tileColors.Values.FirstOrDefault(tile => tile.TileState == state);
        _fill.color = tileConfig.FillColor;
        _outline.effectColor = tileConfig.OutlineColor;

        OnTileChangedState?.Invoke(this);
    }
}
