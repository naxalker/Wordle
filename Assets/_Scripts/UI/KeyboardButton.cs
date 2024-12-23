using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ThemeableImage))]
[RequireComponent(typeof(Image))]
public class KeyboardButton : MonoBehaviour
{
    private bool _buttonColorChanged;
    private ThemeableImage _themeableImage;
    private Image _background;
    
    public bool ButtonColorChanged => _buttonColorChanged;

    private void Start()
    {
        _themeableImage = GetComponent<ThemeableImage>();
        _background = GetComponent<Image>();
    }

    public void ChangeColor(Color color)
    {
        _buttonColorChanged = true;

        _background.color = color;

        _themeableImage.Lock();
    }
}
