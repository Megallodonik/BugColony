using UnityEngine;

public interface IDamageble : IEntity
{
    public int Health { get; }
    public void Damage(int amount);
    public void Heal(int amount);
}
