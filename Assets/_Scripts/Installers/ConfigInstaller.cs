using UnityEngine;
using Zenject;

public class ConfigInstaller : MonoInstaller
{
    [SerializeField] private TileColorsSO _tileColors;

    public override void InstallBindings()
    {
        Container.Bind<TileColorsSO>().FromScriptableObject(_tileColors).AsSingle();
    }
}