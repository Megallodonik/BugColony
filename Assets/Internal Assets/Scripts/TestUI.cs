using UnityEngine;
using Zenject;

public class TestUI : MonoBehaviour
{
    [Inject] private GameWorldService _gameWorldService;
    [Inject] private ResourceSpawnerService _resourceSpawnerService;
    [Inject] private BugSpawnerService _bugSpawnerService;
    [Inject] private ColonyStats _colonyStats;

    [SerializeField] private TMPro.TextMeshProUGUI _predatorDeadBugs;
    [SerializeField] private TMPro.TextMeshProUGUI _workerDeadBugs;

    private void Start()
    {
    }
    private void OnDestroy()
    {
        _colonyStats.OnColonyStatsChange -= OnColonyStatsChange;
    }
    private void OnColonyStatsChange()
    {
        _predatorDeadBugs.text = $"predator bugs dead: {_colonyStats.PredatorBugsDead.ToString()}";
        _workerDeadBugs.text = $"worker bugs dead: {_colonyStats.WorkerBugsDead.ToString()}";
    }
    public void StartGame()
    {
        _gameWorldService.Init();
        _resourceSpawnerService.Init();
        _bugSpawnerService.Init();
        _resourceSpawnerService.StartSpawnLoop();

        _colonyStats.OnColonyStatsChange += OnColonyStatsChange;

        bool result;
        _bugSpawnerService.SpawnBug(out result, "worker_bug", new Vector2(0, 0));
    }


}
