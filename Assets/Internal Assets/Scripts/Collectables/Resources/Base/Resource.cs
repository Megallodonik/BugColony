using System;
using UnityEditor.Sprites;
using UnityEngine;
using Zenject;

public class Resource : MonoBehaviour, ICollectable
{
    public event Action OnPickingUp;
    private ResourceScriptableObject _resourceData;

    [Inject] ResourceSpawnerService _resourceService;

    public ResourceScriptableObject ResourceData => _resourceData;

    public void SetResourceData(ResourceScriptableObject resourceData)
    {
        _resourceData = resourceData;
    }
    public void Interact()
    {
        PickUp();
    }

    public virtual void PickUp()
    {
        if (_resourceData == null)
        {
            Debug.LogError($"Resource data is not attached! {this.name}");
            return;
        }
        OnPickingUp?.Invoke();
        _resourceService.ReturnResource(this);
    }

}
