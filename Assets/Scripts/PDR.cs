using UnityEngine;
using TMPro;

public class PDR : MonoBehaviour
{
    public float stepThreshold = 0.2f; // ステップ検出の閾値（加速度の変化量）
    public float stepSize = 0.01f;      // 1ステップあたりの移動距離
    public float rotationSpeedFactor = 1.0f; // ジャイロの回転速度にかける係数

    private Vector3 lastAcceleration;
    private bool isStepping = false;
    private float cumulativeYaw = 0f; // Z軸回りの累積回転角度
    private Vector3 position; // 初期位置

    public TMP_Text playerPosText; // TextMeshProのUI要素
    public TMP_Text headingText; // TextMeshProのUI要素
    Transform movableObject; // 位置を反映させるGameObject
    public Vector3 initPos; // 初期位置
    public Vector3 initHeading; // 初期向き

    void OnEnable()
    {
        // センサーを有効にする
        Input.gyro.enabled = true;

        // 初期加速度を記録 
        lastAcceleration = Input.acceleration;

        // 初期位置と向きを設定
        //position = initPos; // 初期位置を設定
        cumulativeYaw = Mathf.Atan2(initHeading.y, initHeading.x); // 初期向きを設定

        movableObject = gameObject.transform; // 位置を反映させるGameObjectを取得

        // 初期位置をGameObjectに反映
        if (movableObject != null)
        {
            //movableObject.position = position;
            position = movableObject.position; // 初期位置をGameObjectの位置に設定
            movableObject.rotation = Quaternion.Euler(0, 0, cumulativeYaw * Mathf.Rad2Deg - 90); // 初期向きを反映
        }
        else
        {
            Debug.LogError("MovableObjectがアサインされていません。");
        }
    }

    void Update()
    {
        // ジャイロセンサーによるZ軸回りの回転の累積
        cumulativeYaw += Input.gyro.rotationRate.z * Time.deltaTime * rotationSpeedFactor;

        // 加速度センサーによるステップ検出（簡易的な実装）
        float accelerationChange = Mathf.Abs(Input.acceleration.magnitude - lastAcceleration.magnitude);

        if (accelerationChange > stepThreshold && !isStepping)
        {
            isStepping = true;
            // 現在の向きで前方（XY平面）に移動
            Vector3 forward = new Vector3(Mathf.Cos(cumulativeYaw), Mathf.Sin(cumulativeYaw), 0).normalized;
            position += forward * stepSize;
        }
        else if (accelerationChange < stepThreshold * 0.5f) // 閾値を下回ったらステップ終了とみなす
        {
            isStepping = false;
        }

        lastAcceleration = Input.acceleration;

        // GameObjectの位置と向きを更新
        if (movableObject != null)
        {
            movableObject.position = position;
            movableObject.rotation = Quaternion.Euler(0, 0, cumulativeYaw * Mathf.Rad2Deg - 90); // 向きを反映
        }

        // デバッグ用に現在の位置を表示
        playerPosText.text = $"Position: X={position.x:F2}, Y={position.y:F2}";
        headingText.text = $"Heading: {cumulativeYaw * Mathf.Rad2Deg:F2}°"; // ラジアンを度に変換して表示
    }
}
