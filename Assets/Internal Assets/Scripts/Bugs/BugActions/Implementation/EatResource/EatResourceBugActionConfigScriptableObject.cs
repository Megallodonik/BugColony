using UnityEngine;
[CreateAssetMenu(fileName = "EatResourceBugActionConfig", menuName = "BugActions/EatResourceBugActionConfig")]
public class EatResourceBugActionConfigScriptableObject : BugActionConfigBaseScriptableObject
{
    [SerializeField] private EatResourceBugAction _action;

    private void OnEnable()
    {
        SetAction(_action);
    }
}