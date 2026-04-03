using UnityEngine;

public class TestBug : BugBase
{
    [SerializeField] private BugBaseConfigScriptableObject _config;

    private void Start()
    {
        Init(_config);
        
    }
    public void StartGame()
    {
        StartChase(_config.SeekForTargetRepetative);
    }
    private void Update()
    {
        Debug.Log($"Bug : {this.name}" +
            $"Saturation : {Saturation}");
    }
}
