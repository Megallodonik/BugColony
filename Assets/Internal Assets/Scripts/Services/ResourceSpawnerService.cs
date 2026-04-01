using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ResourceSpawnerService
{
    private DiContainer _container;

    private ResourcesConfigScriptableObject _resourcesConfig;
    private Dictionary<string, SimpleObjectPool> _resourcePools;
    public ResourceSpawnerService(ResourcesConfigScriptableObject resourcesConfig, DiContainer container)
    {
        _resourcesConfig = resourcesConfig;
        _container = container;
    }

    public void Init()
    {
        _resourcePools = new Dictionary<string, SimpleObjectPool>();
        if (_resourcesConfig == null || _resourcesConfig.Resources.Count == 0) return;
        foreach (var resource in _resourcesConfig.Resources)
        {
            var id = resource.ResourceData.ID;
            var objPool = new SimpleObjectPool(_container);
            objPool.CreatePool(resource.ResourceView.gameObject, resource.MaxResourcesOnscene, false);
            _resourcePools.Add(id, objPool);
        }
    }
    private void SpawnLoop()
    {
        var resourcePresentation = ChooseResource();
        if (resourcePresentation == null) return;
        var id = resourcePresentation.ResourceData.ID;
        var obj = _resourcePools[id].GetObject();
        if (obj != null)
        {
            //
        }
    }
    private ResourcePresentation ChooseResource()
    {
        float totalWeight = 0f;
        foreach (var resource in _resourcesConfig.Resources)
        {
            totalWeight += resource.ResourceAppearenceWeight;
        }

        float randomValue = Random.Range(0f, totalWeight);

        float cumulativeWeight = 0f;
        foreach (var resource in _resourcesConfig.Resources)
        {
            cumulativeWeight += resource.ResourceAppearenceWeight;
            if (randomValue <= cumulativeWeight)
                return resource;
        }

        return _resourcesConfig.Resources[_resourcesConfig.Resources.Count - 1];
    }
}
