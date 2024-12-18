using Zenject;

public class ThemeControllerInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<ThemeController>().AsSingle();
    }
}