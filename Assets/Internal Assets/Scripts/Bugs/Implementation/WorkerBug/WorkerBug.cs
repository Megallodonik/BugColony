using UnityEngine;
using Zenject;

public class WorkerBug : BugBase
{
    [Inject] private BugSpawnerService _bugSpawnerService;
    private WorkerBugConfigScriptableObject _bugConfig;

    public override void Init(BugBaseConfigScriptableObject config)
    {
        _bugConfig = config as WorkerBugConfigScriptableObject;
        if (_bugConfig == null)
        {
            Debug.LogError($"Wrong config attached! expected WorkerBugConfigScriptableObject");
            return;
        }
        base.Init(config);
    }
    public override void Eat(int amount)
    {
        base.Eat(amount);
        if (Saturation >= _bugConfig.SaturationForSeparation)
        {
            _saturation = 0;
            _bugSpawnerService.SeparateBug(this);
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
