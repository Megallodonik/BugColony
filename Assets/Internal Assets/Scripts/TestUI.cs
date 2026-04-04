using UnityEngine;
using Zenject;

public class TestUI : MonoBehaviour
{
    [Inject] private GameWorldService _gameWorldService;
    [Inject] private ResourceSpawnerService _resourceSpawnerService;
    [Inject] private BugSpawnerService _bugSpawnerService;

    private void Start()
    {
    }

    public void StartGame()
    {
        _gameWorldService.Init();
        _resourceSpawnerService.Init();
        _bugSpawnerService.Init();
        _resourceSpawnerService.StartSpawnLoop();
        bool result;
        _bugSpawnerService.SpawnBug(out result, "worker_bug", new Vector2(0, 0));
    }
}
