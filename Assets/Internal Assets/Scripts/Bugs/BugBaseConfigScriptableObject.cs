using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "BugBaseConfig", menuName = "GameElements/BugBaseConfig")]
public class BugBaseConfigScriptableObject : ScriptableObject
{
    //[SerializeReference] private IBugAction _bugAction; <= very good solution, but requires Odin Inspector or difficult custom editor classes
    //public IBugAction BugAction => _bugAction;

    [SerializeField] private List<BugActionConfigBaseScriptableObject> _bugActions;
    [SerializeField] private bool _doActionsOnReaching = true;

    [SerializeField] private LayerMask _targetLayer;
    [SerializeField, Range(0, 999999)] private float _detectionRadius;

    [SerializeField, Range(0, 999999)] private int _health;

    [SerializeField, Range(0, 999999)] private float _speed;
    [SerializeField, Range(0, 999999)] private float _reachingThreshold;
    [SerializeField] private bool _seekForTargetRepetative = true;

    [SerializeField, Range(0, 999999), Tooltip("the delay after which the bug will begin to move when it appears")] private int _appearenceDelayInSeconds;

    [SerializeField] private bool _separateOnlyInSelfSimilar = false;

    [SerializeField] private string _id;

    public List<BugActionConfigBaseScriptableObject> BugActions => _bugActions;
    public bool DoActionsOnReaching => _doActionsOnReaching;
    public LayerMask TargetLayer => _targetLayer;
    public float DetectionRadius => _detectionRadius;
    public int Health => _health;
    public float Speed => _speed;
    public float ReachingThreshold => _reachingThreshold;
    public bool SeekForTargetRepetative => _seekForTargetRepetative;

    public int AppearenceDelayInSeconds => _appearenceDelayInSeconds;

    public bool SeparateOnlyInSelfSimilar => _separateOnlyInSelfSimilar;
    public string ID => _id;

    private string _typeName;

    public string TypeName => _typeName;

    private Type _cachedType;
    public Type TargetFollowType
    {
        get
        {
            if (_cachedType == null && !string.IsNullOrEmpty(_typeName))
            {
                _cachedType = Type.GetType(_typeName);
                if (_cachedType == null)
                {
                    Debug.LogError($"Type {_typeName} not found!");
                }
                else if (!typeof(IEntity).IsAssignableFrom(_cachedType))
                {
                    Debug.LogError($"{_typeName} must implement IEnemy!");
                }
            }
            return _cachedType;
        }
    }
}
