using UnityEngine;

[CreateAssetMenu(fileName = "PDRParams", menuName = "Scriptable Objects/PDRParams")]
public class PDRParams : ScriptableObject
{
    public float stepLength;
    public float stepThreshold; // ステップ検出の閾値（加速度の変化量）
    public float rotationSpeedFactor; // ジャイロの回転速度にかける係数

    [Header("Weinberg Stride")]
    public float weinbergK = 0.4f; // Weinberg式の係数
    public float minStrideLength = 0.2f; // 歩幅の下限
    public float maxStrideLength = 1.2f; // 歩幅の上限
    public float strideSmoothing = 0.2f; // 0-1で歩幅の平滑化

    [Header("Calibration")]
    public int calibrationSteps = 0; // 0の場合はキャリブレーションしない
    public float calibrationDistanceMeters = 0f; // キャリブレーション時の既知距離
}
