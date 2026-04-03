using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Zenject;

public class ResourceSpawnerService
{
    private DiContainer _container;

    private GameWorldConfigScriptableObject _gameWorldConfig;

    private ResourcesConfigScriptableObject _resourcesConfig;
    private Dictionary<string, SimpleObjectPool> _resourcePools; // resource id => resource object pool

    public event Action<Vector2> OnResourceSpawned;
    private CancellationTokenSource _spawnLoopCts;

    private bool _isReady = false;
    public bool IsReady => _isReady;
    public ResourceSpawnerService(ResourcesConfigScriptableObject resourcesConfig, GameWorldConfigScriptableObject gameWorldConfig, DiContainer container)
    {
        _resourcesConfig = resourcesConfig;
        _gameWorldConfig = gameWorldConfig;
        _container = container;
    }

    public void Init()
    {
        if (_isReady) return;
        _resourcePools = new Dictionary<string, SimpleObjectPool>();
        if (_resourcesConfig == null || _resourcesConfig.Resources.Count == 0) return;
        foreach (var resource in _resourcesConfig.Resources)
        {
            var id = resource.ResourceData.ID;
            var objPool = new SimpleObjectPool(_container);
            if (resource.ResourceView.gameObject.GetComponent<Resource>() == null)
            {
                Debug.LogError($"One of resources does not contain IResource: {resource.ResourceData.ID}");
                continue;
            }
            objPool.CreatePool(resource.ResourceView.gameObject, resource.MaxResourcesOnscene, true);
            _resourcePools.Add(id, objPool);
        }
        _isReady = true;
    }
    public void StartSpawnLoop()
    {
        if (!_isReady) return;
        _spawnLoopCts?.Cancel();
        _spawnLoopCts?.Dispose();
        _spawnLoopCts = new CancellationTokenSource();
        SpawnLoop(_spawnLoopCts.Token).Forget();
    }
    public void ReturnResource(Resource resource)
    {
        var id = resource.ResourceData.ID;
        if (_resourcePools.ContainsKey(id))
        {
            _resourcePools[id].ReturnObject(resource.gameObject);
            Debug.Log($"resource {resource.name} returned to a pool");
            return;
        }
        Debug.Log($"cant return resource {resource.ResourceData.ID} to a pool!");
    }
    private async UniTask SpawnLoop(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            try
            {
                bool result;
                SpawnResource(out result);
                var message = result ? "Resource spawned!" : "Cant spawn resource";
                Debug.Log(message);
                await UniTask.Delay(_gameWorldConfig.ResourceSpawnIntervalSeconds * 1000, cancellationToken: token);
            }
            catch (OperationCanceledException)
            {
                Debug.Log("Canceled resource spawn loop");
            }
        }
    }
    private void SpawnResource(out bool result)
    {
        result = false;
        var resourcePresentation = ChooseResource();
        if (resourcePresentation == null) return;
        var id = resourcePresentation.ResourceData.ID;
        var obj = _resourcePools[id].GetObject();
        if (obj != null)
        {
            var resource = obj.GetComponent<Resource>();
            if (resource != null)
            {
                resource.SetResourceData(resourcePresentation.ResourceData);
            }
            var posX = UnityEngine.Random.Range(-(_gameWorldConfig.WorldSize.x / 2), _gameWorldConfig.WorldSize.x / 2);
            var posY = UnityEngine.Random.Range(-(_gameWorldConfig.WorldSize.y / 2), _gameWorldConfig.WorldSize.y / 2);
            var pos = new Vector2(posX, posY);
            obj.transform.position = pos;
            result = true;
            OnResourceSpawned?.Invoke(pos);
        }
    }
    private ResourcePresentation ChooseResource()
    {
        float totalWeight = 0f;
        foreach (var resource in _resourcesConfig.Resources)
        {
            totalWeight += resource.ResourceAppearenceWeight;
        }

        float randomValue = UnityEngine.Random.Range(0f, totalWeight);

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
