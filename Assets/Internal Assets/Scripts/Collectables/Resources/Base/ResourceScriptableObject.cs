using UnityEngine;

[CreateAssetMenu(fileName = "Resource", menuName = "GameElements/Resource")]
public class ResourceScriptableObject : ScriptableObject
{
    //This script could be a regular class instead of a Scriptable object. However, as the number of resources and settings increases,

    //configuring regular classes in huge lists becomes inconvenient.
    
    [SerializeField] private string _id;
    [SerializeField] private int _nutritionalValue;
    //more settings can be added
    public string ID => _id;
    public int NutrinionalValue => _nutritionalValue;
}
