using NUnit.Framework;
using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public abstract class BugBase: MonoBehaviour, IDamageble 
{
    public event Action OnDamageTaken;
    public event Action OnHealGained;
    private BugBaseConfigScriptableObject _config;

    private MoveComponent _moveComponent;

    private int _saturation;
    private int _health;

    private bool _followRepetative = false;

    public BugBaseConfigScriptableObject Config => _config;
    public int Saturation => _saturation;
    public int Health => _health;

    public virtual void Init(BugBaseConfigScriptableObject config)
    {
        _config = config;
        _moveComponent = this.GetComponent<MoveComponent>();
        _moveComponent.OnTargetReached += OnTargetReached;
    }
    private void OnDestroy()
    {
        _moveComponent.OnTargetReached -= OnTargetReached;
    }

    public void Interact()
    {
        Damage(0);
    }
    public virtual void Damage(int amount)
    {
        if (_health > 0)
        {
            _health -= amount;
            OnDamageTaken?.Invoke();
        }
    }
    public virtual void Heal(int amount)
    {
        _health += amount;
        OnHealGained?.Invoke();
    }

    public virtual void Eat(int amount)
    {
        _saturation += amount;
    } 

    protected virtual void OnTargetReached(Transform target)
    {
        var entity = target.GetComponent<IEntity>();
        if (_config.DoActionsOnReaching && entity != null)
        {
            InteractWithEntity(entity);
        }
        if (_followRepetative)
        {
            StartChase(_followRepetative);
        }
    }
    protected virtual void InteractWithEntity(IEntity entity)
    {
        foreach (var action in _config.BugActions)
        {
            action.BugAction.DoAction(entity, this);
        }
    }
    public void StartChase(bool repetative)
    {
        _followRepetative = repetative;
        var target = FindClosestTarget(_config.TargetFollowType); 
        if (_moveComponent == null)
        {
            Debug.LogError($"No move component attached to {this.name}");
            return;
        }
        _moveComponent.StartFollowing(target, _config.Speed, _config.ReachingThreshold);
    }


    private Transform FindClosestTarget(Type targetType)
    {
        var results = Physics2D.OverlapCircleAll(transform.position, _config.DetectionRadius, _config.TargetLayer);
        Collider2D closest = null;
        float minDistSq = float.MaxValue;

        for (int i = 0; i < results.Length; i++)
        {
            var target = results[i];
            if (target == null || target.GetComponent(targetType) == null) continue;

            float distSq = (results[i].transform.position - transform.position).sqrMagnitude;
            if (distSq < minDistSq)
            {
                minDistSq = distSq;
                closest = target;
            }
        }
        return closest.transform;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _config.DetectionRadius);
    }
}
