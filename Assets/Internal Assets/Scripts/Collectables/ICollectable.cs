using System;
using UnityEngine;

public interface ICollectable : IEntity
{
    public event Action OnPickingUp;
    public void PickUp();
}
