using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Keyboard : MonoBehaviour
{
    [SerializeField] private Board _board;
    [SerializeField] private Button[] _letterButtons;
    [SerializeField] private Button _clearButton;
    [SerializeField] private Button _submitButton;

    private void Awake()
    {
        foreach (Button button in _letterButtons)
        {
            button.onClick.AddListener(() =>
                _board.PlaceLetter(button.GetComponentInChildren<TMP_Text>().text[0]));
        }

        _clearButton.onClick.AddListener(() => _board.RemoveLetter());
        _submitButton.onClick.AddListener(() => _board.SubmitWord());
    }
}
