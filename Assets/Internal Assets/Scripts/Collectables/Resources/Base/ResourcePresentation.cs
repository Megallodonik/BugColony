using UnityEngine;

[System.Serializable]
public class ResourcePresentation
{
    [SerializeField] private ResourceView _resourceView;
    [SerializeField] private ResourceScriptableObject _resourceData;

    [SerializeField, Range(0, Mathf.Infinity), Tooltip("the more weight, the greater the chance of appearence")] private int _resourceAppearenceWeight;
    [SerializeField, Range(1, Mathf.Infinity)] private int _maxResourcesOnScene;
    public ResourceView ResourceView => _resourceView;
    public ResourceScriptableObject ResourceData => _resourceData;
    public int ResourceAppearenceWeight => _resourceAppearenceWeight;
    public int MaxResourcesOnscene => _maxResourcesOnScene;
}
