using UnityEngine;
using Zenject;

public class PlayerProgressInstaller : MonoInstaller
{
    [SerializeField] private Board _board;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<WordsController>().AsSingle().WithArguments(_board);
    }
}