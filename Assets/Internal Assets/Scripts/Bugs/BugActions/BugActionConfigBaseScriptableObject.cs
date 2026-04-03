using UnityEngine;

public class BugActionConfigBaseScriptableObject : ScriptableObject
{
    private IBugAction _action;

    public IBugAction BugAction => _action;

    public void SetAction(IBugAction action)
    {
        _action = action;
    }
}
