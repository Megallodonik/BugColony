using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "BugBaseConfig", menuName = "GameElements/BugBaseConfig")]
public class BugBaseConfigScriptableObject : ScriptableObject
{
    //[SerializeReference] private IBugAction _bugAction; <= very good solution, but requires Odin Inspector or difficult custom editor classes
    [SerializeField] private List<BugActionConfigBaseScriptableObject> _bugActions;
    [SerializeField] private bool _doActionsOnReaching = true;
    [SerializeField] private LayerMask _targetLayer;
    [SerializeField, Range(0, 999999)] private float _detectionRadius;
    [SerializeField, Range(0, 999999)] private float _speed;
    [SerializeField, Range(0, 999999)] private float _reachingThreshold;
    [SerializeField] private bool _seekForTargetRepetative = true;

    //public IBugAction BugAction => _bugAction;

    public List<BugActionConfigBaseScriptableObject> BugActions => _bugActions;
    public bool DoActionsOnReaching => _doActionsOnReaching;
    public LayerMask TargetLayer => _targetLayer;
    public float DetectionRadius => _detectionRadius;
    public float Speed => _speed;
    public float ReachingThreshold => _reachingThreshold;
    public bool SeekForTargetRepetative => _seekForTargetRepetative;

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
#if UNITY_EDITOR
    [CustomEditor(typeof(BugBaseConfigScriptableObject))]
    public class TargetTypeSOEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var so = (BugBaseConfigScriptableObject)target;

            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => typeof(IEntity).IsAssignableFrom(t))
                .ToArray();

            int currentIndex = Array.FindIndex(types, t => t.AssemblyQualifiedName == so.TypeName);
            int newIndex = EditorGUILayout.Popup("Target Type", currentIndex, types.Select(t => t.Name).ToArray());

            if (newIndex >= 0 && newIndex != currentIndex)
            {
                so.GetType().GetField("_typeName", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    .SetValue(so, types[newIndex].AssemblyQualifiedName);
                EditorUtility.SetDirty(so);
            }
        }
    }
#endif
}
