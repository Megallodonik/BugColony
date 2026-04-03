using UnityEngine;

[System.Serializable]
public class TestBugAction : IBugAction
{
    [SerializeField] private int _amount;
    public void DoAction(IEntity entity, BugBase bug)
    {
        var damageble = entity as IDamageble;
        if (damageble != null)
        {
            damageble.Damage(_amount);
        }
    }
}
