using UnityEngine;

public class TestBug : BugBase
{
    [SerializeField] private BugBaseConfigScriptableObject _bugConfig;

    private void Start()
    {
        Init(_bugConfig);
        
    }
    public void StartGame()
    {
        StartChase(_bugConfig.SeekForTargetRepetative);
    }
    private void Update()
    {
        Debug.Log($"Bug : {this.name}" +
            $"Saturation : {Saturation}");
    }
}
