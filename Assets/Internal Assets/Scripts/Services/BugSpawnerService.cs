using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using Zenject;

public class BugSpawnerService
{
    private DiContainer _container;

    [Inject] private ColonyStats _colonyStats; //preventing circular dependency
    private GameWorldConfigScriptableObject _gameWorldConfig;

    private BugsConfigScriptableObject _bugsConfig;
    private Dictionary<string, SimpleObjectPool> _bugPools; // resource id => resource object pool

    public event Action<BugBase> OnBugSpawned;
    public event Action<BugBase> OnBugReturned;
    private CancellationTokenSource _spawnLoopCts;

    private bool _isReady = false;
    public bool IsReady => _isReady;
    public BugSpawnerService(BugsConfigScriptableObject bugsConfig, GameWorldConfigScriptableObject gameWorldConfig, DiContainer container)
    {
        _bugsConfig = bugsConfig;
        _gameWorldConfig = gameWorldConfig;
        _container = container;
    }

    public void Init()
    {
        if (_isReady) return;
        _bugPools = new Dictionary<string, SimpleObjectPool>();
        if (_bugsConfig == null || _bugsConfig.Bugs.Count == 0) return;
        foreach (var bug in _bugsConfig.Bugs)
        {
            var id = bug.BugData.ID;
            var objPool = new SimpleObjectPool(_container);
            if (bug.BugView.gameObject.GetComponent<BugBase>() == null)
            {
                Debug.LogError($"One of bugs does not contain BugBase: {bug.BugData.ID}");
                continue;
            }
            objPool.CreatePool(bug.BugView.gameObject, bug.MaxBugsOnScene, true);
            _bugPools.Add(id, objPool);
        }
        _isReady = true;
    }
    //public void StartSpawnLoop()
    //{
    //    if (!_isReady) return;
    //    _spawnLoopCts?.Cancel();
    //    _spawnLoopCts?.Dispose();
    //    _spawnLoopCts = new CancellationTokenSource();
    //    SpawnLoop(_spawnLoopCts.Token).Forget();
    //}
    public void ReturnBug(BugBase bug)
    {
        var id = bug.Config.ID;
        if (_bugPools.ContainsKey(id))
        {
            _bugPools[id].ReturnObject(bug.gameObject);
            Debug.Log($"bug {bug.name} returned to a pool");
            OnBugReturned?.Invoke(bug);
            return;
        }
        Debug.Log($"cant return bug {bug.Config.ID} to a pool!");
    }
    //private async UniTask SpawnLoop(CancellationToken token)
    //{
    //    while (!token.IsCancellationRequested)
    //    {
    //        try
    //        {
    //            bool result;
    //            SpawnResource(out result);
    //            var message = result ? "Bug spawned!" : "Cant spawn bug";
    //            Debug.Log(message);
    //            await UniTask.Delay(_gameWorldConfig.ResourceSpawnIntervalSeconds * 1000, cancellationToken: token);
    //        }
    //        catch (OperationCanceledException)
    //        {
    //            Debug.Log("Canceled bug spawn loop");
    //        }
    //    }
    //}
    public void SeparateBug(BugBase bug)
    {
        var bugId = bug.Config.ID;
        var presentation = _bugsConfig.Bugs.FirstOrDefault(p => p.BugData.ID == bugId);
        if (presentation == null)
        {
            Debug.LogError($"Cant find bug with id {bugId} in bugs config");
            return;
        }
        if (bug.Config.SeparateOnlyInSelfSimilar)
        {
            bool result;
            SpawnBug(out result, bug.Config.ID, bug.transform.position);
            return;
        }
        var choosenBugPresentation = ChooseBug();
        if (choosenBugPresentation != null)
        {
            bool result;
            SpawnBug(out result, choosenBugPresentation.BugData.ID, bug.transform.position);
            var message = result ? "Bug spawned!" : "Cant spawn bug";
            Debug.Log(message);
        }
    }
    public void SpawnBug(out bool result, string bugId, Vector2 pos)
    {
        result = false;
        var bugPresentation = _bugsConfig.Bugs.FirstOrDefault(b => b.BugData.ID == bugId);
        if (bugPresentation == null) return;
        var obj = _bugPools[bugId].GetObject();
        if (obj != null)
        {
            var bug = obj.GetComponent<BugBase>();
            if (bug != null)
            {
                obj.transform.position = pos;
                result = true;
                OnBugSpawned?.Invoke(bug);
                bug.Init(bugPresentation.BugData);
            }
        }
    }
    private BugPresentation ChooseBug()
    {
        float totalWeight = 0f;
        foreach (var bug in _bugsConfig.Bugs)
        {
            if (bug.TotalAmountOfBugsToStartSpawning <= _colonyStats.BugsAlive)
            {
                totalWeight += bug.BugAppearenceOnSeparationWeight;
            }
        }

        float randomValue = UnityEngine.Random.Range(0f, totalWeight);

        float cumulativeWeight = 0f;
        foreach (var bug in _bugsConfig.Bugs)
        {
            if (bug.TotalAmountOfBugsToStartSpawning <= _colonyStats.BugsAlive)
            {
                cumulativeWeight += bug.BugAppearenceOnSeparationWeight;
                if (randomValue <= cumulativeWeight)
                    return bug;
            }
        }

        return _bugsConfig.Bugs[_bugsConfig.Bugs.Count - 1];
    }
}
