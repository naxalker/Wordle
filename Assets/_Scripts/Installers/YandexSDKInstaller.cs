using UnityEngine;
using Zenject;

public class YandexSDKInstaller : MonoInstaller
{
    [SerializeField] private Board _board;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<AdController>().AsSingle().WithArguments(_board);
        Container.BindInterfacesAndSelfTo<LeaderboardController>().AsSingle();
    }
}