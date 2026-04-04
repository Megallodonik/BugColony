using System;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class BugPresentation
{
    [SerializeField] private BugView _bugView;
    [SerializeField] private BugBaseConfigScriptableObject _bugData;
    [SerializeField] private int _maxBugsOnScene;
    [SerializeField, Range(0, 999999), Tooltip("the more weight, the greater the chance of appearence")] private int _bugAppearenceOnSeparationWeight;
    [SerializeField, Range(0, 999999)] private int _totalAmountOfBugsToStartSpawning;

    public BugView BugView => _bugView;
    public BugBaseConfigScriptableObject BugData => _bugData;
    public int MaxBugsOnScene => _maxBugsOnScene;
    public int BugAppearenceOnSeparationWeight => _bugAppearenceOnSeparationWeight;
    public int TotalAmountOfBugsToStartSpawning => _totalAmountOfBugsToStartSpawning;

}
