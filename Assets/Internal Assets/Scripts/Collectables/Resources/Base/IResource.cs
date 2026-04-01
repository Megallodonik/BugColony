using System;
using UnityEditor.Sprites;
using UnityEngine;

public interface IResource : ICollectable
{
    public event Action<IResource> OnPickingUp;
    public ResourceScriptableObject ResourceData { get; }
}
