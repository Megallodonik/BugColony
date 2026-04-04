#if UNITY_EDITOR
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BugBaseConfigScriptableObject), true)]
public class TargetTypeSoEditor : Editor
{
    private bool _showTargetTypeDropdown = false;
    private string[] _typeNames;
    private Type[] _types;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var so = (BugBaseConfigScriptableObject)target;

        _showTargetTypeDropdown = EditorGUILayout.Foldout(_showTargetTypeDropdown, "Target Type Selector");

        if (_showTargetTypeDropdown)
        {
            if (_types == null || _typeNames == null)
            {
                _types = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(a => a.GetTypes())
                    .Where(t => typeof(IEntity).IsAssignableFrom(t))
                    .ToArray();

                _typeNames = _types.Select(t => t.Name).ToArray();
            }

            int currentIndex = Array.FindIndex(_types, t => t.AssemblyQualifiedName == so.TypeName);
            int newIndex = EditorGUILayout.Popup("Target Type", currentIndex, _typeNames);

            if (newIndex >= 0 && newIndex != currentIndex)
            {
                var field = typeof(BugBaseConfigScriptableObject)
                    .GetField("_typeName", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

                if (field != null)
                {
                    field.SetValue(so, _types[newIndex].AssemblyQualifiedName);
                    EditorUtility.SetDirty(so);
                }
            }
        }
    }
}
#endif