using UnityEngine;

[CreateAssetMenu(fileName = "WorkerBugConfig", menuName = "GameElements/WorkerBugConfig")]
public class WorkerBugConfigScriptableObject : BugBaseConfigScriptableObject
{
    [SerializeField] private int _saturationForSeparation = 20;

    public int SaturationForSeparation => _saturationForSeparation;
}
