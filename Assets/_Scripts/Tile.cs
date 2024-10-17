using DG.Tweening;
using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class Tile : MonoBehaviour
{
    public static event Action<Tile> OnTileChangedState;

    private const float TIME_TO_FLIP = .5f;

    public char Letter { get; private set; }
    public TileState State { get; private set; }

    private TMP_Text _text;
    private Image _fill;
    private Outline _outline;

    private TileColorsSO _tileColors;
    private RectTransform _rectTransform;
    private Color _fillColor;
    private Color _outlineColor;

    public Color FillColor => _fillColor;
    public Color OutlineColor => _outlineColor;

    [Inject]
    private void Construct(TileColorsSO tileColors)
    {
        _tileColors = tileColors;
    }

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
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
        _fillColor = tileConfig.FillColor;
        _outlineColor = tileConfig.OutlineColor;

        OnTileChangedState?.Invoke(this);
    }

    public IEnumerator Flip()
    {
        float elapsedTime = 0f;

        while (elapsedTime <= TIME_TO_FLIP / 2)
        {
            _rectTransform.localScale = Vector3.Lerp(Vector3.one, new Vector3(1f, 0f, 1f), elapsedTime / (TIME_TO_FLIP / 2));

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        _fill.color = _fillColor;
        _outline.effectColor = _outlineColor;

        elapsedTime = 0f;

        while (elapsedTime <= TIME_TO_FLIP / 2)
        {
            _rectTransform.localScale = Vector3.Lerp(new Vector3(1f, 0f, 1f), Vector3.one, elapsedTime / (TIME_TO_FLIP / 2));

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        _rectTransform.localScale = Vector3.one;
    }
}
