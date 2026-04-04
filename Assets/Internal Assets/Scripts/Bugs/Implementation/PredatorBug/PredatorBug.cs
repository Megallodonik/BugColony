using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using Zenject;

public class PredatorBug : BugBase
{
    [Inject] private BugSpawnerService _bugSpawnerService;
    private PredatorBugConfigScriptableObject _bugConfig;
    private CancellationTokenSource _cts;

    public override void Init(BugBaseConfigScriptableObject config)
    {
        _bugConfig = config as PredatorBugConfigScriptableObject;
        if (_bugConfig == null)
        {
            Debug.LogError($"Wrong config attached! expected PredatorBugConfigScriptableObject");
            return;
        }

        base.Init(config);

        _cts?.Cancel();
        _cts = new CancellationTokenSource();

        StartDeathTimer(_cts.Token).Forget();
    }

    private async UniTaskVoid StartDeathTimer(CancellationToken token)
    {
        try
        {
            await UniTask.Delay(_bugConfig.LifeTimeInSeconds * 1000, cancellationToken: token);
            Damage(Health); 
        }
        catch (OperationCanceledException) { }
    }

    private void OnDestroy()
    {
        _cts?.Cancel();
        _cts?.Dispose();
    }
    public override void Eat(int amount)
    {
        base.Eat(amount);
        if (Saturation >= _bugConfig.SaturationForSeparation)
        {
            _saturation = 0;
            _bugSpawnerService.SeparateBug(this);
            _cts?.Cancel();
            _cts = new CancellationTokenSource();

            StartDeathTimer(_cts.Token).Forget();
        }
    }
    public override void Damage(int amount)
    {
        base.Damage(amount);
        if (Health <= 0)
        {
            _bugSpawnerService.ReturnBug(this);
        }
    }

}
