using UnityEngine;
using TMPro; // TextMeshProを使用するための名前空間

public class PedestrianDeadReckoning : MonoBehaviour
{
    public float stepThreshold = 0.1f; // ステップ検出の閾値（加速度の変化量）
    public float stepSize = 0.5f;      // 1ステップあたりの移動距離
    public float rotationSpeedFactor = 5.0f; // ジャイロの回転速度にかける係数
    public Transform movableObject;     // 動かすオブジェクト

    private Vector3 lastAcceleration;
    private bool isStepping = false;
    private float cumulativeYaw = 0f; // Z軸回りの累積回転角度

    public TMP_Text playerPosText; 

    void Start()
    {
        // センサーを有効にする
        Input.gyro.enabled = true;

        // 初期回転を記録
        if (movableObject != null)
        {
            cumulativeYaw = movableObject.eulerAngles.z;
        }
        else
        {
            Debug.LogError("MovableObjectがアサインされていません。");
            enabled = false; // スクリプトを無効にする
        }

        // 初期加速度を記録
        lastAcceleration = Input.acceleration;
    }

    void Update()
    {
        if (movableObject == null) return;

        // ジャイロセンサーによるZ軸回りの回転の累積
        cumulativeYaw += Input.gyro.rotationRate.z * Time.deltaTime * rotationSpeedFactor;

        // 現在の回転をZ軸回りの回転に限定
        movableObject.rotation = Quaternion.Euler(0, 0, cumulativeYaw);

        // 加速度センサーによるステップ検出（簡易的な実装）
        float accelerationChange = Mathf.Abs(Input.acceleration.magnitude - lastAcceleration.magnitude);

        if (accelerationChange > stepThreshold && !isStepping)
        {
            isStepping = true;
            // 現在の向きで前方（X-Y平面）に移動
            Vector3 forward = new Vector3(movableObject.forward.x, movableObject.forward.y, 0).normalized;
            movableObject.Translate(forward * stepSize, Space.World);
        }
        else if (accelerationChange < stepThreshold * 0.5f) // 閾値を下回ったらステップ終了とみなす
        {
            isStepping = false;
        }

        lastAcceleration = Input.acceleration;

        playerPosText.text = $"Player Position:\nX: {movableObject.position.x:F2}\nY: {movableObject.position.y:F2}\nZ: {movableObject.position.z:F2}";
    }
}