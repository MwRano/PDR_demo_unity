using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FloorMapParam", menuName = "Scriptable Objects/FloorMapParam")]
public class FloorMapParam : ScriptableObject
{
    [SerializeField] List<FloorMapData> floorMapDataList;
}
