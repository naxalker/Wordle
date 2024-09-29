using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    [Serializable]
    public class TileState
    {
        public Color FillColor;
        public Color OutlineColor;
    }

    public char Letter { get; private set; }
    public TileState State { get; private set; }

    private TMP_Text _text;
    private Image _fill;
    private Outline _outline;

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
        _fill.color = state.FillColor;
        _outline.effectColor = state.OutlineColor;
    }
}
