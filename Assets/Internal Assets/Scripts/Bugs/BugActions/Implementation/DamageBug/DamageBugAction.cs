using UnityEngine;

[System.Serializable]
public class DamageBugAction : IBugAction
{
    private IEntity _entity;
    private BugBase _bug;
    public void DoAction(IEntity entity, BugBase bug)
    {
        _entity = entity;
        _bug = bug as PredatorBug;
        if (_bug == null) return;
        DamageBug();
    }

    private void DamageBug()
    {
        var enemyBug = _entity as BugBase;
        if (enemyBug != null && enemyBug.isActiveAndEnabled)
        {
            var config = _bug.Config as PredatorBugConfigScriptableObject;
            if (config.Damage >= enemyBug.Health)
            {
                _bug.Eat(config.Damage);
                enemyBug.Damage(config.Damage);
                return;
            }
            _bug.Eat(enemyBug.Health);
            enemyBug.Damage(config.Damage);
        }
    }
}