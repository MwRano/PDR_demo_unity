using UnityEngine;

[CreateAssetMenu(fileName = "FloorEstimationParameters", menuName = "Scriptable Objects/FloorEstimationParameters")]
public class FloorEstimationParameters : ScriptableObject
{
    public float floorLevelPressureThreshold; // フロアの高さを決定するための閾値
}
