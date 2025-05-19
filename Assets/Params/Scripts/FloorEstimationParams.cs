using UnityEngine;

[CreateAssetMenu(fileName = "FloorEstimationParams", menuName = "Scriptable Objects/FloorEstimationParams")]
public class FloorEstimationParams : ScriptableObject
{
    public float floorLevelPressureThreshold; // フロアの高さを決定するための閾値
}
