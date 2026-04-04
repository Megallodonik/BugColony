using Zenject;
using UnityEngine;
public class ConfigInstaller : MonoInstaller
{
    [SerializeField] private GameWorldConfigScriptableObject _gameWorldConfig = null;
    [SerializeField] private ResourcesConfigScriptableObject _resourcesConfig = null;
    [SerializeField] private BugsConfigScriptableObject _bugsConfig = null;
    public override void InstallBindings()
    {
        Container.BindInstance(_gameWorldConfig);
        Container.BindInstance(_resourcesConfig);
        Container.BindInstance(_bugsConfig);
    }
}