using UnityEngine;

[CreateAssetMenu(fileName = "DamageBugActionConfig", menuName = "BugActions/DamageBugActionConfig")]
public class DamageBugActionConfigScriptableObject : BugActionConfigBaseScriptableObject
{
    [SerializeField] private DamageBugAction _action;

    private void OnEnable()
    {
        SetAction(_action);
    }
}