using UnityEngine;

[CreateAssetMenu(fileName = "GameWorldConfig", menuName = "Configs/GameWorldConfig")]
public class GameWorldConfigScriptableObject : ScriptableObject
{
    [SerializeField] private Vector2 _worldSize;
    [SerializeField, Range(1, 999999)] private int _resourceSpawnIntervalSeconds;

    public Vector2 WorldSize => _worldSize;
    public int ResourceSpawnIntervalSeconds => _resourceSpawnIntervalSeconds;
}
