using UnityEngine;

[CreateAssetMenu(fileName = "PredatorBugConfig", menuName = "GameElements/PredatorBugConfig")]
public class PredatorBugConfigScriptableObject : WorkerBugConfigScriptableObject
{
    [SerializeField] private int _damage;
    [SerializeField] private int _lifeTimeInSeconds = 3;

    public int Damage => _damage;
    public int LifeTimeInSeconds => _lifeTimeInSeconds; 
}
