using System;
using UnityEngine;

public class ColonyStats
{
    public event Action OnColonyStatsChange;
    private BugSpawnerService _bugSpawnerService;

    private int _workerBugsDead;
    private int _predatorBugsDead;
    private int _bugsDead;

    private int _workerBugsAlive;
    private int _predatorBugsAlive;
    private int _bugsAlive;

    public int WorkerBugsDead => _workerBugsDead;
    public int PredatorBugsDead => _predatorBugsDead;
    public int BugsDead => _bugsDead;

    public int WorkerBugsAlive => _workerBugsAlive;
    public int PredatorBugsAlive => _predatorBugsAlive;
    public int BugsAlive => _bugsAlive;
    public ColonyStats(BugSpawnerService bugSpawnerService)
    {
        _bugSpawnerService = bugSpawnerService;
        _bugSpawnerService.OnBugSpawned += OnBugSpawn;
        _bugSpawnerService.OnBugReturned += OnBugReturned;
    }

    private void OnBugSpawn(BugBase bug)
    {
        switch (bug)
        {
            case WorkerBug w:
                _workerBugsAlive += 1;
                break;
            case PredatorBug pred:
                _predatorBugsAlive += 1;
                break;
        }
        _bugsAlive += 1;
        OnColonyStatsChange?.Invoke();
    }
    private void OnBugReturned(BugBase bug)
    {
        switch (bug)
        {
            case WorkerBug w:
                _workerBugsDead += 1;
                _workerBugsAlive -= 1;
                break;
            case PredatorBug pred:
                _predatorBugsDead += 1;
                _predatorBugsAlive -= 1;
                break;

        }
        _bugsDead += 1;
        _bugsAlive -= 1;
        OnColonyStatsChange?.Invoke();
    }
}
