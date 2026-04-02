using UnityEngine;
using Zenject;

public class TestUI : MonoBehaviour
{
    [Inject] private GameWorldService _gameWorldService;
    [Inject] private ResourceSpawnerService _resourceSpawnerService;

    private void Start()
    {
    }

    public void StartGame()
    {
        _gameWorldService.Init();
        _resourceSpawnerService.Init();
        _resourceSpawnerService.StartSpawnLoop();
    }
}
