using UnityEngine;

[CreateAssetMenu(fileName = "TestBugActionConfig", menuName = "BugActions/TestBugActionConfig")]
public class TestBugActionConfigScriptableObject : BugActionConfigBaseScriptableObject
{
    [SerializeField] private TestBugAction _action;

    private void OnEnable()
    {
        SetAction(_action);
    }
}