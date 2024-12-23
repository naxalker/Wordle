using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Debugger : MonoBehaviour
{
    private ThemeController _themeController;

    [Inject]
    private void Construct(ThemeController themeController)
    {
        _themeController = themeController;
    }

    [Button("ChangeTheme")]
    private void ChangeTheme()
    {
        _themeController.ChangeColors();
    }
}
