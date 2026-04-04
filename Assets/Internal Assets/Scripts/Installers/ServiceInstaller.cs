
using Zenject;


public class ServiceInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<GameWorldService>().AsSingle();
        Container.Bind<ResourceSpawnerService>().AsSingle();
        Container.Bind<ColonyStats>().AsSingle();
        Container.Bind<BugSpawnerService>().AsSingle();
    }
}