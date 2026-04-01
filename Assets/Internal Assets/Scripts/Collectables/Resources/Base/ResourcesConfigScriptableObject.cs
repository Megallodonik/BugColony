using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ResourcesConfig", menuName = "Configs/ResourcesConfig")]
public class ResourcesConfigScriptableObject : ScriptableObject
{
    [SerializeField] private List<ResourcePresentation> _resources;

    public List<ResourcePresentation> Resources => _resources;
}
