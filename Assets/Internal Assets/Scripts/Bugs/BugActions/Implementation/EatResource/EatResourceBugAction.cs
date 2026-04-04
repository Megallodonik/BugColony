using UnityEngine;

[System.Serializable]
public class EatResourceBugAction : IBugAction
{
    private IEntity _entity;
    private BugBase _bug;
    public void DoAction(IEntity entity, BugBase bug)
    {
        _entity = entity;
        _bug = bug;
        EatResource();
    }

    private void EatResource()
    {
        var resource = _entity as Resource;
        if (resource != null && resource.isActiveAndEnabled)
        {
            resource.PickUp();
            var amount = resource.ResourceData.NutrinionalValue;
            _bug.Eat(amount);
        }
    }
}