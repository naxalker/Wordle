using System.Collections.Generic;
using UnityEngine;
using Zenject;
using NaughtyAttributes;

public class ThemeController : IInitializable
{
    private static string DARK_CAMERA_COLOR = "#1D1D1D";
    private static string LIGHT_CAMERA_COLOR = "#E2E2E2";

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

    private List<ThemeableObject> _themeableObjects = new List<ThemeableObject>();
    private Theme _currentTheme = Theme.Dark;

    public void Initialize()
    {
        _themeableObjects.AddRange(Object.FindObjectsOfType<ThemeableObject>(true));
    }

    public void ChangeColors()
    {
        _currentTheme = _currentTheme == Theme.Dark ? Theme.Light : Theme.Dark;

        if (_currentTheme == Theme.Dark)
        {
            if (ColorUtility.TryParseHtmlString(DARK_CAMERA_COLOR, out Color newColor))
            {
                Camera.main.backgroundColor = newColor;
            }
        }
        else if (_currentTheme == Theme.Light)
        {
            if (ColorUtility.TryParseHtmlString(LIGHT_CAMERA_COLOR, out Color newColor))
            {
                Camera.main.backgroundColor = newColor;
            }
        }

        foreach (ThemeableObject themeableObject in _themeableObjects)
        {
            themeableObject.ApplyTheme(_currentTheme);
        }
    }
}
