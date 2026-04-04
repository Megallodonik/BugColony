using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "BugsConfig", menuName = "Configs/BugsConfig")]
public class BugsConfigScriptableObject : ScriptableObject
{
    [SerializeField] private string _starterBugID;
    [SerializeField] private List<BugPresentation> _bugs;

    public List<BugPresentation> Bugs => _bugs;
    public string StarterBugID => _starterBugID;

}
