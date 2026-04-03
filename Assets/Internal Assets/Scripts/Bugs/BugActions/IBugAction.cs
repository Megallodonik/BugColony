using System;
using UnityEngine;

public interface IBugAction
{
    public void DoAction(IEntity entity, BugBase bug) {}
}
