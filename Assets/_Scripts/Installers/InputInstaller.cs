using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class InputInstaller : MonoInstaller
{
    [SerializeField] private Board _board;
    [SerializeField] private Button[] _letterButtons;
    [SerializeField] private Button _clearButton;
    [SerializeField] private Button _submitButton;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<PlayerInput>().AsSingle().WithArguments(_board, _letterButtons, _clearButton, _submitButton);
    }
}