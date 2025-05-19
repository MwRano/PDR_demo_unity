using UnityEngine;

[CreateAssetMenu(fileName = "Parameters", menuName = "Scriptable Objects/PDRParameters")]
public class PDRParameters : ScriptableObject
{
    public float stepLength;
    public float stepThreshold; // ステップ検出の閾値（加速度の変化量）
    public float rotationSpeedFactor; // ジャイロの回転速度にかける係数
}
