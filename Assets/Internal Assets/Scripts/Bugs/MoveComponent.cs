using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public class MoveComponent : MonoBehaviour 
{
    public event Action<Transform> OnTargetReached;
    private Transform _target;
    private float _speed = 5f;
    private float _reachingThreshold = 0.01f;

    private CancellationTokenSource _cts;

    public void StartFollowing(Transform target, float speed, float reachingThreshold)
    {
        if (speed <= 0 || target == null || reachingThreshold < 0)
        {
            Debug.LogError($"Trying to follow target with incorrect data! (No targets available)");
            return;
        }
        StopFollowing();
        _speed = speed;
        _target = target;
        _reachingThreshold = reachingThreshold;
        _cts = new CancellationTokenSource();
        FollowLoop(_cts.Token).Forget();
    }

    public void StopFollowing()
    {
        if (_cts != null)
        {
            _cts.Cancel();
            _cts.Dispose();
            _cts = null;
        }
    }

    private async UniTaskVoid FollowLoop(CancellationToken token)
    {
        try
        {
            while (!token.IsCancellationRequested && _target != null)
            {
                float distance = Vector2.Distance(transform.position, _target.position);

                if (distance <= _reachingThreshold)
                {
                    OnTargetReached?.Invoke(_target);
                }
                transform.position = Vector2.MoveTowards(
                    transform.position,
                    _target.position,
                    _speed * Time.deltaTime
                );

                await UniTask.Yield(PlayerLoopTiming.Update, token);
            }
        }
        catch (OperationCanceledException)
        {

        }
    }

    private void OnDestroy()
    {
        StopFollowing(); 
    }
    private void OnDisable()
    {
        StopFollowing();
    }
}
