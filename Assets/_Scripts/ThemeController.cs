using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ThemeController : IInitializable
{
    private Dictionary<string, string> _themeColors = new Dictionary<string, string>()
    {
        { "#000000", "#FFFFFF" },
        { "#FFFFFF", "#000000" },

        { "#1D1D1D", "#E2E2E2" },
        { "#E2E2E2", "#1D1D1D" },

        { "#3A3A3C", "#C5C5C3" },
        { "#C5C5C3", "#3A3A3C" },

        { "#565758", "#A9A8A7" },
        { "#A9A8A7", "#565758" },

        { "#9C9C9C", "#636363" },
        { "#636363", "#9C9C9C" },
    };

    private List<TMP_Text> texts = new List<TMP_Text>();
    private List<Image> images = new List<Image>();

    public void Initialize()
    {
        texts.AddRange(Object.FindObjectsOfType<TMP_Text>(true));
        images.AddRange(Object.FindObjectsOfType<Image>(true));

        foreach(var text in texts)
        {
            Debug.Log(text.text);
        }
    }

    public void ChangeColors()
    {
        string currentCameraColorHex = "#" + ColorUtility.ToHtmlStringRGB(Camera.main.backgroundColor);
        if (_themeColors.ContainsKey(currentCameraColorHex))
        {
            string newCameraColorHex = _themeColors[currentCameraColorHex];

            if (ColorUtility.TryParseHtmlString(newCameraColorHex, out Color newCameraColor))
            {
                Camera.main.backgroundColor = newCameraColor;
            }
        }

        foreach (TMP_Text text in texts)
        {
            string currentColorHex = "#" + ColorUtility.ToHtmlStringRGB(text.color);

            if (_themeColors.ContainsKey(currentColorHex))
            {
                string newColorHex = _themeColors[currentColorHex];

                if (ColorUtility.TryParseHtmlString(newColorHex, out Color newColor))
                {
                    text.color = newColor;
                }
            }
        }

        foreach (Image image in images)
        {
            string currentColorHex = "#" + ColorUtility.ToHtmlStringRGB(image.color);

            if (_themeColors.ContainsKey(currentColorHex))
            {
                string newColorHex = _themeColors[currentColorHex];

                if (ColorUtility.TryParseHtmlString(newColorHex, out Color newColor))
                {
                    image.color = newColor;
                }
            }
        }
    }
}
