using System;
using UnityEngine;

public class Mushroom : MonoBehaviour, IResource
{
    public event Action<IResource> OnPickingUp;
    private ResourceScriptableObject _resourceData;

    public ResourceScriptableObject ResourceData => _resourceData;

    public void SetResourceData(ResourceScriptableObject resourceData)
    {
        _resourceData = resourceData;
    }
    public void PickUp()
    {
        OnPickingUp?.Invoke(this);
    }
}
